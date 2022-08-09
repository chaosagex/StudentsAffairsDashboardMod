using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentsAffairsDashboard.Models;

namespace StudentsAffairsDashboard.Controllers
{
    public class payment_detailsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: payment_details
        public async Task<ActionResult> Index()
        {
            var payment_details = db.payment_details.Include(p => p.NESSchool);
            return View(await payment_details.ToListAsync());
        }

        // GET: payment_details/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment_details payment_details = await db.payment_details.FindAsync(id);
            if (payment_details == null)
            {
                return HttpNotFound();
            }
            return View(payment_details);
        }

        // GET: payment_details/Create
        public ActionResult Create()
        {
            ViewBag.grade = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName");
            return View();
        }

        // POST: payment_details/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name,type,amount,school,year,student_type,Grade")] payment_details payment_details)
        {
            if (ModelState.IsValid)
            {
                db.payment_details.Add(payment_details);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.grade = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName", payment_details.school);
            return View(payment_details);
        }

        // GET: payment_details/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment_details payment_details = await db.payment_details.FindAsync(id);
            if (payment_details == null)
            {
                return HttpNotFound();
            }
            ViewBag.grade = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName", payment_details.school);
            return View(payment_details);
        }

        // POST: payment_details/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name,type,amount,school,year,student_type,Grade")] payment_details payment_details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment_details).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.grade = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.school = new SelectList(db.NESSchools, "SchoolID", "SchoolName", payment_details.school);
            return View(payment_details);
        }

        // GET: payment_details/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment_details payment_details = await db.payment_details.FindAsync(id);
            if (payment_details == null)
            {
                return HttpNotFound();
            }
            return View(payment_details);
        }

        // POST: payment_details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            payment_details payment_details = await db.payment_details.FindAsync(id);
            db.payment_details.Remove(payment_details);
            await db.SaveChangesAsync();
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
