using MediatR;
using Microsoft.EntityFrameworkCore;
using Prototype.Lord.Application.Common;
using Prototype.Lord.Application.Interfaces;
using Prototype.Lord.Domain.Constants;
using Prototype.Lord.Domain.Enums;

namespace Prototype.Lord.Application.Handlers.OrganizationalStandards.Queries;

public class GetAllOrganizationalStandardQuery : PageQuery, IRequest<ListResponseOutputDto<OrganizationalStandardResponseDto>>
{
}

public class GetAllOrganizationalStandardQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAllOrganizationalStandardQuery, ListResponseOutputDto<OrganizationalStandardResponseDto>>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<ListResponseOutputDto<OrganizationalStandardResponseDto>> Handle(GetAllOrganizationalStandardQuery request, CancellationToken cancellationToken)
    {
        var organizationalStandardsQuery = _dbContext.OrganizationalStandards
            .Where(m => !m.IsDeleted && (m.Name.Contains(request.SearchValue) || string.IsNullOrEmpty(request.SearchValue)));

        var groupedData = await organizationalStandardsQuery
                                            .GroupBy(m => m.OrganizationalStandardType)
                                            .OrderBy(g => g.Key)
                                            .Select(g => new OrganizationalStandardResponseDto
                                            {
                                                OrganizationalStandardType = g.Key,
                                                OrganizationalStandards = g.Select(m => new OrganizationalStandardDatumResponseDto()
                                                {
                                                    Id = m.Id,
                                                    Name = m.Name,
                                                    Description = m.Description,
                                                    CreatedById = m.CreatedById,
                                                }).ToList()
                                            }).ToListAsync(cancellationToken);

        return new ListResponseOutputDto<OrganizationalStandardResponseDto>()
        {
            Data = groupedData,
            Status = Status.Success,
            Message = groupedData.Count != 0 ? Message.Success : Message.NotFound
        };
    }
}