using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.SDK.ResponseBodyWriters
{
    public class StringResponseBodyWriter : IResponseBodyWriter
    {
        private readonly byte[] contentBytes;

        public StringResponseBodyWriter(string content)
        {
            contentBytes = Encoding.UTF8.GetBytes(content);
        }

        public async Task WriteAsync(Stream bodyStream)
        {
            await bodyStream.WriteAsync(contentBytes);
        }
    }
}
