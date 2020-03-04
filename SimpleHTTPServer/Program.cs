using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    class Server
    {
        private TcpListener listener;

        static void ClientThread(Object stateInfo)
        {
            new Client((TcpClient)stateInfo);
        }

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                // Принимаем нового клиента
                TcpClient client = listener.AcceptTcpClient();

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

        static void Main(string[] args)
        {
            new Server(80);
        }
    }
}
