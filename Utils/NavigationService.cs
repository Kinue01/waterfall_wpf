using CommunityToolkit.Mvvm.ComponentModel;

namespace waterfall_wpf.Utils
{
    public class NavigationService : ObservableObject, INavigationService
    {
        ObservableObject currentPage;
        readonly Func<Type, ObservableObject> _pageFactory;

        public ObservableObject CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        public NavigationService(Func<Type, ObservableObject> pageFactory) 
        {
            _pageFactory = pageFactory;
        }

        public void NavigateTo<T>() where T : ObservableObject
        {
            ObservableObject page = _pageFactory.Invoke(typeof(T));
            CurrentPage = page;
        }
    }
}
