using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;
using WebServer.Server.RequestReaders;
using WebServer.Server.ResponseWriters;

namespace WebServer.Server
{
    internal class ResponseWriterFactory : IResponseWriterFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public ResponseWriterFactory(ILoggerFactory loggerFactory) { 
            this.loggerFactory = loggerFactory;
        }

        public IResponseWriter Create(Socket socket)
        {
            return new DefaultResponseWriter(socket, loggerFactory.CreateLogger<DefaultResponseWriter>());
        }
    }
}
