namespace Neovolve.Toolkit.Unity.UnitTests
{
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;

    /// <summary>
    /// The <see cref="SectionExtensionInitiatorTests"/>
    ///   class is used to test the <see cref="SectionExtensionInitiator"/> class.
    /// </summary>
    [TestClass]
    public class SectionExtensionInitiatorTests
    {
        /// <summary>
        /// Runs test for section extension initiator add extension has no affect with null context test.
        /// </summary>
        [TestMethod]
        public void SectionExtensionInitiatorAddExtensionHasNoAffectWithNullContextTest()
        {
            SectionExtensionInitiator target = new SectionExtensionInitiator();

            target.AddExtensions(null);
        }

        /// <summary>
        /// Runs test for section extension initiator add extensions to context.
        /// </summary>
        [TestMethod]
        public void SectionExtensionInitiatorAddExtensionToContextTest()
        {
            SectionExtensionContext context = MockRepository.GenerateStub<SectionExtensionContext>();

            SectionExtensionInitiator target = new SectionExtensionInitiator();

            target.AddExtensions(context);

            context.AssertWasCalled(x => x.AddElement<AppSettingsParameterValueElement>(AppSettingsParameterValueElement.ElementName));
            context.AssertWasCalled(x => x.AddElement<ConnectionStringParameterValueElement>(ConnectionStringParameterValueElement.ElementName));
            context.AssertWasCalled(x => x.AddElement<ProxyParameterValueElement>(ProxyParameterValueElement.ElementName));
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