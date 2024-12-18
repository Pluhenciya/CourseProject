using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class Equipment
{
    public int IdEquipment { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();
}
