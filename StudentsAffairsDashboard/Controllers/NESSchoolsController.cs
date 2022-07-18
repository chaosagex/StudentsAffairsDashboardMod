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
    public class NESSchoolsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: NESSchools
        public ActionResult Index()
        {
            return View(db.NESSchools.ToList());
        }

        // GET: NESSchools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NESSchool nESSchool = db.NESSchools.Find(id);
            if (nESSchool == null)
            {
                return HttpNotFound();
            }
            return View(nESSchool);
        }

        // GET: NESSchools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NESSchools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SchoolID,SchoolName,SchooleAbbreviation")] NESSchool nESSchool)
        {
            if (ModelState.IsValid)
            {
                db.NESSchools.Add(nESSchool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nESSchool);
        }

        // GET: NESSchools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NESSchool nESSchool = db.NESSchools.Find(id);
            if (nESSchool == null)
            {
                return HttpNotFound();
            }
            return View(nESSchool);
        }

        // POST: NESSchools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolID,SchoolName,SchooleAbbreviation")] NESSchool nESSchool)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nESSchool).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nESSchool);
        }

        // GET: NESSchools/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NESSchool nESSchool = db.NESSchools.Find(id);
            if (nESSchool == null)
            {
                return HttpNotFound();
            }
            return View(nESSchool);
        }

        // POST: NESSchools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NESSchool nESSchool = db.NESSchools.Find(id);
            db.NESSchools.Remove(nESSchool);
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
