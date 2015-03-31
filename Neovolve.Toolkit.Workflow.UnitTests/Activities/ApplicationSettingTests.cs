namespace Neovolve.Toolkit.Workflow.UnitTests.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Storage;
    using Neovolve.Toolkit.Unity;
    using Neovolve.Toolkit.Workflow.Activities;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="ApplicationSettingTests"/>
    ///   class is used to test the <see cref="ApplicationSetting{T}"/> class.
    /// </summary>
    [TestClass]
    public class ApplicationSettingTests
    {
        /// <summary>
        /// Runs test for application setting returns stored value from dependency injected configuration store.
        /// </summary>
        [TestMethod]
        public void ApplicationSettingReturnsStoredValueFromDependencyInjectedConfigurationStoreTest()
        {
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            String key = Guid.NewGuid().ToString();
            Guid defaultValue = Guid.NewGuid();
            const Boolean IsRequired = false;
            Guid expected = Guid.NewGuid();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();
            UnityConfigurationSection section = TestHelper.CreateUnitySection(String.Empty);
            RegisterElement registerElement = new RegisterElement
                                              {
                                                  TypeName = typeof(IConfigurationStore).AssemblyQualifiedName, 
                                                  MapToName = typeof(ConfigurationStoreMockWrapper).AssemblyQualifiedName
                                              };

            // Mock out the factory that creates the configuration store on UnityContainerResolver.TryResolve
            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            // Configure the unity section
            section.Containers[0].Registrations.Add(registerElement);

            // Configure the store to return the unity configuration we require
            store.Stub(x => x.GetSection<UnityConfigurationSection>(UnityConfigurationSection.SectionName)).Return(section);

            inputParameters["Key"] = key;
            inputParameters["DefaultValue"] = defaultValue;
            inputParameters["IsRequired"] = IsRequired;

            try
            {
                Activity<Guid> target = ActivityStore.Resolve<ApplicationSetting<Guid>>();

                store.Stub(x => x.GetApplicationSetting(key, defaultValue, IsRequired)).Return(expected);

                try
                {
                    ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

                    Guid actual = WorkflowInvoker.Invoke(target, inputParameters);

                    Assert.AreEqual(expected, actual, "ApplicationSetting returned an incorrect value");
                    store.AssertWasCalled(x => x.GetApplicationSetting(key, defaultValue, IsRequired));
                }
                finally
                {
                    ConfigurationStoreFactory.StoreType = null;
                }
            }
            finally
            {
                DomainContainer.Destroy();
                ConfigurationStoreFactory.StoreType = null;
            }
        }

        /// <summary>
        /// Runs test for application setting returns stored value.
        /// </summary>
        [TestMethod]
        public void ApplicationSettingReturnsStoredValueTest()
        {
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            String key = Guid.NewGuid().ToString();
            Guid defaultValue = Guid.NewGuid();
            const Boolean IsRequired = false;
            Guid expected = Guid.NewGuid();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            inputParameters["Key"] = key;
            inputParameters["DefaultValue"] = defaultValue;
            inputParameters["IsRequired"] = IsRequired;

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
            Activity<Guid> target = ActivityStore.Resolve<ApplicationSetting<Guid>>();

            store.Stub(x => x.GetApplicationSetting(key, defaultValue, IsRequired)).Return(expected);

            try
            {
                ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

                Guid actual = WorkflowInvoker.Invoke(target, inputParameters);

                Assert.AreEqual(expected, actual, "ApplicationSetting returned an incorrect value");
                store.AssertWasCalled(x => x.GetApplicationSetting(key, defaultValue, IsRequired));
            }
            finally
            {
                ConfigurationStoreFactory.StoreType = null;
            }
        }

        /// <summary>
        /// Runs test for application setting throws exception with empty key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplicationSettingThrowsExceptionWithEmptyKeyTest()
        {
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            inputParameters["Key"] = String.Empty;
            inputParameters["DefaultValue"] = Guid.NewGuid();
            inputParameters["IsRequired"] = false;

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
            Activity<Guid> target = ActivityStore.Resolve<ApplicationSetting<Guid>>();

            try
            {
                ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            finally
            {
                ConfigurationStoreFactory.StoreType = null;
            }
        }

        /// <summary>
        /// Runs test for application setting throws exception with null key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplicationSettingThrowsExceptionWithNullKeyTest()
        {
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            inputParameters["Key"] = null;
            inputParameters["DefaultValue"] = Guid.NewGuid();
            inputParameters["IsRequired"] = false;

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
            Activity<Guid> target = ActivityStore.Resolve<ApplicationSetting<Guid>>();

            try
            {
                ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            finally
            {
                ConfigurationStoreFactory.StoreType = null;
            }
        }

        /// <summary>
        /// Runs test for application setting throws exception with white space key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplicationSettingThrowsExceptionWithWhiteSpaceKeyTest()
        {
            IDictionary<String, Object> inputParameters = new Dictionary<String, Object>();
            IConfigurationStore store = MockRepository.GenerateStub<IConfigurationStore>();

            inputParameters["Key"] = " ";
            inputParameters["DefaultValue"] = Guid.NewGuid();
            inputParameters["IsRequired"] = false;

            ConfigurationStoreMockWrapper.MockInstance = store;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);
            Activity<Guid> target = ActivityStore.Resolve<ApplicationSetting<Guid>>();

            try
            {
                ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

                WorkflowInvoker.Invoke(target, inputParameters);
            }
            finally
            {
                ConfigurationStoreFactory.StoreType = null;
            }
        }

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        /// <value>
        ///   The test context.
        /// </value>
        public TestContext TestContext
        {
            get;
            set;
        }
    }
}