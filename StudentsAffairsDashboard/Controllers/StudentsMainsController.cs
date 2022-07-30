using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentsAffairsDashboard.Models;
using System.Diagnostics;
using System.Globalization;

namespace StudentsAffairsDashboard.Controllers
{
    public class StudentsMainsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: StudentsMains
        public ActionResult Index()
        {
            int SchoolIDsession = Int32.Parse(Session["CurrentSchool"].ToString());
            var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount).Where(a => a.NESSchool.SchoolID == SchoolIDsession);


            var ListNESSchools = db.NESSchools.ToList();
            ListNESSchools.Add(new NESSchool() { SchoolID = -1, SchoolName = "All", SchooleAbbreviation = "All" });
            var NESSchoolsResult = ListNESSchools.OrderBy(d => d.SchoolID).ToList();
            ViewBag.SchoolID = new SelectList(NESSchoolsResult, "SchoolID", "SchoolName");

            var ListClasses = db.Classes.ToList();
            ListClasses.Add(new Class() { ClassID = -1, ClassName = "All"});
            var ClassesResult = ListClasses.OrderBy(d => d.ClassID).ToList();
            ViewBag.ClassID = new SelectList(ClassesResult, "ClassID", "ClassName");

            var ListGrades = db.Grades.ToList();
            ListGrades.Add(new Grade() { GradeID = -1, GradeName = "All"});
            var GradesResult = ListGrades.OrderBy(d => d.GradeID).ToList();
            ViewBag.GradeID = new SelectList(GradesResult, "GradeID", "GradeName");

