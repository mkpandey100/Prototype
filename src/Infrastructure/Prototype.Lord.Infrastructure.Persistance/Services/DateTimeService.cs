using Prototype.Lord.Domain.Interfaces;

namespace Prototype.Lord.Infrastructure.Persistance.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}