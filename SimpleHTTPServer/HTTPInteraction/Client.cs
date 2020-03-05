using System;
using System.Net.Sockets;
using System.Text;

namespace SimpleHTTPServer.HTTPInteraction
{
    class Client
    {
        public Client(TcpClient client)
        {
            // Объявим строку, в которой будет хранится запрос клиента
            string request = "";
            // Буфер для хранения принятых от клиента данных
            byte[] buffer = new byte[1024];
            // Переменная для хранения количества байт, принятых от клиента
            int Count;

            // Читаем из потока клиента
            Count = client.GetStream().Read(buffer, 0, buffer.Length);
            // Преобразуем эти данные в строку и добавим ее к переменной Request
            request += Encoding.ASCII.GetString(buffer, 0, Count);
            Console.WriteLine(request);
            Context context = new Context(request);

            // Если запрос не удался
            if (context.isError)
            {
                // Передаем клиенту ошибку 400 - неверный запрос
                SendError(client, 400);
                return;
            }

            if (context.httpReqName == "POST")
            {
                string jsonResponse = "{\n    \"key1\": \"value1\",\n    \"key2\": \"value2\"\n}";
                string Headers = "HTTP/1.1 200 OK\nContent-Type: " + context.contentType + "\nContent-Length: " + context.contentLength + "\n\n" + jsonResponse;
                byte[] HeadersBuffer = Encoding.ASCII.GetBytes(Headers);
                client.GetStream().Write(HeadersBuffer, 0, HeadersBuffer.Length);
            }

            client.Close();
        }



        private void SendError(TcpClient client, int v)
        {
            throw new NotImplementedException();
        }
    }
}
