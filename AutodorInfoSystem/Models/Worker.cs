using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class Worker
{
    public int IdWorker { get; set; }

    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string Name { get; set; } = null!;

    [Column("salary")]
    [Required(ErrorMessage = "Это поле обязательное")]
    [Range(0.01, 9999999.99, ErrorMessage = "Зарплата вышла за допустимый диапозон")]
    public double? Salary { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Это поле обязательное")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество вышло за допустимый диапозон")]
    public int? Quantity { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [Column("created_date")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public virtual ICollection<WorkersHasTask> WorkersHasTasks { get; set; } = new List<WorkersHasTask>();
}
