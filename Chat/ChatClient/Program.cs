using ChatProtocol;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, ChatProtocol.Constants.DefaultChatPort);

            var clientSocket = new Socket(
                endPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
                );

            await clientSocket.ConnectAsync(endPoint);
            var buffer = new byte[1024];
            var r = await clientSocket.ReceiveAsync(buffer);
            if (r == 0)
            {
                showConnectionError();
                return;
            }

            var welcomeText = Encoding.UTF8.GetString(buffer, 0, r);
            if (!Constants.WelcomeText.Equals(welcomeText))
            {
                showConnectionError();
                return;
            }

            Console.WriteLine(welcomeText);

            while (true)
            {
                Console.Write("Enter your message: ");
                var msg = Console.ReadLine();

                if (string.IsNullOrEmpty(msg)) {
                    await closeConnectionAsync(clientSocket);
                    return;
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(msg);
                    await clientSocket.SendAsync(bytes);
                }
            }
        }

        private static async Task closeConnectionAsync(Socket clientSocket)
        {
            var bytes = Encoding.UTF8.GetBytes(Constants.CommandShutdown);
            await clientSocket.SendAsync(bytes);

            clientSocket.Close();   
        }

        private static void showConnectionError()
        {
            Console.WriteLine("Invalid protocol!");
        }
    }
}
