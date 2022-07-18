using StudentsAffairsDashboard.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsAffairsDashboard.Controllers
{
    
    public class HomeController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        
        public ActionResult Index()
        {
            ViewBag.ClassID  = new SelectList(db.Classes,    "ClassID",  "ClassName");
            ViewBag.SchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName");
            ViewBag.GradeID  = new SelectList(db.Grades,     "GradeID",  "GradeName");
            return View();
        }

        [HttpPost]
        public ActionResult Index(string customerName)
        {
            StudentAffairsDatabaseEntities entities = new StudentAffairsDatabaseEntities();
            var searchResult = entities.StudentsMains.Where(
                a=>a.StdEnglishFristName.Contains(customerName)
                || a.StdEnglishLastName.Contains(customerName)
                || a.StdEnglishMiddleName.Contains(customerName)
                || a.StdEnglishFamilyName.Contains(customerName)
                || a.StdArabicFristName.Contains(customerName)
                || a.StdArabicLastName.Contains(customerName)
                || a.StdArabicMiddleName.Contains(customerName)
                || a.StdArabicFamilyName.Contains(customerName)).ToList();
            return View("Search", searchResult);
        }

        public ActionResult Indexx(string SchoolID,string GradeID,string ClassID)
        {
            Debug.Print(SchoolID+GradeID+ClassID);
            int School = Int32.Parse(SchoolID);
            int Class = Int32.Parse(ClassID);
            int Grade = Int32.Parse(GradeID);
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName",School);
            ViewBag.SchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName",Class);
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName",Grade);

            var searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.GradeID == Grade).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
            //.Where(a => a.NESSchool.SchoolID == School)
            //.Where(a => a.StdClassID == Class).ToList();
            
            return View("Search", searchResult);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Login(Account objUser)
        {
            if (ModelState.IsValid)
            {
                using (StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities())
                {
                    var obj = db.Accounts.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Email"] = obj.Email.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

       
    }
}