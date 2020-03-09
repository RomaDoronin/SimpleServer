using System;
using System.Net.Sockets;
using System.Text;
using SimpleHTTPServer.Modules;

namespace SimpleHTTPServer.HTTPInteraction
{
    class Client
    {
        private const string COMPANY_PREFIX = "/sklexp";

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
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendError(client, context.contextResponse.statusCode); return; }

            CheckCompanyPrefix(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendError(client, context.contextResponse.statusCode); return; }
            IModule module = GetModule(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendError(client, context.contextResponse.statusCode); return; }

            module.Validate(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendError(client, context.contextResponse.statusCode); return; }
            module.ProcessRequest(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendError(client, context.contextResponse.statusCode); return; }

            // Формируем response
            string statusKey = "status";
            string statusValue = "success";
            string messageKey = "message";
            string messageValue = "ok";

            string jsonResponse = string.Format("\n    \"{0}\": \"{1}\",\n    \"{2}\": \"{3}\"\n",
                statusKey, statusValue, messageKey, messageValue);
            jsonResponse = "{" + jsonResponse + "}";

            string httpVersion = "HTTP/1.1";
            context.contextResponse.contentType = "application/json";
            context.contextResponse.contentLength = jsonResponse.Length;

            string Headers = string.Format("{0} 200 OK\nContent: {1}\nContent-Length: {2}\n\n{3}",
                httpVersion, context.contextResponse.contentType, context.contextResponse.contentLength, jsonResponse);

            byte[] HeadersBuffer = Encoding.ASCII.GetBytes(Headers);
            client.GetStream().Write(HeadersBuffer, 0, HeadersBuffer.Length);

            client.Close();
        }

        private IModule GetModule(Context context)
        {
            string[] words = context.contextRequest.url.Split(new char[] { '/' });
            string moduleName = words[(int)Constants.UrlPositionNumber.MODULE_NAME];
            return Constants.ModuleList.GetModuleByModuleName(moduleName);
        }

        private void CheckCompanyPrefix(Context context)
        {
            if (!context.contextRequest.url.StartsWith(COMPANY_PREFIX))
            {
                context.contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
            }
        }

        private void SendError(TcpClient client, Constants.StatusCode statusCode)
        {
            throw new NotImplementedException();
        }
    }
}
