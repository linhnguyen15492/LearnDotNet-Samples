
using WebServer.SDK;

namespace WebServer.Server
{
    internal class HeaderLine
    {
        internal static bool TryParse(string? headerLine, out WHeader? header)
        {
            ArgumentNullException.ThrowIfNull(headerLine, nameof(headerLine));

            var idx = headerLine.IndexOf(':');
            if (idx < 0) { 
                header = default;
                return false;
            }

            header = new WHeader() { 
                Name = headerLine[..idx].Trim(),
                Values = headerLine[(idx + 1)..].Trim()
            };

            return true;
        }
    }
}