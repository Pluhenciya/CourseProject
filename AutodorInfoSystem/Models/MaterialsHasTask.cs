using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class MaterialsHasTask
{
    public int MaterialsIdMaterial { get; set; }

    public int TasksIdTask { get; set; }

    public int Quantity { get; set; }

    public virtual Material MaterialsIdMaterialNavigation { get; set; } = null!;

    public virtual Task TasksIdTaskNavigation { get; set; } = null!;
}
