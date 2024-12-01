using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Equipment
{
    public int IdEquipment { get; set; }

    public string Name { get; set; } = null!;

    [Column("price")]
    public double Price { get; set; }

    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();
}
