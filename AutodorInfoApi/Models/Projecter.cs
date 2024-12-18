using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutodorInfoApi.Models;

public partial class Projecter
{
    public int IdUser { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    [JsonIgnore]
    public virtual User IdUserNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Project> ProjectsIdProjects { get; set; } = new List<Project>();
}
