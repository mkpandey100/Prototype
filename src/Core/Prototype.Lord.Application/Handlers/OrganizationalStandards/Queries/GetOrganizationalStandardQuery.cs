using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Queries;

public class GetOrganizationalStandardQuery : PageQuery, IRequest<ResponseOutputDto<OrganizationalStandardDetailResponseDto>>
{
    public Guid Id { get; set; }
}

public class GetOrganizationalStandardQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetOrganizationalStandardQuery, ResponseOutputDto<OrganizationalStandardDetailResponseDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<ResponseOutputDto<OrganizationalStandardDetailResponseDto>> Handle(GetOrganizationalStandardQuery request, CancellationToken cancellationToken)
    {
        var organizationalStandard = await _dbContext.OrganizationalStandards
            .Where(m => m.Id == request.Id).Select(m => new OrganizationalStandardDetailResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                OrganizationalStandardType = m.OrganizationalStandardType,
            }).FirstOrDefaultAsync(cancellationToken);

        return new ResponseOutputDto<OrganizationalStandardDetailResponseDto>()
        {
            Data = organizationalStandard,
            Status = Status.Success,
            Message = organizationalStandard != null ? Message.Success : Message.NotFound
        };
    }
}