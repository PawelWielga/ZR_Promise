namespace AppAudit.Api.Endpoints;

public sealed class GetProgramsPageRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public string? Search { get; init; }
    public bool? RequiresKey { get; init; }
    public bool? HasKey { get; init; }
}
