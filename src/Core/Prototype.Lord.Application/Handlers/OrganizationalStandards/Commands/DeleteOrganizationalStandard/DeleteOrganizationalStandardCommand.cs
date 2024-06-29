using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Common.Exceptions;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Commands;

public class DeleteOrganizationalStandardCommand : IRequest<OutputDto>
{
    public Guid Id { get; set; }
    public IReadOnlyCollection<string> Permissions { get; set; }
}

public class DeleteOrganizationalStandardCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService) : IRequestHandler<DeleteOrganizationalStandardCommand, OutputDto>
{
    private readonly IApplicationDbContext _dbContext = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<OutputDto> Handle(DeleteOrganizationalStandardCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.OrganizationalStandards.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException();

        if (request.Permissions.Contains(Permissions.DeleteOwnOS) && entity.CreatedById != _currentUserService.UserId)
            throw new UnauthorizedAccessException();

        entity.IsDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OutputDto
        {
            Status = Status.Success,
            Message = $"Successfully Deleted Organizational Standard {entity.Name}."
        };
    }
}