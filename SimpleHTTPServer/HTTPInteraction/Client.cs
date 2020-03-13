using System;
using System.Net.Sockets;
using System.Text;
using SimpleHTTPServer.Modules;

namespace SimpleHTTPServer.HTTPInteraction
{
    class Client
    {
        public Client(TcpClient client)
        {
            // Объявим строку, в которой будет хранится запрос клиента
            string request = string.Empty;
            // Буфер для хранения принятых от клиента данных
            const uint BUFFER_SIZE = 1024;
            byte[] buffer = new byte[BUFFER_SIZE];
            // Переменная для хранения количества байт, принятых от клиента
            int Count;

            // Читаем из потока клиента
            Count = client.GetStream().Read(buffer, 0, buffer.Length);
            // Преобразуем эти данные в строку и добавим ее к переменной Request
            request += Encoding.ASCII.GetString(buffer, 0, Count);
            Console.WriteLine(request);
            // Заполняем контекст из запроса
            Context context = new Context(request);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }

            // Получаем используемый модуль
            IModule module = GetModule(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }

            // Запускаем валидацию запроса в модуле
            module.Validate(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { SendResponse(client, context); return; }
            // Запускаем обработку запроса
            module.ProcessRequest(context);
            SendResponse(client, context);
        }

        /// <summary>
        /// Получение модуля для обработки запроса
        /// </summary>
        private IModule GetModule(Context context)
        {
            string[] words = context.contextRequest.url.Split(new char[] { Constants.CommonConstants.URL_SEPARATOR });
            int moduleIndex = context.contextRequest.isAccountRequest ? (int)Constants.UrlPositionNumberWithAccount.MODULE_NAME : (int)Constants.UrlPositionNumber.MODULE_NAME;
            string moduleName = words[moduleIndex];
            IModule module = Constants.ModuleList.GetModuleByModuleName(moduleName);
            if (module == null)
            {
                context.contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_MODULE_NAME);
            }

            return module;
        }

        /// <summary>
        /// Отправка ответа
        /// </summary>
        private void SendResponse(TcpClient client, Context context)
        {
            string headers = PrepareResponse(context);
            byte[] headersBuffer = Encoding.ASCII.GetBytes(headers);
            client.GetStream().Write(headersBuffer, 0, headersBuffer.Length);

            client.Close();
        }

        /// <summary>
        /// Подготовка ответа
        /// </summary>
        private string PrepareResponse(Context context)
        {
            string messageValue = Constants.CommonConstants.DEFAULT_RESPONSE_MSG;
            if (context.contextResponse.statusCode != Constants.StatusCode.OK)
            {
                messageValue = context.contextResponse.message;
            }
            if (context.contextResponse.statusCode == Constants.StatusCode.CREATED)
            {
                messageValue = "create " + Constants.CommonConstants.DEFAULT_RESPONSE_MSG;
            }

            string jsonResponse = "{\n";
            if (context.contextResponse.respData.data.Count > 0)
            {
                jsonResponse += "    \"data\": {\n";
                foreach (var key in context.contextResponse.respData.data.Keys)
                {
                    jsonResponse += string.Format("        \"{0}\": \"{1}\"\n", key, context.contextResponse.respData.data[key]);
                }
                jsonResponse += "    }\n";
            }
            jsonResponse += string.Format("    \"message\": \"{0}\"\n", messageValue) + "}";

            string responseCode = context.contextResponse.statusCode.ToString("d");
            string responseStatus = StrManualLib.ConstFormatToStringFormat(context.contextResponse.statusCode.ToString("g"));

            return string.Format("{0} {1} {2}\nContent-Type: {3}\nContent-Length: {4}\n\n{5}", Constants.CommonConstants.HTTP_VERSION,
                                                                                               responseCode,
                                                                                               responseStatus,
                                                                                               Constants.CommonConstants.CONTENT_TYPE_JSON,
                                                                                               jsonResponse.Length,
                                                                                               jsonResponse);
        }
    }
}
