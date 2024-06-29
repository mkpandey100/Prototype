using System.ComponentModel.DataAnnotations;

namespace Prototype.Lord.Domain.BizVueModels;

public class OrganizationalStandard: AuditableEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int OrganizationalStandardType { get; set; }
    public bool IsDeleted { get; set; }
}
