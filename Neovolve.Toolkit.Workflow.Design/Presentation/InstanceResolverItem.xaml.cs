namespace Neovolve.Toolkit.Workflow.Design.Presentation
{
    using System;
    using System.Activities.Presentation.Model;
    using System.Diagnostics;
    using System.Windows;
    using Neovolve.Toolkit.Workflow.Activities;

    /// <summary>
    /// The <see cref="InstanceResolverItem"/>
    ///   class displays the item being resolved by the 
    ///   <see cref="InstanceResolver{T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16}"/>
    ///   class.
    /// </summary>
    public partial class InstanceResolverItem
    {
        /// <summary>
        ///   Defines the dependency property for the <see cref = "InstanceName" /> property.
        /// </summary>
        public static readonly DependencyProperty InstanceNameProperty = DependencyProperty.Register(
            "InstanceName", typeof(String), typeof(InstanceResolverItem));

        /// <summary>
        ///   Defines the dependency property for the <see cref = "InstanceType" /> property.
        /// </summary>
        public static readonly DependencyProperty InstanceTypeProperty = DependencyProperty.Register(
            "InstanceType", typeof(Type), typeof(InstanceResolverItem));

        /// <summary>
        ///   Defines the dependency property for the <see cref = "ResolutionName" /> property.
        /// </summary>
        public static readonly DependencyProperty ResolutionNameProperty = DependencyProperty.Register(
            "ResolutionName", typeof(ModelItem), typeof(InstanceResolverItem));

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InstanceResolverItem" /> class.
        /// </summary>
        public InstanceResolverItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement"/> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">
        /// The event data that describes the property that changed, as well as old and new values.
        /// </param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            String old = "<null>";
            String newValue = "<null>";

            if (e.OldValue != null)
            {
                old = e.OldValue + " <" + e.OldValue.GetType().Name + ">";
            }

            if (e.NewValue != null)
            {
                newValue = e.NewValue + " <" + e.NewValue.GetType().Name + ">";
            }

            Trace.WriteLine("Property " + e.Property.Name + " changed from " + old + " to " + newValue);
        }

        /// <summary>
        ///   Gets or sets the InstanceName.
        /// </summary>
        /// <value>
        ///   The InstanceName.
        /// </value>
        public String InstanceName
        {
            get
            {
                return (String)GetValue(InstanceNameProperty);
            }

            set
            {
                SetValue(InstanceNameProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the InstanceType.
        /// </summary>
        /// <value>
        ///   The InstanceType.
        /// </value>
        public Type InstanceType
        {
            get
            {
                return (Type)GetValue(InstanceTypeProperty);
            }

            set
            {
                SetValue(InstanceTypeProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the ResolutionName.
        /// </summary>
        /// <value>
        ///   The ResolutionName.
        /// </value>
        public ModelItem ResolutionName
        {
            get
            {
                return (ModelItem)GetValue(ResolutionNameProperty);
            }

            set
            {
                SetValue(ResolutionNameProperty, value);
            }
        }
    }
}