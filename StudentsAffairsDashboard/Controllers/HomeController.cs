﻿using StudentsAffairsDashboard.Models;
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
            return View(db.NESSchools.ToList());
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
            var searchResult = db.StudentGradesHistories.ToList();

            if (School == -1)
            {
                if (Grade == -1)
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).ToList();
                    }
                }
                else 
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.GradeID == Grade).ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.GradeID == Grade).ToList();
                    }
                }
            }
            else
            {
                if (Grade == -1)
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                }
                else 
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.GradeID == Grade).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.GradeID == Grade).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                }
            }
            
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
                        Session["currentSchool"] = obj.School.ToString();
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