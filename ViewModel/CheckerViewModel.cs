using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using waterfall_wpf.Model;

namespace waterfall_wpf.ViewModel
{
    public class CheckerViewModel : ObservableObject
    {
        string ticket, lastname, firstname, result;
        DateTime date;
        TimeOnly time;
        IDbContextFactory<WaterfallDbContext> _dbContextFactory;

        public string Ticket
        {
            get => ticket;
            set => SetProperty(ref ticket, value);
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
        public string Result
        {
            get => result;
            set => SetProperty(ref result, value);
        }
        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
        public TimeOnly Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        public IAsyncRelayCommand CheckTicketCommand { get; set; }

        public CheckerViewModel(IDbContextFactory<WaterfallDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;

            CheckTicketCommand = new AsyncRelayCommand(CheckTicket);
        }

        async Task CheckTicket()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var res = await context.TbTickets.Include(t => t.TicketClient).Include(t => t.TicketSession).Where(u => u.TicketId == int.Parse(Ticket)).ToListAsync();
            if (res != null && res[0] != null)
            {
                Lastname = res[0].TicketClient.ClientLastname;
                Firstname = res[0].TicketClient.ClientFirstname;
                Date = res[0].TicketDate.ToDateTime(TimeOnly.MinValue);
                Time = res[0].TicketSession.SessionTime;

                if (res[0].TicketDate == DateOnly.FromDateTime(DateTime.Now) && TimeOnly.FromTimeSpan(res[0].TicketSession.SessionTime - TimeOnly.FromDateTime(DateTime.Now)) < TimeOnly.Parse("10:00") && !res[0].TicketChecked)
                {
                    Result = "Проход разрешён";
                    await context.TbTickets.Where(t => t.TicketId == int.Parse(Ticket)).ExecuteUpdateAsync(s => s.SetProperty(u => u.TicketChecked, true));
                }
                else
                {
                    Result = "Проход запрещён";
                }
            }
        }
    }
}
