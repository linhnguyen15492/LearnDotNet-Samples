using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server.DefaultMiddlewares
{
    internal class NullMiddleware : IMiddleware
    {
        public Task InvokeAsync(MiddlewareContext context, ICallable next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
