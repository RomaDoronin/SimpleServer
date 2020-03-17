using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Modules
{
    public abstract class AModule : IModule
    {
        // Validate
        // ---------------------------------------------------------
        public void Validate(Context context)
        {
            // Проверка на обязательные поля
            string[] requiredFieldNames = GetRequiredFieldNames(context.contextRequest.httpReqName);
            ValidateRequiredFieldExistAndNotEmpty(context, requiredFieldNames);
            if (context.contextResponse.statusCode != Constants.StatusCode.OK) { return; }

            // Персональная проверка
            switch (context.contextRequest.httpReqName)
            {
                case "GET":
                    ValidateGET(context);
                    break;
                case "POST":
                    ValidatePOST(context);
                    break;
                case "PUT":
                    ValidatePUT(context);
                    break;
                case "DELETE":
                    ValidateDELETE(context);
                    break;
                case "PATCH":
                    ValidatePATCH(context);
                    break;
                default:
                    SetMethodNotAllowed(context);
                    break;
            }
        }

        // Validate virtual methods
        public virtual string[] GetRequiredFieldNames(string httpReqName)
        {
            return new string[] { };
        }

        public virtual void ValidateGET(Context context) => SetMethodNotAllowed(context);
        public virtual void ValidatePOST(Context context) => SetMethodNotAllowed(context);
        public virtual void ValidatePUT(Context context) => SetMethodNotAllowed(context);
        public virtual void ValidateDELETE(Context context) => SetMethodNotAllowed(context);
        public virtual void ValidatePATCH(Context context) => SetMethodNotAllowed(context);

        // ProcessRequest
        // ---------------------------------------------------------
        public void ProcessRequest(Context context)
        {
            switch (context.contextRequest.httpReqName)
            {
                case "GET":
                    Get(context);
                    break;
                case "POST":
                    Post(context);
                    break;
                case "PUT":
                    Put(context);
                    break;
                case "DELETE":
                    Delete(context);
                    break;
                case "PATCH":
                    Patch(context);
                    break;
            }
        }

        // ProcessRequest virtual methods
        public virtual void Get(Context context) { }
        public virtual void Post(Context context) { }
        public virtual void Put(Context context) { }
        public virtual void Delete(Context context) { }
        public virtual void Patch(Context context) { }

        // Private methods
        // ---------------------------------------------------------
        private void SetMethodNotAllowed(Context context)
        {
            context.contextResponse.statusCode = Constants.StatusCode.METHOD_NOT_ALLOWED;
            context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.METHOD_NOT_ALLOWED);
        }

        private void ValidateRequiredFieldExistAndNotEmpty(Context context, string[] requiredFieldNames)
        {
            foreach (var fieldName in requiredFieldNames)
            {
                if (context.contextRequest.reqData.data.ContainsKey(fieldName))
                {
                    if (context.contextRequest.reqData.data.Count == 0)
                    {
                        context.contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
                        context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.EMPTY_FIELD_VALUE_PRESENT);
                        return;
                    }
                }
                else
                {
                    context.contextResponse.statusCode = Constants.StatusCode.BAD_REQUEST;
                    context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.REQUIRED_FIELD_MISSING);
                    return;
                }
            }
        }
    }
}
