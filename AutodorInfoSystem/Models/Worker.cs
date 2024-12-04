using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Worker
{
    public int IdWorker { get; set; }

    public string Name { get; set; } = null!;

    [Column("salary")]
    public double Salary { get; set; }

    [NotMapped]
    public int Quantity { get; set; }

    public virtual ICollection<WorkersHasTask> WorkersHasTasks { get; set; } = new List<WorkersHasTask>();
}
