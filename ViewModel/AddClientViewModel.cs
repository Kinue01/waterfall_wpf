using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using waterfall_wpf.Model;
using waterfall_wpf.Utils;

namespace waterfall_wpf.ViewModel
{
    public class AddClientViewModel : DialogViewModelBase<DialogResult>
    {
        string login, password, lastname, firstname, middlename, email;
        ObservableCollection<TbCountry> countries;
        TbCountry currCountry;
        IDbContextFactory<WaterfallDbContext> _dbContextFactory;
        INavigationService _navigationService;

        public string Login
        {
            get => login;
            set => SetProperty(ref login, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string Lastname
        {
            get => lastname;
            set => SetProperty(ref lastname, value);
        }
        public string Firstname
        {
            get => firstname; 
            set => SetProperty(ref firstname, value);
        }
        public string Email
        {
            get => email; 
            set => SetProperty(ref email, value);
        }
        public string Middlename
        {
            get => middlename; 
            set => SetProperty(ref middlename, value);
        }
        public ObservableCollection<TbCountry> Countries
        {
            get => countries; 
            set => SetProperty(ref countries, value);
        }
        public TbCountry CurrCountry
        {
            get => currCountry; 
            set => SetProperty(ref currCountry, value);
        }

        public IAsyncRelayCommand AddClientCommand { get; set; }
        public ICommand NavigateAuthCommand { get; set; }

        public AddClientViewModel(IDbContextFactory<WaterfallDbContext> dbContextFactory, INavigationService navigationService)
        {
            _dbContextFactory = dbContextFactory;
            _navigationService = navigationService;

            AddClientCommand = new AsyncRelayCommand<IDialogWindow>(AddClient);
            NavigateAuthCommand = new RelayCommand<IDialogWindow>(Cancel);

            new Action(async () =>
            {
                await GetCountries();
            })();
        }

        async Task GetCountries()
        {
            Countries = [];
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var counries = context.TbCountries.AsAsyncEnumerable();
            await foreach (var coun in counries)
            {
                Countries.Add(coun);
            }
        }
        async Task AddClient(IDialogWindow window)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var user = await context.TbUsers.AddAsync(new TbUser
            {
                UserLogin = Login,
                UserPass = Password,
                UserRoleId = 1
            });
            await context.SaveChangesAsync();
            await context.TbClients.AddAsync(new TbClient
            {
                ClientCountryId = CurrCountry.CountryId,
                ClientEmail = Email,
                ClientFirstname = Firstname,
                ClientLastname = Lastname,
                ClientMiddlename = Middlename,
                ClientUserId = user.Entity.UserId,
            });
            await context.SaveChangesAsync();
            CloseDialogWithResult(window, DialogResult.OK);
        }
        void Cancel(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResult.Cancel);
        }
    }
}
