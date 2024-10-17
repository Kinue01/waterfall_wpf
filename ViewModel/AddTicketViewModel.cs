using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MessagingToolkit.QRCode.Codec;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using waterfall_wpf.Model;
using waterfall_wpf.Utils;

namespace waterfall_wpf.ViewModel
{
    public class AddTicketViewModel : DialogViewModelBase<DialogResult>
    {
        ObservableCollection<TbClient> clients;
        ObservableCollection<TbSession> sessions;
        ObservableCollection<TbTicketType> ticketTypes;
        DateOnly date;
        TbClient currClientl;
        TbSession currSession;
        TbTicketType currTicketType;
        IDbContextFactory<WaterfallDbContext> _dbContextFactory;
        QRCodeEncoder encoder;
        IDialogService dialogService;
        AddClientViewModel addClientViewModel;

        public ObservableCollection<TbClient> Clients
        {
            get => clients;
            set => SetProperty(ref clients, value);
        }
        public ObservableCollection<TbSession> Sessions
        {
            get => sessions;
            set => SetProperty(ref sessions, value);
        }
        public ObservableCollection<TbTicketType> Types
        {
            get => ticketTypes; 
            set => SetProperty(ref ticketTypes, value);
        }
        public DateOnly CurrDate
        {
            get => date; 
            set => SetProperty(ref date, value);
        }
        public TbClient CurrClient
        {
            get => currClientl;
            set => SetProperty(ref currClientl, value);
        }
        public TbSession CurrSession
        {
            get => currSession;
            set => SetProperty(ref currSession, value);
        }
        public TbTicketType CurrType
        {
            get => currTicketType;
            set => SetProperty(ref currTicketType, value);
        }

        public IAsyncRelayCommand AddTicketCommand { get; set; }
        public ICommand NavigateAddClientCommand { get; set; }
        public ICommand Cancel { get; set; }

        public AddTicketViewModel(IDbContextFactory<WaterfallDbContext> dbContextFactory, INavigationService navigationService, QRCodeEncoder qr, IDialogService dialogService, AddClientViewModel addClientViewModel)
        {
            _dbContextFactory = dbContextFactory;
            encoder = qr;
            encoder.QRCodeScale = 8;
            this.dialogService = dialogService;
            this.addClientViewModel = addClientViewModel;
            CurrDate = DateOnly.FromDateTime(Date);

            AddTicketCommand = new AsyncRelayCommand<IDialogWindow>(AddTicket);
            NavigateAddClientCommand = new RelayCommand(OpenAddClient);
            Cancel = new RelayCommand<IDialogWindow>(CancelDialog);

            WeakReferenceMessenger.Default.Register<TimeMessenger>(this, (r, m) =>
            {
                CurrSession = Sessions.Where(s => s.SessionTime == m.Value).FirstOrDefault();
            });

            new Action(async () =>
            {
                await GetClients();
                await GetSessions();
                await GetTypes();
            })();
        }

        async Task GetClients()
        {
            Clients = [];
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var clients = context.TbClients.AsAsyncEnumerable();
            await foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }
        async Task GetSessions()
        {
            Sessions = [];
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var sessions = context.TbSessions.AsAsyncEnumerable();
            await foreach (var session in sessions)
            {
                Sessions.Add(session);
            }
        }
        async Task GetTypes()
        {
            Types = [];
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var types = context.TbTicketTypes.AsAsyncEnumerable();
            await foreach (var type in types)
            {
                Types.Add(type);
            }
        }
        async Task AddTicket(IDialogWindow window)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            await context.TbTickets.AddAsync(new TbTicket
            {
                TicketClientId = CurrClient.ClientId,
                TicketDate = CurrDate,
                TicketSessionId = CurrSession.SessionId,
                TicketTypeId = CurrType.TypeId
            });
            await context.SaveChangesAsync();
            var ticket = await context.TbTickets.LastOrDefaultAsync();
            Bitmap bitmap = encoder.Encode(ticket.TicketId.ToString());

            string qrCodeFileName = "img.png";
            string pathToReport = @"C:\Users\student\myTest.pdf";
            Document doc = new();
            PdfWriter.GetInstance(doc, new FileStream(pathToReport, FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.Paragraph newId = new(ticket.TicketId.ToString(), font);
            iTextSharp.text.Paragraph newPar = new(CurrClient.ClientLastname + " " + CurrClient.ClientFirstname + " " + CurrClient.ClientMiddlename, font);
            iTextSharp.text.Paragraph newParagraph = new(Date.ToString(), font);

            bitmap.Save(qrCodeFileName, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image imageQR = iTextSharp.text.Image.GetInstance(qrCodeFileName);

            doc.Add(newId);
            doc.Add(newPar);
            doc.Add(newParagraph);
            doc.Add(imageQR);
            doc.Close();

            File.Delete(qrCodeFileName);

            PrintDialog printDialog = new();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "PDF");
            }
            CloseDialogWithResult(window, DialogResult.OK);
        }
        void OpenAddClient()
        {
            dialogService.OpenDialog(addClientViewModel, DateTime.MinValue, TimeOnly.MinValue);
        }
        void CancelDialog(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResult.Cancel);
        }
    }
}
