using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Lord.Domain.BizVueModels.Projects;

public class Project
{
    public Guid Id { get; set; }
    public Guid CreatedUserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid StatusId { get; set; }
    public decimal CompletePercentage { get; set; }
    public bool IsArchived { get; set; }
    public int BoardType { get; set; }
    public Guid? MissionId { get; set; }
    public string CustomStatus { get; set; }
    public bool IsDeleted { get; set; }
}