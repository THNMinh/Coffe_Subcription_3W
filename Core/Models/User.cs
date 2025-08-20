using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("users")]
    public partial class User : Entity
    {
        [Key]
        [Column("user_id")]
        public override int Id { get; set; }

        [Column("username")]
        public string Username { get; set; } = null!;

        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [Column("full_name")]
        public string FullName { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("role_id")]
        public int? RoleId { get; set; }

        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();

        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    }
}
