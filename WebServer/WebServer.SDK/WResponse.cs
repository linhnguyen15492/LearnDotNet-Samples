using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public class WResponse
    {
        public string HttpVersion { get; set; } = "HTTP/1.1";
        public HttpResponseCodes ResponseCode { get; set; } = HttpResponseCodes.NotFound;
        public string ReasonPhrase { get; set; } = string.Empty;
        public int ContentLength { get; set; }
        public string ContentType { get; set; } = "text/html";
        public IResponseBodyWriter ResponseBodyWriter { get; set; } = NullResponseBodyWriter.Instance;
    }
}
