using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public interface IResponseWriterFactory
    {
        IResponseWriter Create(Socket socket);
    }
}
