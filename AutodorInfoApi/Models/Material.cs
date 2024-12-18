using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class Material
{
    public int IdMaterial { get; set; }

    public string Name { get; set; } = null!;

    public string? MeasurementUnit { get; set; }

    public double Price { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<MaterialsHasTask> MaterialsHasTasks { get; set; } = new List<MaterialsHasTask>();
}
