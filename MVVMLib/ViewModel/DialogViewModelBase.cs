namespace MVVMLib.ViewModel
{
    /// <summary>
    /// Base class for view models that have to be shown as a modal dialog.
    /// </summary>
    /// <remarks>
    /// Add the following property to the view, to allow the view to close itself.
    ///     dialog:DialogCloser.DialogResult="{Binding DialogResult}"
    /// </remarks>
    /// <author>Daniel Lemke - lemked@web.de</author>
    public abstract class DialogViewModelBase : ViewModelBase
    {
        private bool? dialogResult;

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }

            set
            {
                dialogResult = value;
                OnPropertyChanged();
            }
        }
    }
}