            return View(studentsMains.ToList());
            
        }

        // GET: StudentsMains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsMain studentsMain = db.StudentsMains.Find(id);
            if (studentsMain == null)
            {
                return HttpNotFound();
            }
            return View(studentsMain);
        }

        // GET: StudentsMains/Create
        public ActionResult Create()
        {
            ViewBag.StdClassID = new SelectList(db.Classes, "ClassID", "ClassName");
            ViewBag.StdGradeID = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.StdSchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName");
            ViewBag.StdCode = new SelectList(db.StudentAccounts, "StdCode", "StdEmail");            
            return View();
        }

        public ActionResult Update(int? id)
        {
            var studentsGrades = db.StudentGradesHistories.Include(s => s.Grade).Include(s => s.StudentsMain).Where(a=>a.StdCode == id);
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdEnglishFristName",id);
            return View(studentsGrades.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(string StdCode, string GradeID, string StudyYear,string KindBatch)
        {
            StudentGradesHistory studentGradesHistory = new StudentGradesHistory();
            studentGradesHistory.GradeID = Int32.Parse(GradeID);
            studentGradesHistory.StdCode = Int32.Parse(StdCode);
            studentGradesHistory.KindBatch = KindBatch;
            studentGradesHistory.StudyYear = StudyYear;
            db.StudentGradesHistories.Add(studentGradesHistory);
            db.SaveChanges();
            LogsController logs = new LogsController();
            DateTime now = DateTime.Now;
            Log log = new Log();
            log.UserName = Session["UserName"].ToString();
            log.Times = now.ToString();
            log.LogContent = "Update : Account (" + Session["UserName"].ToString() + ") Update Student Grade With ID: (" + studentGradesHistory.StdCode + ")";
            bool checklog = logs.Create(log);
            return RedirectToAction("Update", studentGradesHistory.StdCode);
        }

        // POST: StudentsMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StdCode,StdArabicFristName,StdArabicMiddleName,StdArabicLastName,StdArabicFamilyName,StdEnglishFristName,StdEnglishMiddleName,StdEnglishLastName,StdEnglishFamilyName,StdMotherArabicName,StdMotherEnglishName,StdFatherMobilePhone,StdMotherMobilePhone,StdFatherEmail,StdMotherEmail,StdFatherNationality,StdMotherNationality,StdFatherSpokenLanguage,StdMotherSpokenLanguage,StdFatherJob,StdMotherJob,StdFatherQualification,StdMotherQualification,StdStudentsAffairs1,StdStudentsAffairs2,StdBirthCode,StdAddressGov,StdEmergencyContact,StdEmergencyPhone,StdBOD,StdBirthPlace,StdGender,StdReligion,StdFatherNID,StdMotherNID,StdCity,StdAddress,StdNID,StdSchoolID,StdClassID,StdNationality,StdStatus,StdJoinYear,StdStaffSon,StdLegalGuardianship,StdParentsSeparated")] StudentsMain studentsMain, string StdGradeID)
        {
            
            if (ModelState.IsValid)
            {
                
                
                db.StudentsMains.Add(studentsMain);
                db.SaveChanges();

                StudentGradesHistory studentGradesHistory = new StudentGradesHistory();
                studentGradesHistory.GradeID = Int32.Parse(StdGradeID);
                studentGradesHistory.StdCode = studentsMain.StdCode;
                studentGradesHistory.StudyYear = studentsMain.StdJoinYear.ToString().Substring(4,4);
                db.StudentGradesHistories.Add(studentGradesHistory);
                db.SaveChanges();
                LogsController logs = new LogsController();
                DateTime now = DateTime.Now;
                Log log = new Log();
                log.UserName = Session["UserName"].ToString();
                log.Times = now.ToString();
                log.LogContent = "Create : Account (" + Session["UserName"].ToString() + ") Create New Student Data With ID: (" + studentsMain.StdCode + ")";
                bool checklog = logs.Create(log);
                return RedirectToAction("Index");
            }

            ViewBag.StdClassID = new SelectList(db.Classes, "ClassID", "ClassName", studentsMain.StdClassID);
            ViewBag.StdSchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName", studentsMain.StdSchoolID);
            ViewBag.StdCode = new SelectList(db.StudentAccounts, "StdCode", "StdEmail", studentsMain.StdCode);
            return View(studentsMain);
        }

        // GET: StudentsMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsMain studentsMain = db.StudentsMains.Find(id);

            if (studentsMain == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.StdClassID = new SelectList(db.Classes, "ClassID", "ClassName", studentsMain.StdClassID);
            ViewBag.StdSchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName", studentsMain.StdSchoolID);
            StudentGradesHistory studentsMain2 = db.StudentGradesHistories.Where(a=>a.StdCode == id).OrderByDescending(a=>a.GradeID).FirstOrDefault();
            //DateTime dt = (DateTime)studentsMain.StdBOD;

            //ViewBag.StdBOD = dt.ToString("dd/MM/yyyy");
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", studentsMain2.GradeID);
            ViewBag.StdCode = new SelectList(db.StudentAccounts, "StdCode", "StdEmail", studentsMain.StdCode);
            return View(studentsMain);
        }

        // POST: StudentsMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StdCode,StdArabicFristName,StdArabicMiddleName,StdArabicLastName,StdArabicFamilyName,StdEnglishFristName,StdEnglishMiddleName,StdEnglishLastName,StdEnglishFamilyName,StdMotherArabicName,StdMotherEnglishName,StdFatherMobilePhone,StdMotherMobilePhone,StdFatherEmail,StdMotherEmail,StdFatherNationality,StdMotherNationality,StdFatherSpokenLanguage,StdMotherSpokenLanguage,StdFatherJob,StdMotherJob,StdFatherQualification,StdMotherQualification,StdStudentsAffairs1,StdStudentsAffairs2,StdBirthCode,StdAddressGov,StdEmergencyContact,StdEmergencyPhone,StdBOD,StdBirthPlace,StdGender,StdReligion,StdFatherNID,StdMotherNID,StdCity,StdAddress,StdNID,StdSchoolID,StdClassID,StdNationality,StdStatus,StdJoinYear,StdStaffSon,StdLegalGuardianship,StdParentsSeparated")] StudentsMain studentsMain, string GradeID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentsMain).State = EntityState.Modified;
                db.SaveChanges();
                StudentGradesHistory studentGradesHistory = db.StudentGradesHistories.Where(a => a.StdCode == studentsMain.StdCode).OrderByDescending(a => a.GradeID).FirstOrDefault();
                studentGradesHistory.GradeID = Int32.Parse(GradeID);
                studentGradesHistory.StudyYear = studentsMain.StdJoinYear.ToString().Substring(4, 4);
                db.SaveChanges();
                LogsController logs = new LogsController();
                DateTime now = DateTime.Now;
                Log log = new Log();
                log.UserName = Session["UserName"].ToString();
                log.Times = now.ToString();
                log.LogContent = "Edit : Account (" + Session["UserName"].ToString() + ") Edit Student Data With ID: (" + studentsMain.StdCode + ")";
                bool checklog = logs.Create(log);
                return RedirectToAction("Index");
            }


            ViewBag.StdClassID = new SelectList(db.Classes, "ClassID", "ClassName", studentsMain.StdClassID);
            ViewBag.StdSchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName", studentsMain.StdSchoolID);
            StudentGradesHistory studentsMain2 = db.StudentGradesHistories.Where(a => a.StdCode == studentsMain.StdCode).OrderByDescending(a => a.GradeID).FirstOrDefault();
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", studentsMain2.GradeID);
            ViewBag.StdCode = new SelectList(db.StudentAccounts, "StdCode", "StdEmail", studentsMain.StdCode);

            return View(studentsMain);
        }

        // GET: StudentsMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsMain studentsMain = db.StudentsMains.Find(id);
            if (studentsMain == null)
            {
                return HttpNotFound();
            }
            return View(studentsMain);
        }

        // POST: StudentsMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentsMain studentsMain = db.StudentsMains.Find(id);
            db.StudentsMains.Remove(studentsMain);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: StudentsMains/Delete/5
        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentsMain studentsMain = db.StudentsMains.Find(id);
            if (studentsMain == null)
            {
                return HttpNotFound();
            }
            return View(studentsMain);
        }
        public ActionResult DeleteStudent(string hiddenId)
        {
            int StdCode = Int32.Parse(hiddenId);
            var studentsG = db.StudentGradesHistories.Where(a => a.StdCode == StdCode);
            foreach (StudentGradesHistory item in studentsG)
            {
                db.StudentGradesHistories.Remove(item);
                
            }
            db.SaveChanges();
            var studentsGG = db.StudentClothes.Where(a => a.StdCode == StdCode);
            foreach (StudentClothe item in studentsGG)
            {
                db.StudentClothes.Remove(item);
            }
            db.SaveChanges();
            StudentsMain studentsMain = db.StudentsMains.Find(Int32.Parse(hiddenId));
            db.StudentsMains.Remove(studentsMain);
            db.SaveChanges();
            LogsController logs = new LogsController();
            DateTime now = DateTime.Now;
            Log log = new Log();
            log.UserName = Session["UserName"].ToString();
            log.Times = now.ToString();
            log.LogContent = "Delete : Account (" + Session["UserName"].ToString() + ") Delete Student Data With ID: (" + hiddenId + ")";
            bool checklog = logs.Create(log);
            var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount);
            var ListNESSchools = db.NESSchools.ToList();
            ListNESSchools.Add(new NESSchool() { SchoolID = -1, SchoolName = "All", SchooleAbbreviation = "All" });
            var NESSchoolsResult = ListNESSchools.OrderBy(d => d.SchoolID).ToList();
            ViewBag.SchoolID = new SelectList(NESSchoolsResult, "SchoolID", "SchoolName");

            var ListClasses = db.Classes.ToList();
            ListClasses.Add(new Class() { ClassID = -1, ClassName = "All" });
            var ClassesResult = ListClasses.OrderBy(d => d.ClassID).ToList();
            ViewBag.ClassID = new SelectList(ClassesResult, "ClassID", "ClassName");

            var ListGrades = db.Grades.ToList();
            ListGrades.Add(new Grade() { GradeID = -1, GradeName = "All" });
            var GradesResult = ListGrades.OrderBy(d => d.GradeID).ToList();
            ViewBag.GradeID = new SelectList(GradesResult, "GradeID", "GradeName");
            return View("Index",studentsMains.ToList());
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
