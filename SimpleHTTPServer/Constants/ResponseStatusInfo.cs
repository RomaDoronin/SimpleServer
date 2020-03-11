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
        UNSUPPORTED_MEDIA_TYPE = 415,

        INTERNAL_SERVER_ERROR = 500
    }

    enum ErrorMessageKey
    {
        METHOD_NOT_ALLOWED,
        INCORRECT_COMPANY_PREFIX,
        INCORRECT_MODULE_NAME,
        REQUIRED_FIELD_MISSING,
        EMPTY_FIELD_VALUE_PRESENT,
        PROBLEM_WITH_ACCESS_DATABASE,
        USERNAME_ALREADY_EXIST,
        SUPPORTED_ONLY_JSON,
        INCORRECT_JSON_FORMAT,
        IDENTICAL_KEYS_ON_SAME_LEVEL,
        NO_SUCH_USERNAME,
        INCORRECT_PASSWORD
    }

    class ResponseStatusInfo
    {
        private static readonly Dictionary<ErrorMessageKey, string> errorMessageDict = new Dictionary<ErrorMessageKey, string>
        {
            {ErrorMessageKey.METHOD_NOT_ALLOWED, "Method not allowed"},
            {ErrorMessageKey.INCORRECT_COMPANY_PREFIX, "Incorrect company prefix"},
            {ErrorMessageKey.INCORRECT_MODULE_NAME, "Incorrect module name"},
            {ErrorMessageKey.REQUIRED_FIELD_MISSING, "Required field is missing"},
            {ErrorMessageKey.EMPTY_FIELD_VALUE_PRESENT, "An empty field value is present"},
            {ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE, "The problem with the access database"},
            {ErrorMessageKey.USERNAME_ALREADY_EXIST, "A user with this username already exists"},
            {ErrorMessageKey.SUPPORTED_ONLY_JSON, "The server only supports type: application/json"},
            {ErrorMessageKey.INCORRECT_JSON_FORMAT, "Incorrect JSON format"},
            {ErrorMessageKey.IDENTICAL_KEYS_ON_SAME_LEVEL, "Two identical keys on the same level"},
            {ErrorMessageKey.NO_SUCH_USERNAME, "There is no such username"},
            {ErrorMessageKey.INCORRECT_PASSWORD, "The password was entered incorrectly"}
        };

        public static string GetErrorMessage(ErrorMessageKey key)
        {
            return errorMessageDict[key];
        }
    }
}
