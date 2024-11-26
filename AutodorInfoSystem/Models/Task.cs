using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class Task
{
    public int IdTask { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int ProjectsIdProject { get; set; }

    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();

    public virtual ICollection<MaterialsHasTask> MaterialsHasTasks { get; set; } = new List<MaterialsHasTask>();

    public virtual Project ProjectsIdProjectNavigation { get; set; } = null!;

    public virtual ICollection<WorkersHasTask> WorkersHasTasks { get; set; } = new List<WorkersHasTask>();
}
