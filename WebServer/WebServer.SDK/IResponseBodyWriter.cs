namespace WebServer.SDK
{
    public interface IResponseBodyWriter
    {
        Task WriteAsync (Stream bodyStream);
    }
}