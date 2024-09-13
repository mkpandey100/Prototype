using Microsoft.AspNetCore.Identity;

namespace Prototype.Lord.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public Guid CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? LastModifiedById { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get { return (FirstName + " " + LastName).Trim(); } }
    }
}
