using AppAudit.Contracts.Paging;
using MediatR;

namespace AppAudit.Contracts.Programs.Queries;

public sealed record GetProgramsPageQuery(
    int Page = 1,
    int PageSize = 25,
    string? Search = null,
    bool? RequiresKey = null,
    bool? HasKey = null
) : IRequest<PagedResult<ProgramRecord>>;