using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbCountry
{
    public short CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<TbClient> TbClients { get; set; } = new List<TbClient>();
}
