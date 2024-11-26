using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class Equipment
{
    public int IdEquipment { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();
}
