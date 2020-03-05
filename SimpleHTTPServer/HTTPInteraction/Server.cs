using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleHTTPServer.HTTPInteraction
{
    class Server
    {
        private TcpListener listener;

        static void ClientThread(object stateInfo)
        {
            new Client((TcpClient)stateInfo);
        }

        public Server(int port)
        {
            Console.WriteLine("Server prepare to start");
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server start");

            while (true)
            {
                Console.WriteLine("Weit client");
                // Принимаем нового клиента
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");

                // Создаем поток
                Thread thread = new Thread(new ParameterizedThreadStart(ClientThread));

                // И запускаем этот поток, передавая ему принятого клиента
                thread.Start(client);
            }
        }

        ~Server()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }
    }
}
