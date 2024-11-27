using System;
using System.Collections.Generic;

namespace AutodorInfoSystem.Models;

public partial class Admin
{
    public int UsersIdUser { get; set; }

    public virtual User UsersIdUserNavigation { get; set; } = null!;
}
