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
        private List<User> m_userList;
        private List<Patient> m_patientList;

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
            m_userList = new List<User>();
            m_patientList = new List<Patient>();
        }

        // IDataBaseService implementation
        // ---------------------------------------------------------

        public DatabaseReturn ChangeUser(User user)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn CreatePatient(Patient patient)
        {
            m_patientList.Add(patient);
            return new DatabaseReturn(DatabaseStatus.DB_OK, null);
        }

        public DatabaseReturn CreatePetMedicalCard(PetMedicalCard petMedicalCard)
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn CreateUser(User user)
        {
            m_userList.Add(new User(user));
            return new DatabaseReturn(DatabaseStatus.DB_OK, null);
        }

        public DatabaseReturn GetAllPatients()
        {
            throw new NotImplementedException();
        }

        public DatabaseReturn GetAllUser()
        {
            return new DatabaseReturn(DatabaseStatus.DB_OK, new List<InternalObject.User>(m_userList));
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
            foreach (var user in m_userList)
            {
                if (0 == string.Compare(user.username, username))
                {
                    return new DatabaseReturn(DatabaseStatus.DB_OK, new InternalObject.User(user));
                }
            }

            return new DatabaseReturn(DatabaseStatus.DB_OBJECT_NOT_FOUND, null);
        }

        public DatabaseReturn GetUserByAccountId(string accountId)
        {
            foreach (var user in m_userList)
            {
                if (0 == string.Compare(user.accountId, accountId))
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
