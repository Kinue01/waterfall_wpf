namespace waterfall_wpf.Model;

public partial class TbTicket
{
    public int TicketId { get; set; }

    public DateOnly TicketDate { get; set; }

    public bool TicketChecked { get; set; }

    public int TicketClientId { get; set; }

    public int TicketSessionId { get; set; }

    public short TicketTypeId { get; set; }

    public virtual TbClient TicketClient { get; set; } = null!;

    public virtual TbSession TicketSession { get; set; } = null!;

    public virtual TbTicketType TicketType { get; set; } = null!;
}
