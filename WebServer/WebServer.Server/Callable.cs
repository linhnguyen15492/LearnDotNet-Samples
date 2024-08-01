using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server
{
    internal class Callable : ICallable
    {
        public required IMiddleware Middleware { get; set; }
        public ICallable? Next { get; set; }

        public async Task InvokeAsync(MiddlewareContext context, CancellationToken cancellationToken)
        {
            if (Next == null)
            {
                throw new InvalidOperationException();
            }
            Console.WriteLine($"calling to {Middleware}");
            await Middleware.InvokeAsync(context, Next, cancellationToken);
        }
    }
}
