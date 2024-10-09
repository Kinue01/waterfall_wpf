using System;
using System.Collections.Generic;

namespace waterfall_wpf.Model;

public partial class TbEmployee
{
    public int EmployeeId { get; set; }

    public string EmployeeLastname { get; set; } = null!;

    public string EmployeeFirstname { get; set; } = null!;

    public string? EmployeeMiddlename { get; set; }

    public long EmployeeUserId { get; set; }

    public short EmployeePositionId { get; set; }

    public virtual TbPosition EmployeePosition { get; set; } = null!;

    public virtual TbUser EmployeeUser { get; set; } = null!;
}
