using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class EquipmentHasTask
{
    public int IdEquipment { get; set; }

    public int IdTask { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }


    public virtual Equipment IdEquipmentNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Task IdTaskNavigation { get; set; } = null!;
}
