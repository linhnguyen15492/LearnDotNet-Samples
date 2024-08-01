using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.SDK
{
    public interface ICallable
    {
        Task InvokeAsync(MiddlewareContext context, CancellationToken cancellationToken);
    }
}
