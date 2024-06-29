using MediatR;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.BizVueModels;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Commands;

public class CreateOrganizationalStandardCommand : IRequest<OutputDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int OrganizationalStandardType { get; set; }
}

public class CreateOrganizationalStandardCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateOrganizationalStandardCommand, OutputDto>
{
    private readonly IApplicationDbContext _dbContext = context;

    public async Task<OutputDto> Handle(CreateOrganizationalStandardCommand request, CancellationToken cancellationToken)
    {
        var entity = new OrganizationalStandard
        {
            Name = request.Name,
            Description = request.Description,
            Created = DateTime.UtcNow,
            OrganizationalStandardType = request.OrganizationalStandardType
        };
        _dbContext.OrganizationalStandards.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OutputDto
        {
            Status = Status.Success,
            Message = $"Successfully created Organizational Standard {request.Name}."
        };
    }
}