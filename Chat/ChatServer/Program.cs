using ChatProtocol;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    internal class Program
    {
        // đây là phiên bản đã được chỉnh sửa, nếu bạn muốn xem code trình bày trên video Youtube (bài học lập trình Socket), hãy 
        // tham khảo: https://github.com/daohainam/LearnDotNet-Samples/tree/af72bd21659bbc164ff85284ce654e8a2f1339f4 

        static async Task Main(string[] args)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, ChatProtocol.Constants.DefaultChatPort);
            var serverSocket = new Socket(
                endPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
                );

            serverSocket.Bind(endPoint);

            Console.WriteLine($"Listening... (port {endPoint.Port})");

            serverSocket.Listen();


            CancellationTokenSource cancellationTokenSource = new();
            var cancellationToken = cancellationTokenSource.Token;

            var acceptTask = AcceptConnectionsAsync(serverSocket, cancellationToken);

            Console.WriteLine("Press Enter to shutdown the server...");
            Console.ReadLine();

            cancellationTokenSource.Cancel();
            await acceptTask;
        }

        private static async Task HandleClientRequestAsync(Socket clientSocket, int clientId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Client {clientId}] connected!");

            var welcomeBytes = Encoding.UTF8.GetBytes(Constants.WelcomeText);
            await clientSocket.SendAsync(welcomeBytes, cancellationToken);

            var buffer = new byte[1024];

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var r = await clientSocket.ReceiveAsync(buffer, cancellationToken: cancellationToken);
                    var msg = Encoding.UTF8.GetString(buffer, 0, r);

                    if (msg.Equals(Constants.CommandShutdown))
                    {
                        CloseConnection(clientSocket);
                        Console.WriteLine($"[Client {clientId}] disconnected!");
                        break;
                    }

                    Console.WriteLine($"[Client {clientId}]: {msg}");
                } catch (OperationCanceledException)
                { }
            }
        }

        private static void CloseConnection(Socket clientSocket)
        {
            clientSocket.Close();
        }

        private static async Task AcceptConnectionsAsync(Socket serverSocket, CancellationToken cancellationToken)
        {
            var clientHandlers = new List<Task>();
            int clientId = 1;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var clientSocket = await serverSocket.AcceptAsync(cancellationToken);
                    var t = HandleClientRequestAsync(clientSocket, clientId++, cancellationToken);
                    clientHandlers.Add(t);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            await Task.WhenAll(clientHandlers);
        }
    }
}
