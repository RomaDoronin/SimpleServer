using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Constants
{
    enum StatusCode
    {
        OK = 200,
        CREATED = 201,
        ACCEPTED = 202,

        BAD_REQUEST = 400,
        UNAUTHORIZED = 401,
        NOT_FOUND = 404,
        METHOD_NOT_ALLOWED = 405,
        CONFLICT = 409,

        INTERNAL_SERVER_ERROR = 500
    }

    enum ErrorMessageKey
    {
        METHOD_NOT_ALLOWED,
        INCORRECT_COMPANY_PREFIX,
        REQUIRED_FIELD_MISSING,
        EMPTY_FIELD_VALUE_PRESENT,
        PROBLEM_WITH_ACCESS_DATABASE,
        USERNAME_ALREADY_EXIST
    }

    class ResponseStatusInfo
    {
        private static readonly Dictionary<ErrorMessageKey, string> errorMessageDict = new Dictionary<ErrorMessageKey, string>
        {
            {ErrorMessageKey.METHOD_NOT_ALLOWED, "Method not allowed"},
            {ErrorMessageKey.INCORRECT_COMPANY_PREFIX, "Incorrect company prefix"},
            {ErrorMessageKey.REQUIRED_FIELD_MISSING, "Required field is missing"},
            {ErrorMessageKey.EMPTY_FIELD_VALUE_PRESENT, "An empty field value is present"},
            {ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE, "The problem with the access database"},
            {ErrorMessageKey.USERNAME_ALREADY_EXIST, "A user with this username already exists"}
        };

        public static string GetErrorMessage(ErrorMessageKey key)
        {
            return errorMessageDict[key];
        }
    }
}
