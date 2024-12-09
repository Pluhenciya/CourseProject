using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class MaterialsHasTask
{
    public int IdMaterial { get; set; }

    public int IdTask { get; set; }

    public int Quantity { get; set; }

    [Column("cost")]
    public double Cost { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual Task IdTaskNavigation { get; set; } = null!;
}
