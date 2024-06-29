using System.ComponentModel.DataAnnotations;

namespace Prototype.Lord.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}