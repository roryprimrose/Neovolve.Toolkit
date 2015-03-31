namespace Neovolve.Toolkit.Workflow.Design
{
    using System;
    using System.Activities.Presentation;
    using System.Activities.Presentation.Model;
    using System.Activities.Presentation.View;
    using System.Windows.Threading;

    /// <summary>
    /// The <see cref="DesignerUpdater"/>
    ///   class is used to update the designer..
    /// </summary>
    internal sealed class DesignerUpdater
    {
        /// <summary>
        ///   The new model item.
        /// </summary>
        private readonly ModelItem _newModelItem;

        /// <summary>
        ///   The model item.
        /// </summary>
        private readonly ModelItem _originalModelItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignerUpdater"/> class.
        /// </summary>
        /// <param name="originalItem">
        /// The original item.
        /// </param>
        /// <param name="newItem">
        /// The new item.
        /// </param>
        internal DesignerUpdater(ModelItem originalItem, ModelItem newItem)
        {
            _originalModelItem = originalItem;
            _newModelItem = newItem;
        }

        /// <summary>
        /// Updates the model item.
        /// </summary>
        /// <param name="originalItem">
        /// The original item.
        /// </param>
        /// <param name="updatedItem">
        /// The updated item.
        /// </param>
        public static void UpdateModelItem(ModelItem originalItem, ModelItem updatedItem)
        {
            DesignerUpdater class2 = new DesignerUpdater(originalItem, updatedItem);

            Action method = class2.UpdateDesigner;

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, method);
        }

        /// <summary>
        /// Updates the designer.
        /// </summary>
        public void UpdateDesigner()
        {
            EditingContext editingContext = _originalModelItem.GetEditingContext();
            DesignerView designerView = editingContext.Services.GetService<DesignerView>();

            if ((designerView.RootDesigner != null) && (((WorkflowViewElement)designerView.RootDesigner).ModelItem == _originalModelItem))
            {
                designerView.MakeRootDesigner(_newModelItem);
            }

            Selection.SelectOnly(editingContext, _newModelItem);
        }
    }
}