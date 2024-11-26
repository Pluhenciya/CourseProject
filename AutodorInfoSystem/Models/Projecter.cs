using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class Projecter
{
    public int IdUser { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Project> ProjectsIdProjects { get; set; } = new List<Project>();
}
