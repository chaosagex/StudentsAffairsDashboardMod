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
    public class PaymentDetailsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: PaymentDetails
        public ActionResult Index()
        {
            var payment_details = db.payment_details.Include(p => p.NESSchool);
            return View(payment_details.ToList());
        }

        // GET: PaymentDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment_details payment_details = db.payment_details.Find(id);
            if (payment_details == null)
            {
                return HttpNotFound();
            }
            return View(payment_details);
        }

        // GET: PaymentDetails/Create
        public ActionResult Create()
        {
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName");
            return View();
        }

        // POST: PaymentDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,type,amount,school,year,student_type,Grade")] payment_details payment_details)
        {
            if (ModelState.IsValid)
            {
                db.payment_details.Add(payment_details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName", payment_details.school);
            return View(payment_details);
        }

        // GET: PaymentDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment_details payment_details = db.payment_details.Find(id);
            if (payment_details == null)
            {
                return HttpNotFound();
            }
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName", payment_details.school);
            return View(payment_details);
        }

        // POST: PaymentDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,type,amount,school,year,student_type,Grade")] payment_details payment_details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment_details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName", payment_details.school);
            return View(payment_details);
        }

        // GET: PaymentDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment_details payment_details = db.payment_details.Find(id);
            if (payment_details == null)
            {
                return HttpNotFound();
            }
            return View(payment_details);
        }

        // POST: PaymentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            payment_details payment_details = db.payment_details.Find(id);
            db.payment_details.Remove(payment_details);
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
