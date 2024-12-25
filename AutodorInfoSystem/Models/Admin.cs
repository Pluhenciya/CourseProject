using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutodorInfoSystem.Models;

public partial class Admin
{
    public int UsersIdUser { get; set; }

    [JsonIgnore]
    public virtual User UsersIdUserNavigation { get; set; } = null!;
}
