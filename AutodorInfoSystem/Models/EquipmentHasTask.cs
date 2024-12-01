using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class EquipmentHasTask
{
    public int IdEquipment { get; set; }

    public int IdTask { get; set; }

    public int Quantity { get; set; }

    public virtual Equipment IdEquipmentNavigation { get; set; } = null!;

    public virtual Task IdTaskNavigation { get; set; } = null!;
}
