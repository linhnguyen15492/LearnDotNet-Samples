using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public class NullResponseBodyWriter : IResponseBodyWriter
    {
        public Task WriteAsync(Stream bodyStream)
        {
            return Task.CompletedTask;
        }

        public static readonly NullResponseBodyWriter Instance = new();
    }
}
