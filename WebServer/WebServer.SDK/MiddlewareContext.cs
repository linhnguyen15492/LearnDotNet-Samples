namespace WebServer.SDK
{
    public class MiddlewareContext
    {
        public required WRequest Request { get; init; }
        public required WResponse Response { get; init; }
    }
}