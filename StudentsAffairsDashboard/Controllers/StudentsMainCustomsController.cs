using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentsAffairsDashboard.Models;

namespace StudentsAffairsDashboard.Controllers
{
    public class StudentsMainCustomsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: StudentsMainCustoms
        public ActionResult Index()
        {
            return View(db.StudentsMainCustoms.ToList());
        }

        // GET: StudentsMainCustoms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsMainCustom studentsMainCustom = db.StudentsMainCustoms.Find(id);
            if (studentsMainCustom == null)
            {
                return HttpNotFound();
            }
            return View(studentsMainCustom);
        }

        // GET: StudentsMainCustoms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentsMainCustoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StdCodeCustom,StdArabicFristName,StdArabicMiddleName,StdArabicLastName,StdArabicFamilyName,StdEnglishFristName,StdEnglishMiddleName,StdEnglishLastName,StdEnglishFamilyName,StdMotherArabicName,StdMotherEnglishName,StdFatherMobilePhone,StdMotherMobilePhone,StdFatherEmail,StdMotherEmail,StdFatherNationality,StdMotherNationality,StdFatherSpokenLanguage,StdMotherSpokenLanguage,StdFatherJob,StdMotherJob,StdFatherQualification,StdMotherQualification,StdStudentsAffairs1,StdStudentsAffairs2,StdBirthCode,StdAddressGov,StdEmergencyContact,StdEmergencyPhone,StdBOD,StdBirthPlace,StdGender,StdReligion,StdFatherNID,StdMotherNID,StdCity,StdAddress,StdNID,StdSchoolID,StdClassID,StdNationality,StdStatus,StdJoinYear,StdStaffSon,StdLegalGuardianship,StdParentsSeparated")] StudentsMainCustom studentsMainCustom)
        {
            if (ModelState.IsValid)
            {
                db.StudentsMainCustoms.Add(studentsMainCustom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(studentsMainCustom);
        }

        // GET: StudentsMainCustoms/Edit/5
        public ActionResult Edit()
        {          
            return View(db.StudentsMainCustoms.ToList());
        }

        // POST: StudentsMainCustoms/EditCustom
        [HttpPost]
        public ActionResult EditCustom(string searchText)
        {            
            string[] words = searchText.Split('&');           

            StudentsMainCustom SMC_Show = db.StudentsMainCustoms.Find(1);
            StudentsMainCustom SMC_Requ = db.StudentsMainCustoms.Find(2);
            List<string> SMC_Sh = new List<string>();
            List<string> SMC_Re = new List<string>();

            for (int i = 0; i < words.Length; i+=2)
            {
                string[] wordshow = words[i].Split('=');
                System.Diagnostics.Debug.WriteLine(wordshow[0] + "=>" + wordshow[1]);
                SMC_Sh.Add(wordshow[1]);
            }
            System.Diagnostics.Debug.WriteLine("==============================================>");
            for (int i = 1; i < words.Length; i+=2)
            {
                string[] wordRequ = words[i].Split('=');
                System.Diagnostics.Debug.WriteLine(wordRequ[0] + "===>" + wordRequ[1]);
                SMC_Re.Add(wordRequ[1]);
            }
            
            SMC_Show.StdArabicFristName= Boolean.Parse(SMC_Sh[0]);
            SMC_Show.StdArabicMiddleName= Boolean.Parse(SMC_Sh[1]);
            SMC_Show.StdArabicLastName= Boolean.Parse(SMC_Sh[2]);
            SMC_Show.StdArabicFamilyName= Boolean.Parse(SMC_Sh[3]);
            SMC_Show.StdEnglishFristName= Boolean.Parse(SMC_Sh[4]);
            SMC_Show.StdEnglishMiddleName= Boolean.Parse(SMC_Sh[5]);
            SMC_Show.StdEnglishLastName= Boolean.Parse(SMC_Sh[6]);
            SMC_Show.StdEnglishFamilyName= Boolean.Parse(SMC_Sh[7]);
            SMC_Show.StdMotherArabicName= Boolean.Parse(SMC_Sh[8]);
            SMC_Show.StdMotherEnglishName= Boolean.Parse(SMC_Sh[9]);
            SMC_Show.StdFatherMobilePhone= Boolean.Parse(SMC_Sh[10]);
            SMC_Show.StdMotherMobilePhone= Boolean.Parse(SMC_Sh[11]);
            SMC_Show.StdFatherEmail= Boolean.Parse(SMC_Sh[12]);
            SMC_Show.StdMotherEmail= Boolean.Parse(SMC_Sh[13]);
            SMC_Show.StdFatherNationality= Boolean.Parse(SMC_Sh[14]);
            SMC_Show.StdMotherNationality= Boolean.Parse(SMC_Sh[15]);
            SMC_Show.StdFatherSpokenLanguage= Boolean.Parse(SMC_Sh[16]);
            SMC_Show.StdMotherSpokenLanguage= Boolean.Parse(SMC_Sh[17]);
            SMC_Show.StdFatherJob= Boolean.Parse(SMC_Sh[18]);
            SMC_Show.StdMotherJob= Boolean.Parse(SMC_Sh[19]);
            SMC_Show.StdFatherQualification= Boolean.Parse(SMC_Sh[20]);
            SMC_Show.StdMotherQualification= Boolean.Parse(SMC_Sh[21]);
            SMC_Show.StdStudentsAffairs1= Boolean.Parse(SMC_Sh[22]);
            SMC_Show.StdStudentsAffairs2= Boolean.Parse(SMC_Sh[23]);
            SMC_Show.StdBirthCode= Boolean.Parse(SMC_Sh[24]);
            SMC_Show.StdAddressGov= Boolean.Parse(SMC_Sh[25]);
            SMC_Show.StdEmergencyContact= Boolean.Parse(SMC_Sh[26]);
            SMC_Show.StdEmergencyPhone= Boolean.Parse(SMC_Sh[27]);
            SMC_Show.StdBOD= Boolean.Parse(SMC_Sh[28]);
            SMC_Show.StdBirthPlace= Boolean.Parse(SMC_Sh[29]);
            SMC_Show.StdGender= Boolean.Parse(SMC_Sh[30]);
            SMC_Show.StdReligion= Boolean.Parse(SMC_Sh[31]);
            SMC_Show.StdFatherNID= Boolean.Parse(SMC_Sh[32]);
            SMC_Show.StdMotherNID= Boolean.Parse(SMC_Sh[33]);
            SMC_Show.StdCity= Boolean.Parse(SMC_Sh[34]);
            SMC_Show.StdAddress= Boolean.Parse(SMC_Sh[35]);
            SMC_Show.StdNID= Boolean.Parse(SMC_Sh[36]);
            SMC_Show.StdSchoolID= Boolean.Parse(SMC_Sh[37]);
            SMC_Show.StdClassID= Boolean.Parse(SMC_Sh[38]);
            SMC_Show.StdNationality= Boolean.Parse(SMC_Sh[39]);
            SMC_Show.StdStatus= Boolean.Parse(SMC_Sh[40]);
            SMC_Show.StdJoinYear= Boolean.Parse(SMC_Sh[41]);
            SMC_Show.StdStaffSon= Boolean.Parse(SMC_Sh[42]);
            SMC_Show.StdLegalGuardianship= Boolean.Parse(SMC_Sh[43]);
            SMC_Show.StdParentsSeparated= Boolean.Parse(SMC_Sh[44]);

            SMC_Requ.StdArabicFristName = Boolean.Parse(SMC_Re[0]);
            SMC_Requ.StdArabicMiddleName = Boolean.Parse(SMC_Re[1]);
            SMC_Requ.StdArabicLastName = Boolean.Parse(SMC_Re[2]);
            SMC_Requ.StdArabicFamilyName = Boolean.Parse(SMC_Re[3]);
            SMC_Requ.StdEnglishFristName = Boolean.Parse(SMC_Re[4]);
            SMC_Requ.StdEnglishMiddleName = Boolean.Parse(SMC_Re[5]);
            SMC_Requ.StdEnglishLastName = Boolean.Parse(SMC_Re[6]);
            SMC_Requ.StdEnglishFamilyName = Boolean.Parse(SMC_Re[7]);
            SMC_Requ.StdMotherArabicName = Boolean.Parse(SMC_Re[8]);
            SMC_Requ.StdMotherEnglishName = Boolean.Parse(SMC_Re[9]);
            SMC_Requ.StdFatherMobilePhone = Boolean.Parse(SMC_Re[10]);
            SMC_Requ.StdMotherMobilePhone = Boolean.Parse(SMC_Re[11]);
            SMC_Requ.StdFatherEmail = Boolean.Parse(SMC_Re[12]);
            SMC_Requ.StdMotherEmail = Boolean.Parse(SMC_Re[13]);
            SMC_Requ.StdFatherNationality = Boolean.Parse(SMC_Re[14]);
            SMC_Requ.StdMotherNationality = Boolean.Parse(SMC_Re[15]);
            SMC_Requ.StdFatherSpokenLanguage = Boolean.Parse(SMC_Re[16]);
            SMC_Requ.StdMotherSpokenLanguage = Boolean.Parse(SMC_Re[17]);
            SMC_Requ.StdFatherJob = Boolean.Parse(SMC_Re[18]);
            SMC_Requ.StdMotherJob = Boolean.Parse(SMC_Re[19]);
            SMC_Requ.StdFatherQualification = Boolean.Parse(SMC_Re[20]);
            SMC_Requ.StdMotherQualification = Boolean.Parse(SMC_Re[21]);
            SMC_Requ.StdStudentsAffairs1 = Boolean.Parse(SMC_Re[22]);
            SMC_Requ.StdStudentsAffairs2 = Boolean.Parse(SMC_Re[23]);
            SMC_Requ.StdBirthCode = Boolean.Parse(SMC_Re[24]);
            SMC_Requ.StdAddressGov = Boolean.Parse(SMC_Re[25]);
            SMC_Requ.StdEmergencyContact = Boolean.Parse(SMC_Re[26]);
            SMC_Requ.StdEmergencyPhone = Boolean.Parse(SMC_Re[27]);
            SMC_Requ.StdBOD = Boolean.Parse(SMC_Re[28]);
            SMC_Requ.StdBirthPlace = Boolean.Parse(SMC_Re[29]);
            SMC_Requ.StdGender = Boolean.Parse(SMC_Re[30]);
            SMC_Requ.StdReligion = Boolean.Parse(SMC_Re[31]);
            SMC_Requ.StdFatherNID = Boolean.Parse(SMC_Re[32]);
            SMC_Requ.StdMotherNID = Boolean.Parse(SMC_Re[33]);
            SMC_Requ.StdCity = Boolean.Parse(SMC_Re[34]);
            SMC_Requ.StdAddress = Boolean.Parse(SMC_Re[35]);
            SMC_Requ.StdNID = Boolean.Parse(SMC_Re[36]);
            SMC_Requ.StdSchoolID = Boolean.Parse(SMC_Re[37]);
            SMC_Requ.StdClassID = Boolean.Parse(SMC_Re[38]);
            SMC_Requ.StdNationality = Boolean.Parse(SMC_Re[39]);
            SMC_Requ.StdStatus = Boolean.Parse(SMC_Re[40]);
            SMC_Requ.StdJoinYear = Boolean.Parse(SMC_Re[41]);
            SMC_Requ.StdStaffSon = Boolean.Parse(SMC_Re[42]);
            SMC_Requ.StdLegalGuardianship = Boolean.Parse(SMC_Re[43]);
            SMC_Requ.StdParentsSeparated = Boolean.Parse(SMC_Re[44]);
            

            db.SaveChanges();


            return RedirectToAction("Edit");
        }

        // GET: StudentsMainCustoms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsMainCustom studentsMainCustom = db.StudentsMainCustoms.Find(id);
            if (studentsMainCustom == null)
            {
                return HttpNotFound();
            }
            return View(studentsMainCustom);
        }

        // POST: StudentsMainCustoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentsMainCustom studentsMainCustom = db.StudentsMainCustoms.Find(id);
            db.StudentsMainCustoms.Remove(studentsMainCustom);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
