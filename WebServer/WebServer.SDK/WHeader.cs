using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public class WHeader
    {
        public required string Name { get; set; }
        public required StringValues Values { get; set; }
    }
}
