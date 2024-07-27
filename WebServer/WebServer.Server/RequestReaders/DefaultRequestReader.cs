using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.SDK;

namespace WebServer.Server.RequestReaders
{
    internal class DefaultRequestReader: IRequestReader
    {
        private readonly ILogger<DefaultRequestReader> _logger;
        private readonly Socket socket;

        public DefaultRequestReader(Socket socket, ILogger<DefaultRequestReader> logger) { 
            this.socket = socket;
            this._logger = logger;
        }

        public async Task<WRequest> ReadRequestAsync(CancellationToken cancellationToken)
        {
            var stream = new NetworkStream(socket);
            var reader = new StreamReader(stream, Encoding.ASCII);

            var requestBuilder = new WRequestBuilder();

            var requestLineString = await reader.ReadLineAsync(cancellationToken);
            _logger.LogInformation(requestLineString);

            if (requestLineString != null)
            {
                if (RequestLineParser.TryParse(requestLineString, out var requestLine) && requestLine != null)
                {
                    requestBuilder.Url = requestLine.Url;
                    requestBuilder.HttpVersion = requestLine.Version;

                    var headerLine = await reader.ReadLineAsync(cancellationToken);
                    while (!string.IsNullOrEmpty(headerLine))
                    {
                        _logger.LogInformation(headerLine);
                        if (HeaderLine.TryParse(headerLine, out var header))
                        {
                            requestBuilder.AddHeader(header!);
                        }

                        headerLine = await reader.ReadLineAsync(cancellationToken);
                    }
                }
            }

            return requestBuilder.Build();
        }
    }
}
