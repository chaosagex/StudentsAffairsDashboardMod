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
    public class StudentAccountsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: StudentAccounts
        public ActionResult Index()
        {
            var studentAccounts = db.StudentAccounts.Include(s => s.StudentsMain);
            return View(studentAccounts.ToList());
        }

        // GET: StudentAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAccount studentAccount = db.StudentAccounts.Find(id);
            if (studentAccount == null)
            {
                return HttpNotFound();
            }
            return View(studentAccount);
        }

        // GET: StudentAccounts/Create
        public ActionResult Create()
        {
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID");
            return View();
        }

        // POST: StudentAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StdCode,StdEmail,StdPassword,StdStatus")] StudentAccount studentAccount)
        {
            if (ModelState.IsValid)
            {
                db.StudentAccounts.Add(studentAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentAccount.StdCode);
            return View(studentAccount);
        }

        // GET: StudentAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAccount studentAccount = db.StudentAccounts.Find(id);
            if (studentAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentAccount.StdCode);
            return View(studentAccount);
        }

        // POST: StudentAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StdCode,StdEmail,StdPassword,StdStatus")] StudentAccount studentAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdNID", studentAccount.StdCode);
            return View(studentAccount);
        }

        // GET: StudentAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAccount studentAccount = db.StudentAccounts.Find(id);
            if (studentAccount == null)
            {
                return HttpNotFound();
            }
            return View(studentAccount);
        }

        // POST: StudentAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentAccount studentAccount = db.StudentAccounts.Find(id);
            db.StudentAccounts.Remove(studentAccount);
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
