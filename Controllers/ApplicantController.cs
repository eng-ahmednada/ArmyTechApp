using ArmyTechApp.Models;
using ArmyTechApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArmyTechApplicant.Controllers
{
    public class ApplicantController : Controller
    {
        static ArmyTechApplicantEntities db = new ArmyTechApplicantEntities();
        // GET: Applicant
        public ActionResult Index()
        {
            return View();
        }

        //GET : All CertificationSpecifics For CertificationType
        public ActionResult getCertificationSpecifics(int id)
        {
            ViewBag.CertificationSpecificId = new SelectList(db.CertificationSpecifics.Where(a => a.CertificationTypeID == id), "ID", "Name");
            return PartialView("~/Views/Applicant/_getCertificationSpecifics.cshtml");
        }

        public ActionResult createApplication()
        {
            ViewBag.ApplicationsFieldId = db.ApplicationsFields.ToList();
            ViewBag.GenderId = new SelectList(db.Genders, "ID", "Name");
            ViewBag.CertificationTypeId = new SelectList(db.CertificationTypes, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createApplication(ApplicantVM applicant)
        {
            if (ModelState.IsValid)
            {
                //check if the user chooses application field or No
                if (applicant.ApplicationsFieldId.Count() > 0)
                {
                    //Save in Applicant table
                    db.Applicants.Add(applicant.app);
                    if (db.SaveChanges() > 0)
                    {
                        foreach (var item in applicant.ApplicationsFieldId)
                        {
                            //Save in ApplicantApplicationsFields table for number of application field user chooses
                            db.ApplicantApplicationsFields.Add(new ApplicantApplicationsField { ApplicantId = applicant.app.ID, ApplicationsFieldId = item });
                        }
                        applicant.appQualif.ApplicantId = applicant.app.ID;
                        //Save in ApplicantQualifications table 
                        db.ApplicantQualifications.Add(applicant.appQualif);
                        db.SaveChanges();
                        return PartialView("~/Views/Applicant/_Congratulations.cshtml");
                    }

                }

            }
            TempData["error"] = "Don't Save Try Again";
            return RedirectToAction(nameof(createApplication));
        }
    }
}