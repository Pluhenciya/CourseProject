using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual Projecter? Projecter { get; set; }
}
