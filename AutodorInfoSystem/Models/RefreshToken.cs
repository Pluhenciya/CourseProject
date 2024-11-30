using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutodorInfoSystem.Models
{
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("token")]
        [Required]
        public string Token { get; set; }

        [Column("expires")]
        [Required]
        public DateTime Expires { get; set; }

        [Column("is_revoked")]
        public bool IsRevoked { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
