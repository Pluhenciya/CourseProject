using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    [NotMapped]
    public string? Role { get; set; } = null!;

    [NotMapped]
    public string? Surname { get; set; } = null!;

    [NotMapped]
    public string? Name { get; set; } = null!;

    [NotMapped]
    public string? Patronymic { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual Projecter? Projecter { get; set; }
}
