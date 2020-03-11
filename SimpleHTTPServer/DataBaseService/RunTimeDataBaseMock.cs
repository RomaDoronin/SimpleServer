using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleHTTPServer.InternalObject;

namespace SimpleHTTPServer.DataBaseService
{
    class RunTimeDataBaseMock : IDataBaseService
    {
        private List<InternalObject.User> m_users;

        private static RunTimeDataBaseMock instance;

        public static RunTimeDataBaseMock Inst()
        {
            if (instance == null)
            {
                instance = new RunTimeDataBaseMock();
            }

            return instance;
        }

        private RunTimeDataBaseMock()
        {
            m_users = new List<User>();
        }

        // IDataBaseService implementation

        public DatabaseReturn ChangeUser(User user)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn CreatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn CreatePetMedicalCard(PetMedicalCard petMedicalCard)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn CreateUser(User user)
        {
            m_users.Add(new User(user));
            return new DatabaseReturn(DatabaseStatus.DB_OK, null);
        }

        public DatabaseReturn GetAllPatients()
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn GetAllUser()
        {
            return new DatabaseReturn(DatabaseStatus.DB_OK, new List<InternalObject.User>(m_users));
        }

        public DatabaseReturn GetPatient(Patient patient)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn GetPetMedicalCard(PetMedicalCard petMedicalCard)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn GetUserByUsername(string username)
        {
            foreach (var user in m_users)
            {
                if (0 == string.Compare(user.username, username))
                {
                    return new DatabaseReturn(DatabaseStatus.DB_OK, new InternalObject.User(user));
                }
            }

            return new DatabaseReturn(DatabaseStatus.DB_OBJECT_NOT_FOUND, null);
        }

        public DatabaseReturn SetPetMedicalCardToPatient(string patientId, PetMedicalCard petMedicalCard)
        {
            throw new NotImplementedException();
        }
    }
}
