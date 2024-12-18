using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class WorkersHasTask
{
    public int IdWorker { get; set; }

    public int IdTask { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    [JsonIgnore]
    public virtual Task IdTaskNavigation { get; set; } = null!;

    public virtual Worker IdWorkerNavigation { get; set; } = null!;
}
