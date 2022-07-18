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
    public class StudentGradesHistoriesController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: StudentGradesHistories
        public ActionResult Index()
        {
            var studentGradesHistories = db.StudentGradesHistories.Include(s => s.Grade).Include(s => s.StudentsMain);
            return View(studentGradesHistories.ToList());
        }

        // GET: StudentGradesHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGradesHistory studentGradesHistory = db.StudentGradesHistories.Find(id);
            if (studentGradesHistory == null)
            {
                return HttpNotFound();
            }
            return View(studentGradesHistory);
        }

        // GET: StudentGradesHistories/Create
        public ActionResult Create()
        {
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdEnglishFristName");
            return View();
        }

        // POST: StudentGradesHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SGID,StdCode,GradeID,StudyYear")] StudentGradesHistory studentGradesHistory)
        {
            if (ModelState.IsValid)
            {
                db.StudentGradesHistories.Add(studentGradesHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", studentGradesHistory.GradeID);
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentGradesHistory.StdCode);
            return View(studentGradesHistory);
        }

        // GET: StudentGradesHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGradesHistory studentGradesHistory = db.StudentGradesHistories.Find(id);
            if (studentGradesHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", studentGradesHistory.GradeID);
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentGradesHistory.StdCode);
            return View(studentGradesHistory);
        }

        // POST: StudentGradesHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SGID,StdCode,GradeID,StudyYear")] StudentGradesHistory studentGradesHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentGradesHistory).State = EntityState.Modified;
                db.SaveChanges();
                LogsController logs = new LogsController();
                DateTime now = DateTime.Now;
                Log log = new Log();
                log.UserName = Session["UserName"].ToString();
                log.Times = now.ToString();
                log.LogContent = "Edit : Account (" + Session["UserName"].ToString() + ") Edit Student Grade With ID: (" + studentGradesHistory.StdCode + ")";
                bool checklog = logs.Create(log);
                return RedirectToAction("Index");
            }
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", studentGradesHistory.GradeID);
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentGradesHistory.StdCode);
            return View(studentGradesHistory);
        }

        // GET: StudentGradesHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentGradesHistory studentGradesHistory = db.StudentGradesHistories.Find(id);
            if (studentGradesHistory == null)
            {
                return HttpNotFound();
            }
            return View(studentGradesHistory);
        }

        // POST: StudentGradesHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentGradesHistory studentGradesHistory = db.StudentGradesHistories.Find(id);
            db.StudentGradesHistories.Remove(studentGradesHistory);
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
