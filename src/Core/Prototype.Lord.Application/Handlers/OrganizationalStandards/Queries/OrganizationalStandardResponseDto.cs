namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Queries;

public class OrganizationalStandardResponseDto
{
    public int OrganizationalStandardType { get; set; }
    public List<OrganizationalStandardDatumResponseDto> OrganizationalStandards { get; set; }
}

public class OrganizationalStandardDatumResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CreatedById { get; set; }
}

public class OrganizationalStandardDetailResponseDto : OrganizationalStandardDatumResponseDto
{
    public int OrganizationalStandardType { get; set; }
}