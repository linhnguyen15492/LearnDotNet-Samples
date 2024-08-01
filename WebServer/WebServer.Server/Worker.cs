using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using WebServer.Middleware.StaticContent;
using WebServer.SDK;
using WebServer.Server.DefaultMiddlewares;
using WebServer.Server.RequestReaders;
using WebServer.SDK.ResponseBodyWriters;

namespace WebServer.Server;

public class Worker : BackgroundService
    {
    private WebServerOptions options;
    private readonly IRequestReaderFactory requestReaderFactory;
    private readonly IResponseWriterFactory responseWriterFactory;
    private readonly ILogger<Worker> _logger;

    private ICallable firstMiddleware = new NullCallable();

    public Worker(WebServerOptions options,
        IRequestReaderFactory requestReaderFactory,
        IResponseWriterFactory responseWriterFactory,
        ILogger<Worker> logger)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.requestReaderFactory = requestReaderFactory;
        this.responseWriterFactory = responseWriterFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        AddMiddleware(new NotFoundMiddleware());
        AddMiddleware(new StaticContentMiddleware());


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
        IRequestReader requestReader = requestReaderFactory.Create(socket);

        // read request from socket
        WRequest request = await requestReader.ReadRequestAsync(CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancelationTokenSource.Token).Token);
        // create response 

        var response = new WResponse()
        {
        };

        var invokeCancelationTokenSource = new CancellationTokenSource(15000);

        // handle the request
        await InvokeMiddlewaresAsync(new MiddlewareContext() { 
            Request = request, Response = response 
        },
        CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, invokeCancelationTokenSource.Token).Token
        );


        // send back the response
        IResponseWriter responseWriter = responseWriterFactory.Create(socket);
        await responseWriter.SendResponseAsync(response);

        socket.Close();
    }

    private async Task InvokeMiddlewaresAsync(MiddlewareContext context, CancellationToken cancellationToken)
    {
        await firstMiddleware.InvokeAsync(context, cancellationToken);
    }

    private void AddMiddleware(IMiddleware middleware)
    {
        firstMiddleware = new Callable() { Middleware = middleware, Next = firstMiddleware };
    }
}
