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
    public class StudentSubjectEnrollsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: StudentSubjectEnrolls
        public ActionResult Index()
        {
            var studentSubjectEnrolls = db.StudentSubjectEnrolls.Include(s => s.StudentsMain).Include(s => s.Subject);
            return View(studentSubjectEnrolls.ToList());
        }

        // GET: StudentSubjectEnrolls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubjectEnroll studentSubjectEnroll = db.StudentSubjectEnrolls.Find(id);
            if (studentSubjectEnroll == null)
            {
                return HttpNotFound();
            }
            return View(studentSubjectEnroll);
        }

        // GET: StudentSubjectEnrolls/Create
        public ActionResult Create()
        {
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID");
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectCode");
            return View();
        }

        // POST: StudentSubjectEnrolls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SSID,StdCode,SubjectID,StudyYear")] StudentSubjectEnroll studentSubjectEnroll)
        {
            if (ModelState.IsValid)
            {
                db.StudentSubjectEnrolls.Add(studentSubjectEnroll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentSubjectEnroll.StdCode);
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectCode", studentSubjectEnroll.SubjectID);
            return View(studentSubjectEnroll);
        }

        // GET: StudentSubjectEnrolls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubjectEnroll studentSubjectEnroll = db.StudentSubjectEnrolls.Find(id);
            if (studentSubjectEnroll == null)
            {
                return HttpNotFound();
            }
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentSubjectEnroll.StdCode);
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectCode", studentSubjectEnroll.SubjectID);
            return View(studentSubjectEnroll);
        }

        // POST: StudentSubjectEnrolls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SSID,StdCode,SubjectID,StudyYear")] StudentSubjectEnroll studentSubjectEnroll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentSubjectEnroll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentSubjectEnroll.StdCode);
            ViewBag.SubjectID = new SelectList(db.Subjects, "SubjectID", "SubjectCode", studentSubjectEnroll.SubjectID);
            return View(studentSubjectEnroll);
        }

        // GET: StudentSubjectEnrolls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentSubjectEnroll studentSubjectEnroll = db.StudentSubjectEnrolls.Find(id);
            if (studentSubjectEnroll == null)
            {
                return HttpNotFound();
            }
            return View(studentSubjectEnroll);
        }

        // POST: StudentSubjectEnrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentSubjectEnroll studentSubjectEnroll = db.StudentSubjectEnrolls.Find(id);
            db.StudentSubjectEnrolls.Remove(studentSubjectEnroll);
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
