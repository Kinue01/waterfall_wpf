using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbUser
{
    public long UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserPass { get; set; } = null!;

    public short UserRoleId { get; set; }

    public virtual ICollection<TbClient> TbClients { get; set; } = new List<TbClient>();

    public virtual ICollection<TbEmployee> TbEmployees { get; set; } = new List<TbEmployee>();

    public virtual TbRole UserRole { get; set; } = null!;
}
