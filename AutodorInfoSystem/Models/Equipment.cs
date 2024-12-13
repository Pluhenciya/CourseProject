using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Equipment
{
    public int IdEquipment { get; set; }

    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Это поле обязательное")]
    [Range(0.01, 9999999.99, ErrorMessage = "Цена вышла за допустимый диапозон")]
    [Column("price")]
    public double? Price { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Это поле обязательное")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество вышло за допустимый диапозон")]
    public int? Quantity { get; set; }

    public virtual ICollection<EquipmentHasTask> EquipmentHasTasks { get; set; } = new List<EquipmentHasTask>();
}
