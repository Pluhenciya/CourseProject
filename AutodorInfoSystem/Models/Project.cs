using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class Project
{
    public int IdProject { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Projecter> ProjectersIdUsers { get; set; } = new List<Projecter>();
}
