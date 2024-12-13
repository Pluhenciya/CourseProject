using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Project
{
    public int IdProject { get; set; }

    [Required(ErrorMessage = "Поле Название обязательно")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    [NotMapped]
    public string? ShortDescription => Description?.Length > 100 ? $"{Description?.Substring(0, 100)}..." : Description; 

    public bool IsCompleted { get; set; }

    [Column("cost")]
    public double Cost { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual ICollection<Projecter> ProjectersIdUsers { get; set; } = new List<Projecter>();
}
