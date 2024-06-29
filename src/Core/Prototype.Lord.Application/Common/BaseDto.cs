namespace Prototype.Lord.Application.Common;

public class BaseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class OptionalBaseDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
}