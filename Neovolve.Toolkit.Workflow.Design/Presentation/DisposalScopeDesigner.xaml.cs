namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Metadata;
    using System.ComponentModel;
    using System.Diagnostics;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="DisposalScopeDesigner"/>
    ///   class is used to provide desig time support for the <see cref="DisposalScope{T}"/> class.
    /// </summary>
    public partial class DisposalScopeDesigner
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "DisposalScopeDesigner" /> class.
        /// </summary>
        [DebuggerNonUserCode]
        public DisposalScopeDesigner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The type filter to allow only <see cref="IDisposable"/> types.
        /// </summary>
        /// <param name="typeToValidate">
        /// The type to validate.
        /// </param>
        /// <returns>
        /// A <see cref="Boolean"/> instance.
        /// </returns>
        public Boolean DisposalTypeFilter(Type typeToValidate)
        {
            if (typeToValidate == null)
            {
                return false;
            }

            if (typeof(IDisposable).IsAssignableFrom(typeToValidate))
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
            builder.AddCustomAttributes(typeof(DisposalScope<>), new DesignerAttribute(typeof(DisposalScopeDesigner)));
            builder.AddCustomAttributes(typeof(DisposalScope<>), new DefaultTypeArgumentAttribute(typeof(IDisposable)));
        }

        /// <summary>
        /// Called when the model item has been changed.
        /// </summary>
        /// <param name="newItem">
        /// The new item.
        /// </param>
        protected override void OnModelItemChanged(Object newItem)
        {
            base.OnModelItemChanged(newItem);

            GenericArgumentTypeUpdater.Attach(ModelItem);
        }
    }
}