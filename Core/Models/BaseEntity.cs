using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public abstract class BaseEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; init; }

        [Column("code")]
        public Guid Code { get; init; }

        [Column("created_at")]
        public DateTime CreatedAt { get; init; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_by")]
        public Guid CreatedBy { get; init; } = default!;

        [Column("updated_by")]
        public Guid UpdatedBy { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
