using CommunityToolkit.Mvvm.ComponentModel;
using MessagingToolkit.QRCode.Codec;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using waterfall_wpf.Model;
using waterfall_wpf.Utils;
using waterfall_wpf.ViewModel;

namespace waterfall_wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly ServiceProvider serviceProvider;
        public App()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ServiceCollection services = new();

            services.AddSingleton(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<CashierViewModel>();
            services.AddSingleton<AuthViewModel>();
            services.AddSingleton<CheckerViewModel>();
            services.AddSingleton<AddTicketViewModel>();
            services.AddSingleton<AddClientViewModel>();

            services.AddSingleton<Func<Type, ObservableObject>>(serviceProvider => page => (ObservableObject)serviceProvider.GetRequiredService(page));
            services.AddSingleton<QRCodeEncoder>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();

            //services.AddPooledDbContextFactory<WaterfallDbContext>(options => options.UseNpgsql("Host=172.20.105.123;Port=5432;Database=waterfall_db;Username=9po12-21-16;Password=ki9boh3Y"));
            services.AddPooledDbContextFactory<WaterfallDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=waterfall_db;Username=postgres;Password=1"));

            serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }
    }
}
