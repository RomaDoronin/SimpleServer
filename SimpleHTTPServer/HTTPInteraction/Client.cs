using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using SimpleHTTPServer.Modules;

namespace SimpleHTTPServer.HTTPInteraction
{
    public class Client
    {
        public static void Run(TcpClient client)
        {
            string request = ReadRequestFromClient(client);
            Console.WriteLine(request);

            Context context = HandleRequest(request);
            string headers = PrepareResponse(context);
            SendResponse(client, headers);
        }

        private static string ReadRequestFromClient(TcpClient client)
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

            return request;
        }

        public static Context HandleRequest(string request)
        {
            // Заполняем контекст из запроса
            Context context = new Context(request);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { return context; }

            // Получаем используемый модуль
            IModule module = GetModule(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { return context; }

            // Запускаем валидацию запроса в модуле
            module.Validate(context);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { return context; }

            // Запускаем обработку запроса
            module.ProcessRequest(context);

            return context;
        }

        /// <summary>
        /// Получение модуля для обработки запроса
        /// </summary>
        private static IModule GetModule(Context context)
        {
            int moduleIndex = context.contextRequest.isAccountRequest ? (int)Constants.UrlPositionNumberWithAccount.MODULE_NAME : (int)Constants.UrlPositionNumber.MODULE_NAME;
            string moduleName = context.contextRequest.url[moduleIndex];
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
        private static void SendResponse(TcpClient client, string headers)
        {
            byte[] headersBuffer = Encoding.ASCII.GetBytes(headers);
            client.GetStream().Write(headersBuffer, 0, headersBuffer.Length);

            client.Close();
        }

        /// <summary>
        /// Подготовка ответа
        /// </summary>
        public static string PrepareResponse(Context context)
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

            Dictionary<string, object> jsonDict = new Dictionary<string, object>();
            if (context.contextResponse.respData.data.Count > 0)
            {
                jsonDict["data"] = context.contextResponse.respData;
            }
            jsonDict["message"] = messageValue;
            string jsonResponse = new JSON(jsonDict).ToString();

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
