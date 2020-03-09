﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Modules
{
    class Regist : IModule
    {
        public void Validate(Context context)
        {
            switch (context.contextRequest.httpReqName)
            {
                case "POST":
                    ValidatePOST(context);
                    break;
                default:
                    context.contextResponse.statusCode = Constants.StatusCode.METHOD_NOT_ALLOWED;
                    break;
            }
        }

        private void ValidatePOST(Context context)
        {
            // Проверка что все поля есть и заполнены
            ValidatePOSTFieldExistAndNotEmpty(context);

            // Проверка на неповторяющийся username
            ValidatePOSTNonRepeatingUsername(context);
        }

        private void ValidatePOSTFieldExistAndNotEmpty(Context context)
        {
            string[] fieldNames = { "firstname", "secondname", "username", "hash_password" };

            foreach (string fieldName in fieldNames)
            {
                ValidatePOSTFieldExistAndNotEmpty(context, fieldName);
            }
        }

        private void ValidatePOSTFieldExistAndNotEmpty(Context context, string fiendName)
        {
            if (context.contextRequest.reqData.data.ContainsKey(fiendName))
            {
                ValidatePOSTFieldExistAndNotEmpty(context, context.contextRequest.reqData.data[fiendName].Length);
            }
            else
            {
                context.contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
            }
        }

        private void ValidatePOSTFieldExistAndNotEmpty(Context context, int valueLength)
        {
            if (valueLength == 0)
            {
                context.contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
            }
        }

        private void ValidatePOSTNonRepeatingUsername(Context context)
        {
            DataBaseService.DatabaseReturn databaseReturn = context.dataBase.GetAllUser("username");

            if (databaseReturn.status != DataBaseService.DatabaseStatus.DB_OK)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                return;
            }

            List<InternalObject.User> userList = (List<InternalObject.User>)databaseReturn.internalObject;
            foreach (InternalObject.User user in userList)
            {
                if (user.username == context.contextRequest.reqData.data["username"])
                {
                    context.contextResponse.statusCode = Constants.StatusCode.CONFLICT;
                    return;
                }
            }
        }

        // ---------------------------------------------------------

        public void ProcessRequest(Context context)
        {
            switch (context.contextRequest.httpReqName)
            {
                case "POST":
                    Post(context);
                    break;
            }
        }

        private void Post(Context context)
        {
            InternalObject.User user = new InternalObject.User();
            JSON reqData = context.contextRequest.reqData;

            user.accountId = StrManualLib.GenerateRandomString(16);
            user.firstname = reqData.data["firstname"];
            user.secondname = reqData.data["secondname"];
            user.hashPassword = reqData.data["hash_password"];
            user.username = reqData.data["username"];

            if (context.dataBase.CreateUser(user).status != DataBaseService.DatabaseStatus.DB_OK)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                return;
            }
        }
    }
}
