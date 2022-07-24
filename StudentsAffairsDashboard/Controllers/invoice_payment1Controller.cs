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
    public class invoice_payment1Controller : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: invoice_payment1
        public async Task<ActionResult> Index()
        {
            var invoice_payment = db.invoice_payment.Include(i => i.invoice_payment2).Include(i => i.StudentsMain);
            return View(await invoice_payment.ToListAsync());
        }

        // GET: invoice_payment1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = await db.invoice_payment.FindAsync(id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            return View(invoice_payment);
        }

        // GET: invoice_payment1/Create
        public ActionResult Create()
        {
            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes");
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName");
            return View();
        }

        // POST: invoice_payment1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,student,date,total_cost,paid,remaining,previous_payment,Notes")] invoice_payment invoice_payment)
        {
            if (ModelState.IsValid)
            {
                db.invoice_payment.Add(invoice_payment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes", invoice_payment.previous_payment);
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // GET: invoice_payment1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = await db.invoice_payment.FindAsync(id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes", invoice_payment.previous_payment);
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // POST: invoice_payment1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,student,date,total_cost,paid,remaining,previous_payment,Notes")] invoice_payment invoice_payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice_payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes", invoice_payment.previous_payment);
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // GET: invoice_payment1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = await db.invoice_payment.FindAsync(id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            return View(invoice_payment);
        }

        // POST: invoice_payment1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            invoice_payment invoice_payment = await db.invoice_payment.FindAsync(id);
            db.invoice_payment.Remove(invoice_payment);
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
