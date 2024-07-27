using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Server
{
    public class WebServerOptions
    {
        public string IPAddress { get; set; } = string.Empty;
        public int Port { get; set; } = 80;
    }
}
