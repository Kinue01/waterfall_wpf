using CommunityToolkit.Mvvm.ComponentModel;
using waterfall_wpf.Utils;

namespace waterfall_wpf.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        INavigationService navigationService;

        public INavigationService NavigationService
        {
            get => navigationService;
            set => SetProperty(ref navigationService, value);
        }

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.NavigateTo<AuthViewModel>();
        }
    }
}
