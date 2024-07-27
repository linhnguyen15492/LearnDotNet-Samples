using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public class WRequest
    {
        public required WMethods Method { get; set; }
        public required string Url { get; set; }
        public required string HttpVersion { get; set; }
        public required string Host { get; set; } 
        public bool IsKeepAlive { get; set; } = false;
        public required IDictionary<string, StringValues> Headers { get; set; }
    }
}
