using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutodorInfoSystem.Models;

public partial class User
{
    public int IdUser { get; set; }

    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(255, ErrorMessage = "Лимит символов превышен (максимум 255 символов)")]
    public string Password { get; set; } = null!;

    [NotMapped]
    public string? Role { get; set; } = null!;

    [NotMapped]
    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string? Surname { get; set; } = null!;

    [NotMapped]
    [Required(ErrorMessage = "Это поле обязательное")]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string? Name { get; set; } = null!;

    [NotMapped]
    [StringLength(45, ErrorMessage = "Лимит символов превышен (максимум 45 символов)")]
    public string? Patronymic { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual Projecter? Projecter { get; set; }
}
