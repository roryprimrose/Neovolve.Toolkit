namespace Neovolve.Toolkit.Reflection
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// The <see cref="MethodCacheKey"/>
    ///   struct is used to describe a method for referencing method information in a cache.
    /// </summary>
    internal struct MethodCacheKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCacheKey"/> struct.
        /// </summary>
        /// <param name="declaringType">
        /// Type of the declaring.
        /// </param>
        /// <param name="methodName">
        /// Name of the method.
        /// </param>
        /// <param name="parameterTypes">
        /// The parameter types.
        /// </param>
        public MethodCacheKey(Type declaringType, String methodName, params Type[] parameterTypes)
            : this()
        {
            Contract.Requires<ArgumentNullException>(declaringType != null);
            Contract.Requires<ArgumentNullException>(String.IsNullOrWhiteSpace(methodName) == false);

            DeclaringType = declaringType;
            MethodName = methodName;

            // The Type.GetMethod checks for null types so if there are no types provided, this value needs to be an empty array
            if (parameterTypes == null)
            {
                parameterTypes = Type.EmptyTypes;
            }

            ParameterTypes = new ReadOnlyCollection<Type>(parameterTypes.Where(x => x != null).ToList());
        }

        /// <summary>
        /// Returns a description of the method signature.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a description of the method signature.
        /// </returns>
        [Pure]
        public override String ToString()
        {
            Contract.Ensures(String.IsNullOrWhiteSpace(Contract.Result<String>()) == false);

            String declaringTypeName = DeclaringType.Name;
            String methodName = MethodName;
            String output = String.Concat(declaringTypeName, ".", methodName, "(");

            if (ParameterTypes != null)
            {
                for (Int32 index = 0; index < ParameterTypes.Count; index++)
                {
                    Type parameterType = ParameterTypes[index];

                    Contract.Assume(parameterType != null);

                    if (index > 0)
                    {
                        output = String.Concat(output, ", ", parameterType.Name);
                    }
                    else
                    {
                        output = String.Concat(output, parameterType.Name);
                    }
                }
            }

            output = String.Concat(output, ")");

            return output;
        }

        /// <summary>
        /// Defines the contracts that ensure the state of this instance is correct.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(DeclaringType != null);
            Contract.Invariant(String.IsNullOrWhiteSpace(MethodName) == false);
            Contract.Invariant(ParameterTypes != null);
        }

        /// <summary>
        ///   Gets the type of the declaring.
        /// </summary>
        /// <value>
        ///   The type of the declaring.
        /// </value>
        public Type DeclaringType
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the name of the method.
        /// </summary>
        /// <value>
        ///   The name of the method.
        /// </value>
        public String MethodName
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the parameter types.
        /// </summary>
        /// <value>
        ///   The parameter types.
        /// </value>
        public ReadOnlyCollection<Type> ParameterTypes
        {
            get;
            private set;
        }
    }
}