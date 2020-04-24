using SimpleHTTPServer.DataBaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.Modules
{
    public class Patient : AModule
    {
        // Validate
        // ---------------------------------------------------------
        public override string[] GetRequiredFieldNames(string httpReqName, string[] url)
        {

            switch (httpReqName)
            {
                case "GET": return new string[] { };
                case "POST": return new string[] { "firstname", "secondname" };
                case "PATCH": return new string[] { "pet_medical_card_id" };
            }

            return new string[] { };
        }

        public override void ValidatePOST(Context context) { }

        public override void ValidateGET(Context context) { }

        // ProcessRequest
        // ---------------------------------------------------------
        public override void Post(Context context)
        {
            var patient = new InternalObject.Patient
            {
                patientId = StrManualLib.GenerateRandomString(24),
                firstname = context.contextRequest.reqData.data["firstname"].ToString(),
                secondname = context.contextRequest.reqData.data["secondname"].ToString()
            };

            DataBaseService.DatabaseReturn databaseReturn = context.dataBase.CreatePatient(patient);
            if (databaseReturn.status == DataBaseService.DatabaseStatus.DB_UNKNOWN_ERROR)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                return;
            }

            context.contextResponse.statusCode = Constants.StatusCode.CREATED;
        }

        public override void Get(Context context)
        {
            if (context.contextRequest.url.Length > (int)Constants.UrlPositionNumberWithAccount.MODULE_OBJECT_ID)
            {
                Get(context, context.contextRequest.url[(int)Constants.UrlPositionNumberWithAccount.MODULE_OBJECT_ID]);
                return;
            }

            var databaseReturn = context.dataBase.GetAllPatients();
            if (databaseReturn.status == DataBaseService.DatabaseStatus.DB_UNKNOWN_ERROR)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                return;
            }
            var patientList = (List<InternalObject.Patient>)databaseReturn.internalObject;

            List<JSON> patientJsonList = new List<JSON>();
            foreach(var patient in patientList)
            {
                patientJsonList.Add(MapPatientToJson(patient));
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["patients"] = patientJsonList;

            context.contextResponse.respData = new JSON(data);
        }

        private void Get(Context context, string patientId)
        {
            var databaseReturn = context.dataBase.GetPatient(patientId);
            if (databaseReturn.status == DatabaseStatus.DB_UNKNOWN_ERROR)
            {
                context.contextResponse.statusCode = Constants.StatusCode.INTERNAL_SERVER_ERROR;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.PROBLEM_WITH_ACCESS_DATABASE);
                return;
            }
            else if (databaseReturn.status == DatabaseStatus.DB_OBJECT_NOT_FOUND)
            {
                context.contextResponse.statusCode = Constants.StatusCode.NOT_FOUND;
                context.contextResponse.message = Constants.ResponseStatusInfo.GetErrorMessage(Constants.ErrorMessageKey.OBJECT_NOT_FOUND);
                return;
            }

            var patient = (InternalObject.Patient)databaseReturn.internalObject;
            JSON jsonPatient = MapPatientToJson(patient);
            jsonPatient.data.Remove("patient_id");
            context.contextResponse.respData = jsonPatient;
        }

        // Private method
        // ---------------------------------------------------------
        private JSON MapPatientToJson(InternalObject.Patient patient)
        {
            Dictionary<string, object> jsonPatientData = new Dictionary<string, object>();
            jsonPatientData["patient_id"] = patient.patientId;
            jsonPatientData["firstname"] = patient.firstname;
            jsonPatientData["secondname"] = patient.secondname;
            jsonPatientData["pet_id"] = patient.petMedicalCardId;

            return new JSON(jsonPatientData);
        }
    }
}
