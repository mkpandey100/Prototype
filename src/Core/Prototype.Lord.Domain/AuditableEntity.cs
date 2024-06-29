namespace Prototype.Lord.Domain
{
    public class AuditableEntity
    {
        public Guid CreatedById { get; set; }
        public DateTime Created { get; set; }
        public Guid? LastModifiedById { get; set; }
        public DateTime? LastModified { get; set; }
    }
}