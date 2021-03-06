﻿using System;
using System.Collections.Generic;

namespace SimpleHTTPServer
{
    public class Context
    {
        private const string KEY_CONTENT_TYPE = "Content-Type:";
        private const string KEY_HOST = "Host:";
        private const string KEY_CONTENT_LENGTH = "Content-Length:";
        private const string REQ_DATA_START = "{";
        private const string REQ_DATA_END = "}";

        public ContextRequest contextRequest;
        public ContextResponse contextResponse;
        public DataBaseService.IDataBaseService dataBase;

        // Getters
        private void GetHttpReqName(ref string request)
        {
            contextRequest.httpReqName = StrManualLib.GetNextWordWithDelete(ref request);
        }

        private void GetURL(ref string request)
        {
            string urlString = StrManualLib.GetNextWordWithDelete(ref request);
            contextRequest.url = urlString.Split(Constants.CommonConstants.URL_SEPARATOR);
        }

        private void GetContentType(ref string request)
        {
            while (KEY_CONTENT_TYPE != StrManualLib.GetNextWordWithDelete(ref request)) { }
            contextRequest.contentType = StrManualLib.GetNextWordWithDelete(ref request);
        }

        private void GetHost(ref string request)
        {
            while (KEY_HOST != StrManualLib.GetNextWordWithDelete(ref request)) { }
            contextRequest.host = StrManualLib.GetNextWordWithDelete(ref request);
        }

        private void GetContentLength(ref string request)
        {
            while (KEY_CONTENT_LENGTH != StrManualLib.GetNextWordWithDelete(ref request)) { }
            contextRequest.contentLength = int.Parse(StrManualLib.GetNextWordWithDelete(ref request));
        }

        private void GetReqData(ref string request)
        {
            // Проверка на наличие контента
            if (contextRequest.contentLength == 0)
            {
                return;
            }

            // Удаление из строки requset всего лишнего до начала контента
            while (REQ_DATA_START != StrManualLib.GetNextWordWithDelete(ref request))
            {
                if (0 == request.Length)
                {
                    contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_JSON_FORMAT);
                    return;
                }
            }
            request = REQ_DATA_START + request;
            if (!StrManualLib.CheckStringForJsonFormat(request.Substring(0)))
            {
                contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
                contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_JSON_FORMAT);
                return;
            }

            contextRequest.reqData = new JSON(request);
        }

        // Checkers
        private void CheckHttpReqName()
        {
            switch (contextRequest.httpReqName)
            {
                case "GET":
                    break;
                case "POST":
                    break;
                case "PUT":
                    break;
                case "DELETE":
                    break;
                case "PATCH":
                    break;
                default:
                    contextResponse.statusCode = Constants.StatusCode.METHOD_NOT_ALLOWED;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.METHOD_NOT_ALLOWED);
                    break;
            }
        }

        private void CheckURL()
        {
            // Проверка на префикс компании
            if (contextRequest.url[(int)Constants.UrlPositionNumber.COMPANY_PREFIX] != Constants.CommonConstants.COMPANY_PREFIX)
            {
                contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
                contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_COMPANY_PREFIX);
                return;
            }

            // Проверка на наличие Account Id в случае запроса с залогиненого аккаунта
            if (contextRequest.url.Length > (int)Constants.UrlPositionNumberWithAccount.MODULE_NAME && contextRequest.url[(int)Constants.UrlPositionNumberWithAccount.ACCOUNTS] == Constants.CommonConstants.ACCOUNT_PREFIX)
            {
                // Проверка что модуль является тем, что выполняется из под аккаунта
                string moduleName = contextRequest.url[(int)Constants.UrlPositionNumberWithAccount.MODULE_NAME];
                if (moduleName == Constants.ModuleList.MODULE_AUTH || moduleName == Constants.ModuleList.MODULE_REGIST)
                {
                    contextResponse.statusCode = Constants.StatusCode.NOT_FOUND;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_URL);
                    return;
                }

                contextRequest.isAccountRequest = true;

                // Проверяем существование пользователя под данным accountId
                string accountId = contextRequest.url[(int)Constants.UrlPositionNumberWithAccount.ACCOUNT_ID];
                DataBaseService.DatabaseReturn databaseReturn = dataBase.GetUserByAccountId(accountId);

                if (databaseReturn.status == DataBaseService.DatabaseStatus.DB_UNKNOWN_ERROR)
                {
                    contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                    return;
                }
                else if (databaseReturn.status == DataBaseService.DatabaseStatus.DB_OBJECT_NOT_FOUND)
                {
                    contextResponse.statusCode = Constants.StatusCode.NOT_FOUND;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.NON_EXISTENT_ACCOUNT_ID);
                    return;
                }
            }
            else
            {
                // Проверка на размер секций url
                if (contextRequest.url.Length <= (int)Constants.UrlPositionNumber.MODULE_NAME)
                {
                    contextResponse.statusCode = Constants.StatusCode.NOT_FOUND;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_URL);
                    return;
                }

                // Проверка что модуль является тем, что выполняется из под аккаунта
                string moduleName = contextRequest.url[(int)Constants.UrlPositionNumber.MODULE_NAME];
                if (moduleName != Constants.ModuleList.MODULE_AUTH && moduleName != Constants.ModuleList.MODULE_REGIST)
                {
                    contextResponse.statusCode = Constants.StatusCode.NOT_FOUND;
                    contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_URL);
                    return;
                }
            }
        }

        private void CheckContentType()
        {
            if (contextRequest.contentType != Constants.CommonConstants.CONTENT_TYPE_JSON)
            {
                contextResponse.statusCode = Constants.StatusCode.UNSUPPORTED_MEDIA_TYPE;
                contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.SUPPORTED_ONLY_JSON);
            }
        }

        public Context(string request)
        {
            contextRequest = new ContextRequest();
            contextResponse = new ContextResponse();
            dataBase = DataBaseService.RunTimeDataBaseMock.Inst();

            GetHttpReqName(ref request);
            CheckHttpReqName();

            GetURL(ref request);
            CheckURL();

            GetContentType(ref request);
            CheckContentType();

            GetHost(ref request);

            GetContentLength(ref request);

            GetReqData(ref request);
        }
    }
}
