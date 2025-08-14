using System.Text;

namespace AppAudit.Core.Helpers;

public static class CsvUtils
{
    public static string Escape(string? s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        var need = s.IndexOfAny([',', '"', '\n', '\r']) >= 0;
        return need ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
    }

    public static string[] SplitLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return Array.Empty<string>();
        var result = new List<string>();
        var sb = new StringBuilder();
        var inQuotes = false;
        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            else
            {
                if (c == ',')
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else if (c == '"')
                {
                    inQuotes = true;
                }
                else
                {
                    sb.Append(c);
                }
            }
        }
        result.Add(sb.ToString());
        return result.ToArray();
    }
}