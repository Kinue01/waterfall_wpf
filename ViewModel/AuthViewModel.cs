using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using waterfall_wpf.Model;
using waterfall_wpf.Utils;

namespace waterfall_wpf.ViewModel
{
    public class AuthViewModel : ObservableObject
    {
        string login, password;
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

        public IAsyncRelayCommand AuthCommand { get; set; }

        public AuthViewModel(IDbContextFactory<WaterfallDbContext> dbContextFactory, INavigationService navigationService)
        {
            _dbContextFactory = dbContextFactory;
            _navigationService = navigationService;

            AuthCommand = new AsyncRelayCommand(Auth);
        }
        
        async Task Auth()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var user = await context.TbUsers.Where(e => e.UserLogin == Login && e.UserPass == Password).FirstOrDefaultAsync();
            if (user != null)
            {
                var emp = await context.TbEmployees.Where(e => e.EmployeeUserId == user.UserId).FirstOrDefaultAsync();
                switch (emp.EmployeePositionId)
                {
                    case 1:
                        _navigationService.NavigateTo<CashierViewModel>();
                        break;
                    case 2:
                        _navigationService.NavigateTo<CheckerViewModel>();
                        break;
                    default:
                        MessageBox.Show("Нет такой должности, или пользователь - клиент");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Нет такого пользователя");
            }
        }

    }
}
