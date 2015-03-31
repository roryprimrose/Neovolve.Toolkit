namespace Neovolve.Toolkit.UnitTests.Reflection
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Neovolve.Toolkit.Reflection;
    using Neovolve.Toolkit.Storage;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="TypeResolverTests"/>
    ///   class is used to test the <see cref="TypeResolver"/> class.
    /// </summary>
    [TestClass]
    public class TypeResolverTests
    {
        #region Setup/Teardown

        /// <summary>
        /// Cleans up after running a unit test.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            ConfigurationStoreFactory.StoreType = null;
            CacheStoreFactory.StoreType = null;
        }

        #endregion

        /// <summary>
        /// Runs test for can resolve type from key returns false with incorrect source type.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeFromKeyReturnsFalseWithIncorrectSourceTypeTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(String);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Boolean actual = TypeResolver.CanResolveTypeFromKey(typeof(Stream), configurationKey);

            Assert.IsFalse(actual, "CanResolveTypeFromKey returned an incorrect value");
            cacheStore.AssertWasCalled(x => x.Add(expected.AssemblyQualifiedName, expected.TypeHandle, null));
        }

        /// <summary>
        /// Runs test for can resolve type from key returns false with invalid type name configuration.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeFromKeyReturnsFalseWithInvalidTypeNameConfigurationTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();
            const String InvalidTypeName = "System.SomeOtherStreamThatDoesNotExist, mscorlib";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(InvalidTypeName);
            cacheStore.Stub(x => x.Contains(InvalidTypeName)).Return(false);

            Boolean actual = TypeResolver.CanResolveTypeFromKey(typeof(Stream), configurationKey);

            Assert.IsFalse(actual, "CanResolveTypeFromKey returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type from key returns false with no matching configuration.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeFromKeyReturnsFalseWithNoMatchingConfigurationTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);

            Boolean actual = TypeResolver.CanResolveTypeFromKey(typeof(Stream), configurationKey);

            Assert.IsFalse(actual, "CanResolveTypeFromKey returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type from key returns true from valid configured type name.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeFromKeyReturnsTrueFromValidConfiguredTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Boolean actual = TypeResolver.CanResolveTypeFromKey(typeof(Stream), configurationKey);

            Assert.IsTrue(actual, "CanResolveTypeFromKey returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type from key returns true with cached runtime type handle.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeFromKeyReturnsTrueWithCachedRuntimeTypeHandleTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(true);
            cacheStore.Stub(x => x.GetItem(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(true);
            cacheStore.Stub(x => x.GetItem(expected.AssemblyQualifiedName)).Return(expected.TypeHandle);

            Boolean actual = TypeResolver.CanResolveTypeFromKey(typeof(Stream), configurationKey);

            Assert.IsTrue(actual, "CanResolveTypeFromKey returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type from key with empty configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanResolveTypeFromKeyWithEmptyConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.CanResolveTypeFromKey(typeof(Stream), String.Empty);
        }

        /// <summary>
        /// Runs test for can resolve type from key with null configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanResolveTypeFromKeyWithNullConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.CanResolveTypeFromKey(typeof(Stream), null);
        }

        /// <summary>
        /// Runs test for can resolve type from key with null source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanResolveTypeFromKeyWithNullSourceTypeThrowsExceptionTest()
        {
            TypeResolver.CanResolveTypeFromKey(null, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Runs test for can resolve type from key with white space configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanResolveTypeFromKeyWithWhiteSpaceConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.CanResolveTypeFromKey(typeof(Stream), "  ");
        }

        /// <summary>
        /// Runs test for can resolve type returns false with incorrect source type.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsFalseWithIncorrectSourceTypeTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(String);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsFalse(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type returns false with invalid type name configuration.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsFalseWithInvalidTypeNameConfigurationTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);
            const String InvalidTypeName = "System.SomeOtherStreamThatDoesNotExist, mscorlib";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(InvalidTypeName);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsFalse(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type returns false with no matching configuration.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsFalseWithNoMatchingConfigurationTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(null);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsFalse(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type returns true from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsTrueFromAssemblyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsTrue(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type returns true from full type name.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsTrueFromFullTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsTrue(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type returns true with cached runtime type handle from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsTrueWithCachedRuntimeTypeHandleFromAssemblyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(true);
            cacheStore.Stub(x => x.GetItem(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(true);
            cacheStore.Stub(x => x.GetItem(expected.AssemblyQualifiedName)).Return(expected.TypeHandle);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsTrue(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type returns true with partially qualified type name.
        /// </summary>
        [TestMethod]
        public void CanResolveTypeReturnsTrueWithPartiallyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Boolean actual = TypeResolver.CanResolveType(typeof(Stream));

            Assert.IsTrue(actual, "CanResolveType returned an incorrect value");
        }

        /// <summary>
        /// Runs test for can resolve type with null source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanResolveTypeWithNullSourceTypeThrowsExceptionTest()
        {
            TypeResolver.CanResolveType(null);
        }

        /// <summary>
        /// Runs test for create from key returns instance of correct type from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void CreateFromKeyReturnsInstanceOfCorrectTypeFromValidConfigurationTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(MemoryStream);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (Stream actual = TypeResolver.CreateFromKey<Stream>(configurationKey))
            {
                Assert.IsNotNull(actual, "CreateFromKey failed to return an instance");
                Assert.IsInstanceOfType(actual, expected, "CreateFromKey returned an instance of an incorrect type");
            }
        }

        /// <summary>
        /// Runs test for create from key throws exception with empty configuration key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromKeyThrowsExceptionWithEmptyConfigurationKeyTest()
        {
            TypeResolver.CreateFromKey<Object>(String.Empty);
        }

        /// <summary>
        /// Runs test for create from key throws exception with null configuration key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromKeyThrowsExceptionWithNullConfigurationKeyTest()
        {
            TypeResolver.CreateFromKey<Object>(null);
        }

        /// <summary>
        /// Runs test for create from key throws exception with white space configuration key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromKeyThrowsExceptionWithWhiteSpaceConfigurationKeyTest()
        {
            TypeResolver.CreateFromKey<Object>(" ");
        }

        /// <summary>
        /// Runs test for create from key uses cached runtime type handle.
        /// </summary>
        [TestMethod]
        public void CreateFromKeyUsesCachedRuntimeTypeHandleTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(MemoryStream);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(true);
            cacheStore.Stub(x => x.GetItem(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(true);
            cacheStore.Stub(x => x.GetItem(expected.AssemblyQualifiedName)).Return(expected.TypeHandle);

            using (Stream actual = TypeResolver.CreateFromKey<Stream>(configurationKey))
            {
                Assert.IsNotNull(actual, "CreateFromKey failed to return an instance");
                Assert.IsInstanceOfType(actual, expected, "CreateFromKey returned an instance of an incorrect type");
            }
        }

        /// <summary>
        /// Runs test for create from key with empty configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromKeyWithEmptyConfigurationKeyThrowsExceptionTest()
        {
            using (TypeResolver.CreateFromKey<Stream>(String.Empty))
            {
            }
        }

        /// <summary>
        /// Runs test for create from key with incorrect source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CreateFromKeyWithIncorrectSourceTypeThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(String);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (TypeResolver.CreateFromKey<Stream>(configurationKey))
            {
            }
        }

        /// <summary>
        /// Runs test for create from key with invalid type name configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void CreateFromKeyWithInvalidTypeNameConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();
            const String InvalidTypeName = "System.SomeOtherStreamThatDoesNotExist, mscorlib";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(InvalidTypeName);
            cacheStore.Stub(x => x.Contains(InvalidTypeName)).Return(false);

            using (TypeResolver.CreateFromKey<Stream>(configurationKey))
            {
            }
        }

        /// <summary>
        /// Runs test for create from key with no matching configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void CreateFromKeyWithNoMatchingConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);

            using (TypeResolver.CreateFromKey<Stream>(configurationKey))
            {
            }
        }

        /// <summary>
        /// Runs test for create from key with null configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromKeyWithNullConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.CreateFromKey<String>(null);
        }

        /// <summary>
        /// Runs test for create from key with type with no default constructor throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void CreateFromKeyWithTypeWithNoDefaultConstructorThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();
            Type expected = typeof(BufferedStream);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);

            using (TypeResolver.CreateFromKey<Stream>(configurationKey))
            {
            }
        }

        /// <summary>
        /// Runs test for create returns instance of correct type from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void CreateReturnsInstanceOfCorrectTypeFromAssemblyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(MemoryStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (Stream actual = TypeResolver.Create<Stream>())
            {
                Assert.IsNotNull(actual, "Create failed to return an instance");
                Assert.IsInstanceOfType(actual, expected, "Create returned an instance of an incorrect type");
            }
        }

        /// <summary>
        /// Runs test for create returns correct type from full type name.
        /// </summary>
        [TestMethod]
        public void CreateReturnsInstanceOfCorrectTypeFromFullTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(MemoryStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (Stream actual = TypeResolver.Create<Stream>())
            {
                Assert.IsNotNull(actual, "Create failed to return an instance");
                Assert.IsInstanceOfType(actual, expected, "Create returned an instance of an incorrect type");
            }
        }

        /// <summary>
        /// Runs test for create returns instance of correct type with partially qualified type name.
        /// </summary>
        [TestMethod]
        public void CreateReturnsInstanceOfCorrectTypeWithPartiallyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(MemoryStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (Stream actual = TypeResolver.Create<Stream>())
            {
                Assert.IsNotNull(actual, "Create failed to return an instance");
                Assert.IsInstanceOfType(actual, expected, "Create returned an instance of an incorrect type");
            }
        }

        /// <summary>
        /// Runs test for create uses cached runtime type handle from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void CreateUsesCachedRuntimeTypeHandleFromAssemblyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(MemoryStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(true);
            cacheStore.Stub(x => x.GetItem(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(true);
            cacheStore.Stub(x => x.GetItem(expected.AssemblyQualifiedName)).Return(expected.TypeHandle);

            using (Stream actual = TypeResolver.Create<Stream>())
            {
                Assert.IsNotNull(actual, "Create failed to return an instance");
                Assert.IsInstanceOfType(actual, expected, "Create returned an instance of an incorrect type");
            }
        }

        /// <summary>
        /// Runs test for create with incorrect source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void CreateWithIncorrectSourceTypeThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(String);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (TypeResolver.Create<Stream>())
            {
            }
        }

        /// <summary>
        /// Runs test for create with invalid type name configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void CreateWithInvalidTypeNameConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);
            const String InvalidTypeName = "System.SomeOtherStreamThatDoesNotExist, mscorlib";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(InvalidTypeName);
            cacheStore.Stub(x => x.Contains(InvalidTypeName)).Return(false);

            using (TypeResolver.Create<Stream>())
            {
            }
        }

        /// <summary>
        /// Runs test for create with no matching configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void CreateWithNoMatchingConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(null);

            using (TypeResolver.Create<Stream>())
            {
            }
        }

        /// <summary>
        /// Runs test for create with type with no default constructor throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void CreateWithTypeWithNoDefaultConstructorThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            using (TypeResolver.Create<Stream>())
            {
            }
        }

        /// <summary>
        /// Runs test for resolve caches resolved type and configuration mapping.
        /// </summary>
        [TestMethod]
        public void ResolveCachesResolvedTypeAndConfigurationMappingTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(expected.AssemblyQualifiedName);

            TypeResolver.Resolve(typeof(Stream));

            // Test that the type handle is cached
            cacheStore.AssertWasCalled(x => x.Add(expected.AssemblyQualifiedName, expected.TypeHandle, null));

            // Test that the mapping of the type names is cached
            cacheStore.AssertWasCalled(x => x.Add(configurationKey, expected.AssemblyQualifiedName));
        }

        /// <summary>
        /// Runs test for resolve from key returns cached runtime type handle.
        /// </summary>
        [TestMethod]
        public void ResolveFromKeyReturnsCachedRuntimeTypeHandleTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();
            Type expected = typeof(BufferedStream);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(true);
            cacheStore.Stub(x => x.GetItem(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(true);
            cacheStore.Stub(x => x.GetItem(expected.AssemblyQualifiedName)).Return(expected.TypeHandle);

            Type actual = TypeResolver.ResolveFromKey(typeof(Stream), configurationKey);

            Assert.AreEqual(expected, actual, "ResolveFromKey returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve from key returns correct type from valid configured type name.
        /// </summary>
        [TestMethod]
        public void ResolveFromKeyReturnsCorrectTypeFromValidConfiguredTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();
            Type expected = typeof(BufferedStream);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Type actual = TypeResolver.ResolveFromKey(typeof(Stream), configurationKey);

            Assert.AreEqual(expected, actual, "ResolveFromKey returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve from key with empty configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveFromKeyWithEmptyConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.ResolveFromKey(typeof(Stream), String.Empty);
        }

        /// <summary>
        /// Runs test for resolve from key with incorrect source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ResolveFromKeyWithIncorrectSourceTypeThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(String);
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            TypeResolver.ResolveFromKey(typeof(Stream), configurationKey);

            Assert.Fail("InvalidCastException was expected");
        }

        /// <summary>
        /// Runs test for resolve from key with invalid type name configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void ResolveFromKeyWithInvalidTypeNameConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();
            const String InvalidTypeName = "System.SomeOtherStreamThatDoesNotExist, mscorlib";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(InvalidTypeName);
            cacheStore.Stub(x => x.Contains(InvalidTypeName)).Return(false);

            TypeResolver.ResolveFromKey(typeof(Stream), configurationKey);

            Assert.Fail("TypeLoadException was expected");
        }

        /// <summary>
        /// Runs test for resolve from key with no matching configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void ResolveFromKeyWithNoMatchingConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = Guid.NewGuid().ToString();

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);

            TypeResolver.ResolveFromKey(typeof(Stream), configurationKey);
        }

        /// <summary>
        /// Runs test for resolve from key with null configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveFromKeyWithNullConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.ResolveFromKey(typeof(Stream), null);
        }

        /// <summary>
        /// Runs test for resolve from key with null source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveFromKeyWithNullSourceTypeThrowsExceptionTest()
        {
            TypeResolver.ResolveFromKey(null, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Runs test for resolve from key with white space configuration key throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveFromKeyWithWhiteSpaceConfigurationKeyThrowsExceptionTest()
        {
            TypeResolver.ResolveFromKey(typeof(Stream), " ");
        }

        /// <summary>
        /// Runs test for resolve returns cached runtime type handle from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsCachedRuntimeTypeHandleFromAssemblyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(true);
            cacheStore.Stub(x => x.GetItem(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(true);
            cacheStore.Stub(x => x.GetItem(expected.AssemblyQualifiedName)).Return(expected.TypeHandle);

            Type actual = TypeResolver.Resolve(typeof(Stream));

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve returns correct type from assembly qualified type name.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsCorrectTypeFromAssemblyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Type actual = TypeResolver.Resolve(typeof(Stream));

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve returns correct type from full type name.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsCorrectTypeFromFullTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(expected.AssemblyQualifiedName);

            Type actual = TypeResolver.Resolve(typeof(Stream));

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve returns correct type with partially qualified type name.
        /// </summary>
        [TestMethod]
        public void ResolveReturnsCorrectTypeWithPartiallyQualifiedTypeNameTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(BufferedStream);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            Type actual = TypeResolver.Resolve(typeof(Stream));

            Assert.AreEqual(expected, actual, "Resolve returned an incorrect value");
        }

        /// <summary>
        /// Runs test for resolve throws exception with null type.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveThrowsExceptionWithNullTypeTest()
        {
            TypeResolver.Resolve(null);
        }

        /// <summary>
        /// Runs test for resolve with incorrect source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ResolveWithIncorrectSourceTypeThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type expected = typeof(String);
            String configurationKey = typeof(Stream).AssemblyQualifiedName;

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(expected.AssemblyQualifiedName);
            cacheStore.Stub(x => x.Contains(expected.AssemblyQualifiedName)).Return(false);

            TypeResolver.Resolve(typeof(Stream));
        }

        /// <summary>
        /// Runs test for resolve with invalid type name configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void ResolveWithInvalidTypeNameConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            String configurationKey = typeof(Stream).AssemblyQualifiedName;
            String partiallyQualifiedName = typeof(Stream).FullName + ", " + Path.GetFileNameWithoutExtension(typeof(Stream).Assembly.Location);
            const String InvalidTypeName = "System.SomeOtherStreamThatDoesNotExist, mscorlib";

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(typeof(Stream).FullName)).Return(InvalidTypeName);
            cacheStore.Stub(x => x.Contains(InvalidTypeName)).Return(false);

            TypeResolver.Resolve(typeof(Stream));

            Assert.Fail("TypeLoadException was expected");
        }

        /// <summary>
        /// Runs test for resolve with no matching configuration throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TypeLoadException))]
        public void ResolveWithNoMatchingConfigurationThrowsExceptionTest()
        {
            ICacheStore cacheStore = MockRepository.GenerateMock<ICacheStore>();
            IConfigurationStore configStore = MockRepository.GenerateStub<IConfigurationStore>();
            Type sourceType = typeof(Stream);
            String configurationKey = sourceType.AssemblyQualifiedName;
            String partiallyQualifiedName = sourceType.FullName + ", " + Path.GetFileNameWithoutExtension(sourceType.Assembly.Location);

            CacheStoreMockWrapper.MockInstance = cacheStore;
            CacheStoreFactory.StoreType = typeof(CacheStoreMockWrapper);
            ConfigurationStoreMockWrapper.MockInstance = configStore;
            ConfigurationStoreFactory.StoreType = typeof(ConfigurationStoreMockWrapper);

            cacheStore.Stub(x => x.Contains(configurationKey)).Return(false);
            configStore.Stub(x => x.GetApplicationSetting<String>(configurationKey)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(partiallyQualifiedName)).Return(null);
            configStore.Stub(x => x.GetApplicationSetting<String>(sourceType.FullName)).Return(null);

            TypeResolver.Resolve(sourceType);
        }

        /// <summary>
        /// Runs test for resolve with null source type throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveWithNullSourceTypeThrowsExceptionTest()
        {
            TypeResolver.Resolve(null);
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