using System;
using System.Collections.Generic;

namespace AutodorInfoApi.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public decimal Cost { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Projecter> ProjectersIdUsers { get; set; } = new List<Projecter>();
}
