using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Task
{
    public int IdTask { get; set; }

    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(200, ErrorMessage = "Лимит символов превышен (максимум 200 символов)")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    [NotMapped]
    public string? ShortDescription => Description?.Length > 100 ? $"{Description?.Substring(0, 200)}..." : Description;

    public int IdProject { get; set; }

    [Column("cost")]
    [DefaultValue(0)]
    public double Cost { get; set; }

    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();

    public virtual ICollection<MaterialsHasTask> MaterialsHasTasks { get; set; } = new List<MaterialsHasTask>();

    public virtual Project IdProjectNavigation { get; set; } = null!;

    public virtual ICollection<WorkersHasTask> WorkersHasTasks { get; set; } = new List<WorkersHasTask>();
}
