using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Material
{
    public int IdMaterial { get; set; }

    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string Name { get; set; } = null!;

    [StringLength(5, ErrorMessage = "Лимит символов превышен (максимум 5 символов)")]
    public string? MeasurementUnit { get; set; }

    [Required(ErrorMessage = "Это поле обязательное")]
    [Range(0.01, 9999999.99, ErrorMessage = "Цена вышла за допустимый диапозон")]
    [Column("price")]
    public double? Price { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Это поле обязательное")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество вышло за допустимый диапозон")]
    public int? Quantity { get; set; }

    public virtual ICollection<MaterialsHasTask> MaterialsHasTasks { get; set; } = new List<MaterialsHasTask>();
}
