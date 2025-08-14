using AppAudit.Core.Models;

namespace AppAudit.Infrastructure.Writers;

internal sealed class ApiResultWriter(string endpoint, string deviceId) : IResultWriter
{
    private readonly string _endpoint = endpoint;
    private readonly string _deviceId = deviceId;

    public void WriteAll(IEnumerable<ProgramEntry> items, DateTimeOffset eventUtc, string scanId)
    {
        // TODO: Zaimplementowa wysyanie danych do API wraz z identyfikatorem urzadzenia
    }
}
