using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server
{
    internal class RequestLine
    {
        public WMethods Method { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}
