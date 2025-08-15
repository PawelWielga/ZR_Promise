namespace AppAudit.Contracts.Paging;

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Total);
