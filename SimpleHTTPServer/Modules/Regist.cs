using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Modules
{
    class Regist : AModule
    {
        // Validate
        // ---------------------------------------------------------
        public override string[] GetRequiredFieldNames(string httpReqName)
        {
            switch (httpReqName)
            {
                case "POST": return new string[] { "firstname", "secondname", "username", "hash_password" };
            }

            return new string[] { };
        }

        public override void ValidatePOST(Context context)
        {
            // Проверка на неповторяющийся username
            ValidatePOSTNonRepeatingUsername(context);
        }

        // ProcessRequest
        // ---------------------------------------------------------
        public override void Post(Context context)
        {
            InternalObject.User user = new InternalObject.User();
            JSON reqData = context.contextRequest.reqData;

            user.accountId = StrManualLib.GenerateRandomString(24);
            user.firstname = reqData.data["firstname"];
            user.secondname = reqData.data["secondname"];
            user.hashPassword = reqData.data["hash_password"];
            user.username = reqData.data["username"];

            if (context.dataBase.CreateUser(user).status != DataBaseService.DatabaseStatus.DB_OK)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                return;
            }

            context.contextResponse.statusCode = Constants.StatusCode.CREATED;
        }

        // Private method
        // ---------------------------------------------------------
        private void ValidatePOSTNonRepeatingUsername(Context context)
        {
            DataBaseService.DatabaseReturn databaseReturn = context.dataBase.GetAllUser();

            if (databaseReturn.status != DataBaseService.DatabaseStatus.DB_OK)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                return;
            }

            var userList = (List<InternalObject.User>)databaseReturn.internalObject;
            foreach (var user in userList)
            {
                if (user.username == context.contextRequest.reqData.data["username"])
                {
                    context.contextResponse.statusCode = Constants.StatusCode.CONFLICT;
                    context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.USERNAME_ALREADY_EXIST);
                    return;
                }
            }
        }
    }
}
