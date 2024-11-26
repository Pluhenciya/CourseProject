using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class WorkersHasTask
{
    public int WorkersIdWorker { get; set; }

    public int TasksIdTask { get; set; }

    public int Quantity { get; set; }

    public virtual Task TasksIdTaskNavigation { get; set; } = null!;

    public virtual Worker WorkersIdWorkerNavigation { get; set; } = null!;
}
