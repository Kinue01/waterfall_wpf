namespace waterfall_wpf.Utils
{
    public class DialogService : IDialogService
    {
        public T OpenDialog<T>(DialogViewModelBase<T> vm, DateTime date, TimeOnly time)
        {
            vm.Date = date;
            vm.Time = time;
            IDialogWindow window = new DialogWindow
            {
                DataContext = vm,
            };
            window.ShowDialog();
            return vm.DialogResult;
        }
    }
}
