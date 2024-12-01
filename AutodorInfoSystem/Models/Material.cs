using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Material
{
    public int IdMaterial { get; set; }

    public string Name { get; set; } = null!;

    public string? MeasurementUnit { get; set; }

    [Column("price")]
    public double Price { get; set; }

    public virtual ICollection<MaterialsHasTask> MaterialsHasTasks { get; set; } = new List<MaterialsHasTask>();
}
