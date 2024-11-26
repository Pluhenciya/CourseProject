using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class Worker
{
    public int IdWorker { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<WorkersHasTask> WorkersHasTasks { get; set; } = new List<WorkersHasTask>();
}
