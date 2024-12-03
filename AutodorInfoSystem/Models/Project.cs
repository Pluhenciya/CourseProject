using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutodorInfoSystem.Models;

public partial class Project
{
    public int IdProject { get; set; }

    [Required(ErrorMessage = "Поле Название обязательно")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Projecter> ProjectersIdUsers { get; set; } = new List<Projecter>();
}
