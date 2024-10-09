using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbSession
{
    public int SessionId { get; set; }

    public TimeOnly SessionTime { get; set; }

    public virtual ICollection<TbTicket> TbTickets { get; set; } = new List<TbTicket>();
}
