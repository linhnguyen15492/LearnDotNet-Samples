using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server
{
    internal class WRequestBuilder
    {
        WMethods Method { get; set; } = WMethods.Get;
        public string Url { get; set; } = string.Empty;
        public string HttpVersion { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public bool IsKeepAlive { get; set; } = false;
        public IDictionary<string, StringValues> Headers { get; set; } = new Dictionary<string, StringValues>();

        public WRequest Build()
        {
            Validate();

            var request = new WRequest() { 
                Method = Method,
                Url = Url, 
                Host = Host,
                HttpVersion = HttpVersion,
                Headers = Headers,
                IsKeepAlive = IsKeepAlive,
            };

            return request;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException(nameof(Url));
            }
            if (string.IsNullOrEmpty(Host))
            {
                throw new ArgumentNullException(nameof(Host));
            }
            if (string.IsNullOrEmpty(HttpVersion))
            {
                throw new ArgumentNullException(nameof(HttpVersion));
            }
        }

        internal void AddHeader(WHeader header)
        {
            if ("Host".Equals(header.Name, StringComparison.OrdinalIgnoreCase))
            {
                this.Host = header.Values.First() ?? string.Empty;
            }
            else if ("Connection".Equals(header.Name, StringComparison.OrdinalIgnoreCase))
            {
                this.IsKeepAlive = "keep-alive".Equals(header.Values.First());
            }

            if (!Headers.TryGetValue(header.Name, out var values))
            {
                Headers.Add(header.Name, header.Values);
            }
            else
            {
                Headers[header.Name] = StringValues.Concat(values, header.Values);
            }
        }
    }
}
