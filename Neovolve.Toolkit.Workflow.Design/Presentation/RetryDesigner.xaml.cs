namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="RetryDesigner"/>
    ///   class is used to provide desig time support for the <see cref="Retry"/> class.
    /// </summary>
    public partial class RetryDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "RetryDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public RetryDesigner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Exceptions the type filter.
        /// </summary>
        /// <param name="typeToValidate">
        /// The type to validate.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        public Boolean ExceptionTypeFilter(Type typeToValidate)
        {
            if (typeToValidate == null)
            {
                return false;
            }

            if (typeof(Exception).IsAssignableFrom(typeToValidate))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Registers the metadata.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        internal static void RegisterMetadata(AttributeTableBuilder builder)
        {
            builder.AddCustomAttributes(typeof(Retry), new DesignerAttribute(typeof(RetryDesigner)));
        }
    }
}