using Microsoft.VisualBasic;
using System.Net;
    using System.Net.Sockets;
using System.Text;
using WebServer.SDK;
using WebServer.Server.RequestReaders;

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

        // handle the request

        // send back the response
        await SendResponseAsync(socket);

        socket.Close();
    }


    private async Task SendResponseAsync(Socket socket)
    {
        var stream = new NetworkStream(socket);
        var streamWriter = new StreamWriter(stream);

        await streamWriter.WriteLineAsync("200 OK");
        await streamWriter.FlushAsync();
    }

    
}
