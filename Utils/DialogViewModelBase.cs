using CommunityToolkit.Mvvm.ComponentModel;

namespace waterfall_wpf.Utils
{
    public class DialogViewModelBase<T> : ObservableObject
    {
        public DateTime Date { get; set; }
        public TimeOnly Time { get; set; }
        public T DialogResult { get; set; }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
            {
                dialog.DialogResult = true;
            }
        }
    }
}
