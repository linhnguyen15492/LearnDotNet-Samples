using WebServer.SDK;
using WebServer.SDK.ResponseBodyWriters;

namespace WebServer.Middleware.StaticContent
{
    public class StaticContentMiddleware : IMiddleware
    {
        private string root = "c:\\wwwroot";

        public async Task InvokeAsync(MiddlewareContext context, ICallable next, CancellationToken cancellationToken)
        {
            if (context.Request.Method == WMethods.Get)
            {
                var url = context.Request.Url;
                if (url.StartsWith("/"))
                {
                    url = url.Substring(1);
                }
                url = url.Replace("/", "\\");

                var file = new FileInfo(Path.Combine(root, url));
                if (file.Exists)
                {
                    var fileContent = await File.ReadAllTextAsync(file.FullName);
                    context.Response.ContentType = "text/html";
                    context.Response.ContentLength = fileContent.Length;
                    context.Response.ResponseBodyWriter = new StringResponseBodyWriter(fileContent);

                    context.Response.ResponseCode = HttpResponseCodes.OK;
                }
                else
                {
                    await next.InvokeAsync(context, cancellationToken);
                }
            }
            else
            {
                await next.InvokeAsync(context, cancellationToken);
            }
        }
    }
}
