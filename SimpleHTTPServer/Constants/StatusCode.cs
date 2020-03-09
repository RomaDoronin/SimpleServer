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
}
