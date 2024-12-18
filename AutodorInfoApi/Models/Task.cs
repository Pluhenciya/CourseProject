using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class Task
{
    public int IdTask { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int IdProject { get; set; }

    public decimal Cost { get; set; }

    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();

    [JsonIgnore]
    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual ICollection<MaterialsHasTask> MaterialsHasTasks { get; set; } = new List<MaterialsHasTask>();

    public virtual ICollection<WorkersHasTask> WorkersHasTasks { get; set; } = new List<WorkersHasTask>();
}
