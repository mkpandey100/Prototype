using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Lord.Domain.BizVueModels.Tasks;

public class Tasks
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public int Row { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? ProjectSectionId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public Guid PriorityId { get; set; }
    public int CompletePercent { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool IsTernary { get; set; }
    public int CustomStatusId { get; set; }
}