namespace waterfall_wpf.Utils
{
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(DialogViewModelBase<T> vm)
        {
            IDialogWindow window = new DialogWindow
            {
                DataContext = vm,
            };
            window.ShowDialog();
            return vm.DialogResult;
        }
    }
}
