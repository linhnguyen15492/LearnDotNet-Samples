using Microsoft.VisualBasic;
using System.Net;
    using System.Net.Sockets;
using System.Text;
using WebServer.SDK;
using WebServer.Server.RequestReaders;
using WebServer.Server.ResponseBodyWriters;

    namespace WebServer.Server;

    public class Worker : BackgroundService
    {
    private WebServerOptions options;
    private readonly ILogger<Worker> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public Worker(WebServerOptions options, ILogger<Worker> logger, ILoggerFactory loggerFactory)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options)); 
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endPoint = new IPEndPoint(string.IsNullOrEmpty(options.IPAddress) ? IPAddress.Any : IPAddress.Parse(options.IPAddress) , options.Port);
        using var serverSocket = new Socket(
            endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp
            );
        serverSocket.Bind(endPoint);

        _logger.LogInformation("Listening... (port: {p})", options.Port);
        serverSocket.Listen();

        var clientConnections = new List<ClientConnection>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var clientSocket = await serverSocket.AcceptAsync(stoppingToken);

            if (clientSocket != null) {
                var t = HandleNewClientConnectionAsync(clientSocket, stoppingToken);
                clientConnections.Add(new ClientConnection()
                {
                    HandlerTask = t
                });
            }
        }

        Task.WaitAll(clientConnections.Select(c => c.HandlerTask).ToArray());

        serverSocket.Close();
    }

    private async Task HandleNewClientConnectionAsync(Socket socket, CancellationToken stoppingToken)
    {
        var cancelationTokenSource = new CancellationTokenSource(3000);
        IRequestReader requestReader = new DefaultRequestReader(socket, _loggerFactory.CreateLogger<DefaultRequestReader>());

        // read request from socket
        WRequest request = await requestReader.ReadRequestAsync(CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancelationTokenSource.Token).Token);
        // create response 

        var content = @"<!DOCTYPE html>
<html>
<body>

<h1>My First Heading</h1>
<p>My first paragraph.</p>

</body>
</html>";
        var response = new WResponse()
        {
            ResponseCode = HttpResponseCodes.OK,
            ContentLength = content.Length,
            ResponseBodyWriter = new StringResponseBodyWriter(content)
        };

        // handle the request

        // send back the response
        await SendResponseAsync(socket, response);

        socket.Close();
    }


    private async Task SendResponseAsync(Socket socket, WResponse response)
    {
        var stream = new NetworkStream(socket);
        var streamWriter = new StreamWriter(stream);

        string reasonPhrase = response.ReasonPhrase;
        if (string.IsNullOrEmpty(reasonPhrase)) {
            reasonPhrase = HttpReasonPhrases.GetByCode(response.ResponseCode);
        }

        await streamWriter.WriteLineAsync($"{response.HttpVersion} {(int)response.ResponseCode} {reasonPhrase}");
        await streamWriter.WriteLineAsync($"Content-Length: {response.ContentLength}");
        await streamWriter.WriteLineAsync($"Content-Type: {response.ContentType}");
        await streamWriter.WriteLineAsync();
        await streamWriter.FlushAsync();
        if (response.ContentLength > 0) {
            await response.ResponseBodyWriter.WriteAsync(stream);
        }
        await stream.FlushAsync();
    }

    
}
