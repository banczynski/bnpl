using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; init; } = default!;
        public string UpdatedBy { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
