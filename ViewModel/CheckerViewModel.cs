using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using waterfall_wpf.Model;

namespace waterfall_wpf.ViewModel
{
    public class CheckerViewModel : ObservableObject
    {
        string ticketId, lastname, firstname, result;
        IDbContextFactory<WaterfallDbContext> _dbContextFactory;

        public string TicketId
        {
            get => ticketId;
            set => SetProperty(ref ticketId, value);
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

        public IAsyncRelayCommand CheckTicketCommand { get; set; }

        public CheckerViewModel(IDbContextFactory<WaterfallDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;

            CheckTicketCommand = new AsyncRelayCommand(CheckTicket);
        }

        async Task CheckTicket()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var res = await context.TbTickets.Include(t => t.TicketClient).Include(t => t.TicketSession).Where(u => u.TicketId == int.Parse(TicketId)).ToListAsync();
            if (res != null && res[0] != null)
            {
                Lastname = res[0].TicketClient.ClientLastname;
                Firstname = res[0].TicketClient.ClientFirstname;

                if (res[0].TicketChecked)
                {
                    Result = "Проход запрещён";
                }
                else
                {
                    Result = "Проход разрешён";
                    await context.TbTickets.Where(t => t.TicketId == int.Parse(TicketId)).ExecuteUpdateAsync(s => s.SetProperty(u => u.TicketChecked, true));
                }

                if (res[0].TicketDate == DateOnly.FromDateTime(DateTime.Now) && TimeOnly.FromTimeSpan(res[0].TicketSession.SessionTime - TimeOnly.FromDateTime(DateTime.Now)) < TimeOnly.Parse("10:00"))
                {
                    Result = "Проход разрешён";
                }
                else
                {
                    Result = "Проход запрещён";
                }
            }
        }
    }
}
