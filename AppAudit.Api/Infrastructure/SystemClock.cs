using AppAudit.Application.Abstractions;

namespace AppAudit.Api.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
