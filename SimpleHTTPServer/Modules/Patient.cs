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
        public override string[] GetRequiredFieldNames(string httpReqName)
        {
            switch (httpReqName)
            {
                case "GET": return new string[] {};
                case "POST": return new string[] { "firstname", "secondname" };
                case "PATCH": return new string[] { "pet_medical_card_id" };
            }

            return new string[] { };
        }

        public override void ValidatePOST(Context context) { }

        public override void ValidateGET(Context context)
        {

        }

        private  void ValidateGET(Context context, string patientId)
        {

        }

        // ProcessRequest
        // ---------------------------------------------------------
        public override void Post(Context context)
        {
            var patient = new InternalObject.Patient
            {
                patientId = StrManualLib.GenerateRandomString(24),
                firstname = context.contextRequest.reqData.data["firstname"],
                secondname = context.contextRequest.reqData.data["secondname"]
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

        // Private method
        // ---------------------------------------------------------
    }
}
