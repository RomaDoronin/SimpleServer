﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer.InternalObject
{
    public class Patient
    {
        public string patientId;
        public string firstname;
        public string secondname;
        public string petMedicalCardId;

        public Patient()
        {
            patientId = string.Empty;
            firstname = string.Empty;
            secondname = string.Empty;
            petMedicalCardId = string.Empty;
        }

        public Patient(Patient previousPatient)
        {
            patientId = previousPatient.patientId;
            firstname = previousPatient.firstname;
            secondname = previousPatient.secondname;
            petMedicalCardId = previousPatient.petMedicalCardId;
        }
    }
}
