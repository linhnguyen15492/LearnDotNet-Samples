using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server.ResponseWriters
{
    internal class DefaultResponseWriter : IResponseWriter
    {
        private Socket socket;
        private ILogger<DefaultResponseWriter> logger;

        public DefaultResponseWriter(Socket socket, ILogger<DefaultResponseWriter> logger)
        {
            this.socket = socket;
            this.logger = logger;
        }

        public async Task SendResponseAsync(WResponse response)
        {
            var stream = new NetworkStream(socket);
            var streamWriter = new StreamWriter(stream);

            string reasonPhrase = response.ReasonPhrase;
            if (string.IsNullOrEmpty(reasonPhrase))
            {
                reasonPhrase = HttpReasonPhrases.GetByCode(response.ResponseCode);
            }

            await streamWriter.WriteLineAsync($"{response.HttpVersion} {(int)response.ResponseCode} {reasonPhrase}");
            await streamWriter.WriteLineAsync($"Content-Length: {response.ContentLength}");
            await streamWriter.WriteLineAsync($"Content-Type: {response.ContentType}");
            await streamWriter.WriteLineAsync("Connection: close");
            await streamWriter.WriteLineAsync();
            await streamWriter.FlushAsync();
            if (response.ContentLength > 0)
            {
                await response.ResponseBodyWriter.WriteAsync(stream);
            }
            await stream.FlushAsync();
        }
    }
}
