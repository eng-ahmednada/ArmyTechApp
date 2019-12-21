using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArmyTechApp.Models.ViewModels
{
    public class ApplicantVM
    {
        public Applicant app { get; set; }
        public ApplicantQualification appQualif { get; set; }
        public List<int> ApplicationsFieldId { get; set; }
        public ApplicantVM()
        {
            ApplicationsFieldId = new List<int>();
        }
    }
}