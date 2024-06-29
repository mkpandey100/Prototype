using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Common.Exceptions;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Commands;

public class UpdateOrganizationalStandardCommand : IRequest<OutputDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int OrganizationalStandardType { get; set; }
    public IReadOnlyCollection<string> Permissions { get; set; }
}

public class UpdateOrganizationalStandardCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService) : IRequestHandler<UpdateOrganizationalStandardCommand, OutputDto>
{
    private readonly IApplicationDbContext _dbContext = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<OutputDto> Handle(UpdateOrganizationalStandardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.OrganizationalStandards.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException();

        if (request.Permissions.Contains(Permissions.UpdateOwnOS) && entity.CreatedById != _currentUserService.UserId)
            throw new UnauthorizedAccessException();

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.OrganizationalStandardType = request.OrganizationalStandardType;
        entity.LastModified = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OutputDto
        {
            Status = Status.Success,
            Message = $"Successfully Updated Organizational Standard {entity.Name}."
        };
    }
}