using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class WorkersHasTask
{
    public int IdWorker { get; set; }

    public int IdTask { get; set; }

    public int Quantity { get; set; }

    public virtual Task IdTaskNavigation { get; set; } = null!;

    public virtual Worker IdWorkerNavigation { get; set; } = null!;
}
