using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using waterfall_wpf.Model;
using waterfall_wpf.Utils;

namespace waterfall_wpf.ViewModel
{
    public class CashierViewModel : ObservableObject
    {
        ObservableCollection<SessionTable> _sessions;
        INavigationService _navigationService;
        IDbContextFactory<WaterfallDbContext> _dbContextFactory;
        DateTime startDate, endDate, currDate;
        IDialogService dialogService;
        AddTicketViewModel addTicketViewModel;
        SessionTable currSession;

        public ObservableCollection<SessionTable> Sessions
        {
            get => _sessions;
            set => SetProperty(ref _sessions, value);
        }
        public DateTime StartDate
        {
            get => startDate;
            set => SetProperty(ref startDate, value);
        }
        public DateTime EndDate
        {
            get => endDate;
            set => SetProperty(ref endDate, value);
        }
        public DateTime CurrDate
        {
            get => currDate;
            set => SetProperty(ref currDate, value);
        }
        public SessionTable CurrSession
        {
            get => currSession;
            set => SetProperty(ref currSession, value);
        }

        public IAsyncRelayCommand NavigateAddTicketCommand { get; set; }
        public IAsyncRelayCommand GetSessionsCommand { get; set; }

        public CashierViewModel(INavigationService navigationService, IDbContextFactory<WaterfallDbContext> dbContextFactory, IDialogService dialogService, AddTicketViewModel addTicketViewModel)
        {
            _navigationService = navigationService;
            _dbContextFactory = dbContextFactory;
            this.dialogService = dialogService;
            this.addTicketViewModel = addTicketViewModel;

            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(1);
            CurrDate = DateTime.Now;

            NavigateAddTicketCommand = new AsyncRelayCommand(OpenAddTicket);
            GetSessionsCommand = new AsyncRelayCommand(GetSessions);

            new Action(async () =>
            {
                await GetSessions();
            })();
        }

        async Task GetSessions()
        {
            Sessions = [];
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var query = from session in context.TbSessions
                        select new
                        {
                            SessionTime = session.SessionTime,
                            Count = context.TbTickets.Where(t => t.TicketSessionId == session.SessionId).Where(t => t.TicketDate == DateOnly.FromDateTime(CurrDate)).Count(),
                        };

            var result = query.AsAsyncEnumerable();
            await foreach (var item in result)
            {
                Sessions.Add(new SessionTable
                {
                    SessionTime = item.SessionTime,
                    TicketCount = 15 - item.Count
                });
            }
        }
        async Task OpenAddTicket()
        {
            WeakReferenceMessenger.Default.Send(new TimeMessenger(CurrSession.SessionTime));
            WeakReferenceMessenger.Default.Send(new DateMessenger(DateOnly.FromDateTime(CurrDate)));
            if(dialogService.OpenDialog(addTicketViewModel) == DialogResult.OK)
            {
                await GetSessions();
            }
        }
    }
}
