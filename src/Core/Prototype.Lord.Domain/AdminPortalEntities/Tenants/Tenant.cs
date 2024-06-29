namespace Prototype.Lord.Domain.AdminPortalModels.Tenants
{
    public partial class Tenant : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SubDomain { get; set; }
        public string StripeCustomerId { get; set; }
        public string Caption { get; set; }
        public string DbConnection { get; set; }
        public bool IsActive { get; set; }
        public string LogoUrl { get; set; }
        public int Status { get; set; }
        public string Misc { get; set; }
        public bool IsDeleted { get; set; }
        public decimal PricePerUserYearly { get; set; }
        public decimal PricePerUserMonthly { get; set; }
    }
}