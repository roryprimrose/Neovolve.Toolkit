namespace Neovolve.Toolkit.Workflow
{
    using System;
    using Neovolve.Toolkit.Workflow.Activities;
    using Neovolve.Toolkit.Workflow.Extensions;

    /// <summary>
    /// The <see cref="InstanceHandler&lt;T&gt;"/>
    ///   class is used to provide instance handling logic for a <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/> instance.
    /// </summary>
    /// <typeparam name="T">
    /// The type of instance to handle.
    /// </typeparam>
    [Serializable]
    public class InstanceHandler<T>
    {
        /// <summary>
        ///   Stores whether a resolution attempt has been made on this instance.
        /// </summary>
        [NonSerialized]
        private Boolean _resolveAttemptMade;

        /// <summary>
        ///   Stores the resolved instance.
        /// </summary>
        [NonSerialized]
        private T _resolvedInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceHandler{T}"/> class.
        /// </summary>
        /// <param name="resolutionName">
        /// The handle description.
        /// </param>
        public InstanceHandler(String resolutionName)
        {
            InstanceHandlerId = Guid.NewGuid();
            ResolutionName = resolutionName;
        }

        /// <summary>
        ///   Gets the instance.
        /// </summary>
        /// <value>
        ///   The instance.
        /// </value>
        public T Instance
        {
            get
            {
                if (_resolveAttemptMade)
                {
                    return _resolvedInstance;
                }

                _resolveAttemptMade = true;

                _resolvedInstance = InstanceManagerExtension.Resolve(this);

                return _resolvedInstance;
            }
        }

        /// <summary>
        ///   Gets the instance handler id.
        /// </summary>
        /// <value>
        ///   The instance handler id.
        /// </value>
        public Guid InstanceHandlerId
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the name of the resolution.
        /// </summary>
        /// <value>
        ///   The name of the resolution.
        /// </value>
        public String ResolutionName
        {
            get;
            private set;
        }
    }
}