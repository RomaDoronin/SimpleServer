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
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }

            CheckCompanyPrefix(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }
            IModule module = GetModule(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }

            module.Validate(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }
            module.ProcessRequest(context);

            // Отправляем ответ
            SendResponse(client, context);
        }

        private string PrepareResponse(Context context)
        {
            string statusValue = "success";
            string messageValue = "ok";
            if (context.contextResponse.statusCode != Constants.StatusCode.OK)
            {
                statusValue = "error";
                messageValue = context.contextResponse.message;
            }

            string jsonResponse = string.Format("\n    \"status\": \"{0}\",\n    \"message\": \"{1}\"\n", statusValue, messageValue);
            jsonResponse = "{" + jsonResponse + "}";

            string httpVersion = "HTTP/1.1";
            context.contextResponse.contentType = "application/json";
            string responseCode = context.contextResponse.statusCode.ToString("d");
            string responseStatus = StrManualLib.ConstFormatToStringFormat(context.contextResponse.statusCode.ToString("g"));

            return string.Format("{0} {1} {2}\nContent-Type: {3}\nContent-Length: {4}\n\n{5}", httpVersion,
                                                                                               responseCode,
                                                                                               responseStatus,
                                                                                               context.contextResponse.contentType,
                                                                                               jsonResponse.Length,
                                                                                               jsonResponse);
        }

        private void SendResponse(TcpClient client, Context context)
        {
            string headers = PrepareResponse(context);
            byte[] headersBuffer = Encoding.ASCII.GetBytes(headers);
            client.GetStream().Write(headersBuffer, 0, headersBuffer.Length);

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
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_COMPANY_PREFIX);
            }
        }
    }
}
