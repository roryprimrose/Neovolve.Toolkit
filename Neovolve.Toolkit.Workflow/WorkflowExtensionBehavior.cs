namespace Neovolve.Toolkit.Workflow
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Activities;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using Neovolve.Toolkit.Workflow.Properties;

    /// <summary>
    /// The <see cref="WorkflowExtensionBehavior"/>
    ///   class provides support for adding a workflow extension to a <see cref="WorkflowServiceHost"/> instance.
    /// </summary>
    public class WorkflowExtensionBehavior : IServiceBehavior
    {
        /// <summary>
        ///   Stores the extension type to use.
        /// </summary>
        private readonly Type _extensionType;

        /// <summary>
        ///   Stores the is singleton value.
        /// </summary>
        private readonly Boolean _isSingleton;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowExtensionBehavior"/> class.
        /// </summary>
        /// <param name="extensionType">
        /// Type of the extension.
        /// </param>
        /// <param name="isSingleton">
        /// If set to <c>true</c> the extension will be added as [is singleton].
        /// </param>
        public WorkflowExtensionBehavior(Type extensionType, Boolean isSingleton)
        {
            Contract.Requires<ArgumentNullException>(extensionType != null);

            _extensionType = extensionType;
            _isSingleton = isSingleton;
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description of the service.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host of the service.
        /// </param>
        /// <param name="endpoints">
        /// The service endpoints.
        /// </param>
        /// <param name="bindingParameters">
        /// Custom objects to which binding elements have access.
        /// </param>
        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
            // Nothing to do
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The host that is currently being built.
        /// </param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            WorkflowServiceHost serviceHost = serviceHostBase as WorkflowServiceHost;

            if (serviceHost == null)
            {
                throw new ArgumentException(Resources.WorkflowExtensionBehavior_InvalidServiceHostType);
            }

            if (_isSingleton)
            {
                Contract.Assume(_extensionType != null);

                Object extensionInstance = Activator.CreateInstance(_extensionType);

                serviceHost.WorkflowExtensions.Add(extensionInstance);
            }
            else
            {
                Func<Object> createExtension = BuildCreateExtension(_extensionType);
                Type[] parameterTypes = new[]
                                        {
                                            _extensionType
                                        };

                // Find the generic based add method
                MethodInfo addMethod = serviceHost.WorkflowExtensions.GetType().GetMethods().Where(x => x.Name == "Add" && x.IsGenericMethod).Single();
                MethodInfo genericAddMethod = addMethod.MakeGenericMethod(parameterTypes);
                Object[] parameters = new[]
                                      {
                                          createExtension
                                      };

                genericAddMethod.Invoke(serviceHost.WorkflowExtensions, parameters);
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">
        /// The service description.
        /// </param>
        /// <param name="serviceHostBase">
        /// The service host that is currently being constructed.
        /// </param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Nothing to do
        }

        /// <summary>
        /// Builds the create extension.
        /// </summary>
        /// <param name="extensionType">
        /// Type of the extension.
        /// </param>
        /// <returns>
        /// A <see cref="Func&lt;T&gt;"/> instance.
        /// </returns>
        private static Func<Object> BuildCreateExtension(Type extensionType)
        {
            Type originalType = typeof(WorkflowExtensionWrapper<Object>);

            Contract.Assume(originalType.IsGenericType);

            Type wrapperTypeDefinition = originalType.GetGenericTypeDefinition();
            Type[] genericTypeArguments = new[]
                                          {
                                              extensionType
                                          };

            Contract.Assume(wrapperTypeDefinition.IsGenericTypeDefinition);
            Contract.Assume(wrapperTypeDefinition.GetGenericArguments().Length == genericTypeArguments.Length);

            Type wrapperType = wrapperTypeDefinition.MakeGenericType(genericTypeArguments);

            Object wrapperInstance = Activator.CreateInstance(wrapperType);
            MethodInfo generateFunction = wrapperType.GetMethod("GenerateCreateExtensionWrapper");

            return (Func<Object>)generateFunction.Invoke(wrapperInstance, null);
        }
    }
}