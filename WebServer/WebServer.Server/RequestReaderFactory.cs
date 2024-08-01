using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;
using WebServer.Server.RequestReaders;

namespace WebServer.Server
{
    internal class RequestReaderFactory : IRequestReaderFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public RequestReaderFactory(ILoggerFactory loggerFactory) { 
            this.loggerFactory = loggerFactory;
        }

        public IRequestReader Create(Socket socket)
        {
            return new DefaultRequestReader(socket, loggerFactory.CreateLogger<DefaultRequestReader>());
        }
    }
}
