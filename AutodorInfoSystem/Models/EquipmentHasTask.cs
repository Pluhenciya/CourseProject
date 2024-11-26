using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class EquipmentHasTask
{
    public int EquipmentIdEquipment { get; set; }

    public int TasksIdTask { get; set; }

    public int Quantity { get; set; }

    public virtual Equipment EquipmentIdEquipmentNavigation { get; set; } = null!;

    public virtual Task TasksIdTaskNavigation { get; set; } = null!;
}
