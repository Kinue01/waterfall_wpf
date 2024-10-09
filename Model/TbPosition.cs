using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbPosition
{
    public short PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<TbEmployee> TbEmployees { get; set; } = new List<TbEmployee>();
}
