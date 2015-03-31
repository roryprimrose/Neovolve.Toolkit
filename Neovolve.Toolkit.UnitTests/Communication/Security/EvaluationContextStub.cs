namespace Neovolve.Toolkit.UnitTests.Communication.Security
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IdentityModel.Claims;
    using System.IdentityModel.Policy;

    /// <summary>
    /// The <see cref="EvaluationContextStub"/>
    ///   class provides a stub for an <see cref="EvaluationContext"/> implementation.
    /// </summary>
    internal class EvaluationContextStub : EvaluationContext
    {
        /// <summary>
        ///   Stores the properties of the context.
        /// </summary>
        private readonly IDictionary<String, Object> _properties;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "EvaluationContextStub" /> class.
        /// </summary>
        public EvaluationContextStub()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationContextStub"/> class.
        /// </summary>
        /// <param name="createProperties">
        /// If set to <c>true</c> [create properties].
        /// </param>
        public EvaluationContextStub(Boolean createProperties)
        {
            if (createProperties)
            {
                _properties = new Dictionary<String, Object>();
            }
        }

        /// <summary>
        /// Adds a set of claims to the evaluation context.
        /// </summary>
        /// <param name="policy">
        /// An <see cref="T:System.IdentityModel.Policy.IAuthorizationPolicy"/> that represents the authorization policy that is adding claims to the evaluation context.
        /// </param>
        /// <param name="claimSet">
        /// A <see cref="T:System.IdentityModel.Claims.ClaimSet"/> that contains a set of claims.
        /// </param>
        public override void AddClaimSet(IAuthorizationPolicy policy, ClaimSet claimSet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the date and time at which this <see cref="T:System.IdentityModel.Policy.EvaluationContext"/> is no longer valid.
        /// </summary>
        /// <param name="expirationTime">
        /// A <see cref="T:System.DateTime"/> value that indicates the date and time when this <see cref="T:System.IdentityModel.Policy.AuthorizationContext"/> object is no longer valid.
        /// </param>
        public override void RecordExpirationTime(DateTime expirationTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Gets a read-only collection of <see cref = "T:System.IdentityModel.Claims.ClaimSet" /> objects that contains the claims added by authorization policies that have been evaluated.
        /// </summary>
        /// <value>
        ///   The claim sets.
        /// </value>
        /// <returns>
        ///   A <see cref = "T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> objects that contains the claims added by authorization policies that have been evaluated.
        /// </returns>
        public override ReadOnlyCollection<ClaimSet> ClaimSets
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Gets the number of times that claims have been added to the evaluation context.
        /// </summary>
        /// <value>
        ///   The generation.
        /// </value>
        /// <returns>
        ///   The number of times that claims have been added to the evaluation context.
        /// </returns>
        public override Int32 Generation
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Gets a collection of non-claim properties associated with this <see cref = "T:System.IdentityModel.Policy.EvaluationContext" />.
        /// </summary>
        /// <value>
        ///   The properties.
        /// </value>
        /// <returns>
        ///   A <see cref = "T:System.Collections.Generic.Dictionary`2" /> that specifies a collection of non-claim properties.
        /// </returns>
        public override IDictionary<String, Object> Properties
        {
            get
            {
                return _properties;
            }
        }
    }
}