using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbClient
{
    public int ClientId { get; set; }

    public string ClientFirstname { get; set; } = null!;

    public string ClientLastname { get; set; } = null!;

    public string? ClientMiddlename { get; set; }

    public string ClientEmail { get; set; } = null!;

    public long ClientUserId { get; set; }

    public short ClientCountryId { get; set; }

    public virtual TbCountry ClientCountry { get; set; } = null!;

    public virtual TbUser ClientUser { get; set; } = null!;

    public virtual ICollection<TbTicket> TbTickets { get; set; } = new List<TbTicket>();
}
