using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public class HttpReasonPhrases
    {
        public static string GetByCode(HttpResponseCodes code) => code switch
        {
            HttpResponseCodes.OK => "OK",
            HttpResponseCodes.NotFound => "Not Found",
            _ => throw new ArgumentOutOfRangeException("Unknown HTTP status code")
        };
    }
}
