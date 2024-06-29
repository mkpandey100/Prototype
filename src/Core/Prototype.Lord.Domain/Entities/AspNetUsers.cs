namespace Prototype.Lord.Domain.BizVueModels;

public class AspNetUsers
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImagePath { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsDisabled { get; set; }
}