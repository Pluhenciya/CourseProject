using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class MaterialsHasTask
{
    public int IdMaterial { get; set; }

    public int IdTask { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Task IdTaskNavigation { get; set; } = null!;
}
