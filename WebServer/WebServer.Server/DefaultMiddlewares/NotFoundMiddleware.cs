using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;
using WebServer.SDK.ResponseBodyWriters;

namespace WebServer.Server.DefaultMiddlewares
{
    internal class NotFoundMiddleware : IMiddleware
    {
        private static readonly IResponseBodyWriter EmptyBodyContentWriter = new StringResponseBodyWriter("");

        public Task InvokeAsync(MiddlewareContext context, ICallable next, CancellationToken cancellationToken)
        {
            context.Response.ContentLength = 0;
            context.Response.ResponseCode = HttpResponseCodes.NotFound;
            context.Response.ContentType = "text/html";
            context.Response.ResponseBodyWriter = EmptyBodyContentWriter;

            return Task.CompletedTask;
        }
    }
}
