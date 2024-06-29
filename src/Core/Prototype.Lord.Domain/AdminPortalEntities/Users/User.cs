using Microsoft.AspNetCore.Identity;

namespace Prototype.Lord.Domain.AdminPortalModels.Users
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get => $"{FirstName} {LastName}"; }
        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public string Misc { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}