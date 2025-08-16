using AppAudit.Contracts;
using AppAudit.Application.Abstractions;
using System.Linq.Expressions;

namespace AppAudit.Application.Mapping;

public static class ProgramMappings
{
    public static readonly Expression<Func<ProgramEntry, ProgramRecord>> ToRecord =
        p => new ProgramRecord(
            p.ProgramId,
            p.Name,
            p.Version,
            p.Publisher,
            p.DiscoveredAt,
            p.RequiresLicense,
            p.LicenseKey
        );

    private static readonly Func<ProgramEntry, ProgramRecord> ToRecordFunc = ToRecord.Compile();

    public static ProgramRecord ToDto(this ProgramEntry e) => ToRecordFunc(e);
}

