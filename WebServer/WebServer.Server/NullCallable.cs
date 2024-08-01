using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server
{
    internal class NullCallable : ICallable
    {
        public Task InvokeAsync(MiddlewareContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
