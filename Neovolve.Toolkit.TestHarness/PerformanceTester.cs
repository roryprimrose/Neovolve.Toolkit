namespace Neovolve.Toolkit.TestHarness
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// The <see cref="PerformanceTester"/>
    ///   class provides a UI for running performance actions.
    /// </summary>
    public partial class PerformanceTester : Form
    {
        /// <summary>
        /// Defines the delegate for invoking the <see cref="PerformanceTester.NotifyIterationCompleted"/> event.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private delegate void IterationCompleted(Object sender, IterationEventArgs e);

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PerformanceTester" /> class.
        /// </summary>
        public PerformanceTester()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the CollectGC control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private static void CollectGC_Click(Object sender, EventArgs e)
        {
            GC.Collect();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the Actions control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Actions_SelectedIndexChanged(Object sender, EventArgs e)
        {
            IRepeatableAction action = Actions.SelectedValue as IRepeatableAction;

            if (action == null)
            {
                return;
            }

            RecursionDepth.Enabled = action.IsRecursive;
        }

        /// <summary>
        /// Finds the actions.
        /// </summary>
        /// <returns>
        /// A <see cref="List{T}"/> instance.
        /// </returns>
        private List<IRepeatableAction> FindActions()
        {
            List<IRepeatableAction> actions = new List<IRepeatableAction>();
            Type[] types = GetType().Assembly.GetTypes();

            for (Int32 index = 0; index < types.Length; index++)
            {
                Type type = types[index];

                if (type.IsInterface)
                {
                    continue;
                }

                if (type.IsAbstract)
                {
                    continue;
                }

                if (typeof(IRepeatableAction).IsAssignableFrom(type))
                {
                    IRepeatableAction action = Activator.CreateInstance(type) as IRepeatableAction;

                    actions.Add(action);
                }
            }

            return actions;
        }

        /// <summary>
        /// Iterations the completed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="Neovolve.Toolkit.TestHarness.IterationEventArgs"/> instance containing the event data.
        /// </param>
        private void NotifyIterationCompleted(Object sender, IterationEventArgs e)
        {
            if (Progress.InvokeRequired)
            {
                Object[] eventArguments = new Object[]
                                          {
                                              e
                                          };

                Progress.Invoke(new IterationCompleted(NotifyIterationCompleted), eventArguments);

                return;
            }

            Int32 percentage = 0;

            if (e.Total > 0)
            {
                percentage = e.Current * 100 / e.Total;
            }

            Progress.Value = percentage;
        }

        /// <summary>
        /// Handles the Load event of the PerformanceTester control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void PerformanceTester_Load(Object sender, EventArgs e)
        {
            List<IRepeatableAction> actions = FindActions();

            Actions.DataSource = actions;
        }

        /// <summary>
        /// Handles the Click event of the Start control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Start_Click(Object sender, EventArgs e)
        {
            Start.Enabled = false;

            IRepeatableAction action = Actions.SelectedValue as IRepeatableAction;

            if (action == null)
            {
                return;
            }

            try
            {
                action.IterationCompleted += NotifyIterationCompleted;

                if (action.IsRecursive)
                {
                    action.InvokeRecursive(Convert.ToInt32(Iterations.Value), Convert.ToInt32(RecursionDepth.Value));
                }
                else
                {
                    action.Invoke(Convert.ToInt32(Iterations.Value));
                }
            }
            finally
            {
                action.IterationCompleted -= NotifyIterationCompleted;

                Progress.Value = 0;
                Start.Enabled = true;
            }
        }
    }
}