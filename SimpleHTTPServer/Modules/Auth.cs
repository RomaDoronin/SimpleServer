using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Modules
{
    public class Auth : AModule
    {
        // Validate
        // ---------------------------------------------------------
        public override string[] GetRequiredFieldNames(string httpReqName)
        {
            switch (httpReqName)
            {
                case "PUT": return new string[] { "username", "hash_password" };
            }

            return new string[] { };
        }

        public override void ValidatePUT(Context context)
        {
            // Проверка существует ли такой username
            ValidatePUTUsernameExist(context);
        }

        // ProcessRequest
        // ---------------------------------------------------------
        public override void Put(Context context)
        {
            DataBaseService.DatabaseReturn databaseReturn = context.dataBase.GetUserByUsername(context.contextRequest.reqData.data["username"]);

            if (databaseReturn.status != DataBaseService.DatabaseStatus.DB_OK)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                return;
            }

            var user = (InternalObject.User)databaseReturn.internalObject;

            if (user.hashPassword != context.contextRequest.reqData.data["hash_password"])
            {
                context.contextResponse.statusCode = Constants.StatusCode.UNAUTHORIZED;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.INCORRECT_PASSWORD);
                return;
            }

            context.contextResponse.respData.data.Add("account_id", user.accountId);
        }

        // Private method
        // ---------------------------------------------------------
        private void ValidatePUTUsernameExist(Context context)
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
                    return;
                }
            }

            context.contextResponse.statusCode = Constants.StatusCode.UNAUTHORIZED;
            context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.NO_SUCH_USERNAME);
        }
    }
}
