using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.InternalObject
{
    class User
    {
        public string accountId;
        public string firstname;
        public string secondname;
        public string username;
        public string hashPassword;

        public User()
        {
            accountId = string.Empty;
            firstname = string.Empty;
            secondname = string.Empty;
            username = string.Empty;
            hashPassword = string.Empty;
        }

        public User(User previousUser)
        {
            accountId = previousUser.accountId;
            firstname = previousUser.firstname;
            secondname = previousUser.secondname;
            username = previousUser.username;
            hashPassword = previousUser.hashPassword;
        }
    }
}
