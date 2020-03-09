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
        DB_OBJECT_NOT_FOUND,
        DB_UNKNOWN_ERROR
    }

    struct DatabaseReturn
    {
        public DatabaseStatus status;
        public Object internalObject;

        public DatabaseReturn(DatabaseStatus status, Object internalObject)
        {
            this.status = status;
            this.internalObject = internalObject;
        }
    }

    interface IDataBaseService
    {
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: null
        /// }
        /// </summary>
        DatabaseReturn CreateUser(InternalObject.User user);
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: null
        /// }
        /// </summary>
        DatabaseReturn ChangeUser(InternalObject.User user);
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: User
        /// }
        /// </summary>
        DatabaseReturn GetUser();
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: [User with one field "key"]
        /// }
        /// </summary>
        DatabaseReturn GetAllUser(string key);

        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: null
        /// }
        /// </summary>
        DatabaseReturn CreatePatient(InternalObject.Patient patient);
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: [Patient]
        /// }
        /// </summary>
        DatabaseReturn GetAllPatients();
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: Patient
        /// }
        /// </summary>
        DatabaseReturn GetPatient(InternalObject.Patient patient);
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: null
        /// }
        /// </summary>
        DatabaseReturn SetPetMedicalCardToPatient(string patientId, InternalObject.PetMedicalCard petMedicalCard);

        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: null
        /// }
        /// </summary>
        DatabaseReturn CreatePetMedicalCard(InternalObject.PetMedicalCard petMedicalCard);
        /// <summary>
        /// Return {
        /// status: DatabaseStatus,
        /// internalObject: PetMedicalCard
        /// }
        /// </summary>
        DatabaseReturn GetPetMedicalCard(InternalObject.PetMedicalCard petMedicalCard);
    }
}
