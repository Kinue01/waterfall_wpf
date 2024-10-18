namespace waterfall_wpf.Utils
{
    public interface IDialogService
    {
        T OpenDialog<T>(DialogViewModelBase<T> vm);
    }
}
