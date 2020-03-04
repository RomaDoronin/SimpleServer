using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.DataBaseService
{
    enum DatabaseStatus
    {
        DB_OK,
        DB_USER_NOT_ALLOWED
    }

    interface IDataBaseService
    {
        DatabaseStatus CreateUser(InternalObject.User user);
        DatabaseStatus ChangeUser(InternalObject.User user);
        DatabaseStatus AddPetMedicalCardToUser(InternalObject.PetMedicalCard petMedicalCard, InternalObject.User user);
    }
}
