using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbTicketType
{
    public short TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public int TypeCost { get; set; }

    public virtual ICollection<TbTicket> TbTickets { get; set; } = new List<TbTicket>();
}
