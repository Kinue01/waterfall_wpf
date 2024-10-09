using CommunityToolkit.Mvvm.ComponentModel;

namespace waterfall_wpf.Utils
{
    public interface INavigationService
    {
        ObservableObject CurrentPage { get; }
        void NavigateTo<T>() where T : ObservableObject;
    }
}
