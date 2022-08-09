﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentsAffairsDashboard.Models;
using Microsoft.Reporting.WebForms;

namespace StudentsAffairsDashboard.Controllers
{
    
    public class invoice_paymentController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();
        private const int uniformType = 5;
        //private const int school = int.Parse(Session["currentSchool"].ToString());

        // GET: invoice_payment
        public async Task<ActionResult> Index()
        {
        int school = int.Parse(Session["currentSchool"].ToString());
        var invoice_payment = db.invoice_payment.Include(i => i.invoice_payment2).Include(i => i.StudentsMain).Include(i => i.payment_details).Where(i=>i.payment_details.FirstOrDefault().school==school);
            return View(await invoice_payment.ToListAsync());
        }
        public async Task<ActionResult> IndexStudents()
        {
            int school = int.Parse(Session["currentSchool"].ToString());
            var students = db.StudentsMains.Include(i => i.NESSchool).Include(i => i.StudentGradesHistories).Where(i => i.StdSchoolID == school);
            return View(await students.ToListAsync());
        }
        public ActionResult AllInvoices()
        {
            var invoice_payment = db.invoice_payment.Include(i => i.invoice_payment2).Include(i => i.StudentsMain);
            return View(invoice_payment.ToList());
        }
        
        public ActionResult AllInvoices(List<invoice_payment> inv)
        {
            return View(inv);
        }
        // GET: invoice_payment/Details/5
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
        [ChildActionOnly]
        public ActionResult DetailsPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = db.invoice_payment.Include(i => i.payment_details).FirstOrDefault(i => i.id == id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            return PartialView(invoice_payment);
        }
        // GET: invoice_payment/Create
        public ActionResult Create(int? st)
        {
            if (Session["currentSchool"] == null)
                return HttpNotFound();
            else
            {
                if (st == null)
                {
                    return RedirectToAction("IndexStudents");
                }
                else
                {
                    return CreateView((int)st);
                    
                }
                
            }

        }
        public ActionResult CreateView(int st)
        {
            int school = Int32.Parse(Session["currentSchool"].ToString());
            StudentsMain stud = db.StudentsMains.Include(s => s.StudentGradesHistories).Include(s => s.Class).FirstOrDefault(s => s.StdCode == st);
            StudentGradesHistory history = stud.StudentGradesHistories.OrderBy(a => a.StudyYear).LastOrDefault();
            if (stud == null)
                throw new Exception("no student with given code");
            short studentType = 0;
            if (history.KindBatch == "Normal")
                studentType = 1;
            else if (history.KindBatch == "Gold")
                studentType = 2;
            else if (history.KindBatch == "Special")
                studentType = 3;
            else if (history.KindBatch == "Discount")
                studentType = 4;
            else
                throw new Exception("Student Type isn't known");
            int studentGrade = history.GradeID;


            string year = history.StudyYear;
            var invs = db.invoice_payment.Include(p => p.payment_details).Where(s=>s.student==st).ToList();
            List<payment_details> itemsPaid=new List<payment_details>();
            foreach(invoice_payment inv in invs)
            {
                var ioo = inv.payment_details.First().year;
                if (inv.payment_details.First().year.Substring(0,4) == year)
                {
                    foreach (payment_details p in inv.payment_details)
                        itemsPaid.Add(p);
                }
            }
            //get items you pay for except uniform
            var items = db.payment_details.Where(a => a.type != uniformType).Where(a => a.student_type == studentType).Where(a => a.year.Substring(0, 4) == year).Where(a => a.school == school).Where(a => a.Grade == studentGrade).ToList();
            foreach(var item in itemsPaid)
            {
                items.Remove(item);
            }
            invoice_payment invoice = new invoice_payment();
            var x = db.getPreviousPayment(st).FirstOrDefault();
            if (x != null)
                invoice.previous_payment = x.id;
            invoice_payment prev = db.invoice_payment.Find(invoice.previous_payment);
            invoice.date = DateTime.Now;
            invoice.payment_details = items;
            invoice.invoice_payment2 = prev;
            if (x != null)
                invoice.Notes = prev.Notes;
            invoice.student = st;
            if (x != null)
                invoice.remaining = prev.remaining;
            ViewData.Model = invoice;
            ViewBag.StdName = stud.StdEnglishFristName + " " + stud.StdEnglishMiddleName + " " + stud.StdEnglishLastName + " " + stud.StdEnglishFamilyName;
            ViewBag.StdSchool = stud.NESSchool.SchoolName;
            ViewBag.StdGrade = history.Grade.GradeName;
            ViewBag.StdGradeID = studentGrade;
            ViewBag.StdClass = stud.Class.ClassName;
            ViewBag.StdCode = st;
            ViewBag.machine = new SelectList(db.Machines.Where(m=>m.MachineSchool==school), "MachineID", "MachineID");
            
            return View();
        }
        


        // POST: invoice_payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(invoice_payment invoice_payment)
        {

            if (Request.IsAjaxRequest())
            {
                ModelState.Clear();
                return CreateView(invoice_payment.student);
            }

            if (ModelState.IsValid && invoice_payment.paid != 0)
            {
                decimal Total = 0;
                string newNotes = "\r\n";
                List<payment_details> pds = new List<payment_details>();
                foreach (payment_details pd in invoice_payment.payment_details)
                {
                    if (pd.selected)
                    {
                        if (pd.type == 1)
                        {
                            invoice_payment.payment_details.Find(a => a.type == 3).selected = true;
                            invoice_payment.payment_details.Find(a => a.type == 4).selected = true;
                        }
                        pds.Add(db.payment_details.Find(pd.id));
                        Total += pd.amount;
                        newNotes += pd.name;
                        newNotes += "\r\n";
                    }


                }
                invoice_payment.payment_details = pds;
                invoice_payment.total_cost = Total;
                decimal currentRemaining = Total - invoice_payment.paid;
                invoice_payment.remaining += currentRemaining;
                //invoice_payment.StudentsMain = db.StudentsMains.Find(invoice_payment.student);
                try
                {
                    if (invoice_payment.remaining < 0)
                        throw new Exception("you can not pay more than you owe");
                    if (invoice_payment.type == 2)
                        invoice_payment.machine = null;
                    db.invoice_payment.Add(invoice_payment);
                    await db.SaveChangesAsync();
                    if (invoice_payment.remaining == 0)
                        invoice_payment.Notes = "";
                    else
                    {
                        invoice_payment.Notes = invoice_payment.Notes + invoice_payment.id + newNotes;
                        //foreach (payment_details pd in invoice_payment.payment_details)
                        //    db.deleteInvoiceItem(invoice_payment.id, pd.id);
                        db.Entry(invoice_payment).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                }

            }


            ModelState.Clear();
            return CreateView(invoice_payment.student);
        }

        // GET: invoice_payment/Edit/5
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
            return EditView(invoice_payment, false);

        }

        
        private ActionResult EditView(invoice_payment invoice_payment, bool ajax)
        {
            int st = invoice_payment.student;
            StudentsMain stud = db.StudentsMains.Include(s => s.StudentGradesHistories).FirstOrDefault(s => s.StdCode == st);
            if (stud == null)
                throw new Exception("no student with given code");
            int school = stud.StdSchoolID;
            short studentType = 0;
            if (stud.StudentGradesHistories.LastOrDefault().KindBatch == "Normal")
                studentType = 1;
            else if (stud.StudentGradesHistories.LastOrDefault().KindBatch == "Gold")
                studentType = 2;
            else
                throw new Exception("Student Type isn't known");
            short studentGrade = (short)stud.StudentGradesHistories.LastOrDefault().GradeID;
            List<StudentsMain> Students = db.StudentsMains.Where(a => a.NESSchool.SchoolID == school).ToList();


            string year = stud.StudentGradesHistories.LastOrDefault().StudyYear;
            //get items you pay for except uniform
            var items = db.payment_details.Where(a => a.type != uniformType).Where(a => a.student_type == studentType).Where(a => a.year.Substring(0, 4) == year).Where(a => a.school == school).Where(a => a.Grade == studentGrade).ToList();
            if (ajax)
            {
                var x = db.getPreviousPayment(st).FirstOrDefault();
                if (x != null)
                    invoice_payment.previous_payment = x.id;
                else
                    invoice_payment.previous_payment = null;
                invoice_payment prev = db.invoice_payment.Find(invoice_payment.previous_payment);
                invoice_payment.date = DateTime.Now;
                invoice_payment.payment_details = items;
                invoice_payment.invoice_payment2 = prev;
                if (x != null)
                    invoice_payment.Notes = prev.Notes;
                if (x != null)
                    invoice_payment.remaining = prev.remaining;
                ViewBag.student = new SelectList(Students, "StdCode", "StdArabicFristName", invoice_payment.student);
                invoice_payment.total_cost = 0;
                invoice_payment.paid = 0;
                invoice_payment.remaining = 0;
                return View(invoice_payment);
            }

            foreach (payment_details item in invoice_payment.payment_details)
                item.selected = true;

            ViewBag.machine = new SelectList(db.Machines, "MachineID", "MachineID", invoice_payment.machine);
            ViewBag.student = new SelectList(Students, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // POST: invoice_payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(invoice_payment invoice_payment)
        {
            if (Request.IsAjaxRequest())
            {
                ModelState.Clear();
                return EditView(invoice_payment, true);
            }
            if (ModelState.IsValid && invoice_payment.paid != 0)
            {
                try
                {
                    decimal Total = 0;
                    string newNotes = "\r\n";
                    List<payment_details> pds = new List<payment_details>();
                    foreach (payment_details pd in invoice_payment.payment_details)
                    {
                        if (pd.selected||pd.type==5)
                        {
                            if (pd.type == 1)
                            {
                                invoice_payment.payment_details.Find(a => a.type == 3).selected = true;
                                invoice_payment.payment_details.Find(a => a.type == 4).selected = true;
                            }
                            pds.Add(db.payment_details.Find(pd.id));
                            Total += pd.amount;
                            newNotes += pd.name;
                            newNotes += "\r\n";
                        }


                    }
                    invoice_payment.payment_details = pds;
                    invoice_payment.total_cost = Total;
                    //if (Total == 0)
                    //    Total = inv.invoice.Remaining + inv.invoice.Paid;
                    decimal currentRemaining = Total - invoice_payment.paid;
                    invoice_payment.remaining = currentRemaining;


                    if (currentRemaining < 0)
                        throw new Exception("you can not pay more than you owe");
                    if (currentRemaining == 0)
                        invoice_payment.Notes = "";
                    else
                    {
                        if (invoice_payment.previous_payment == null)
                            invoice_payment.Notes = "";
                        else
                        {
                            invoice_payment pre = await db.invoice_payment.FindAsync(invoice_payment.previous_payment);
                            invoice_payment.Notes = pre.Notes;
                        }

                        invoice_payment.Notes = invoice_payment.Notes + invoice_payment.id + newNotes;
                    }
                    if (invoice_payment.type == 2)
                        invoice_payment.machine = null;
                    db.Entry(invoice_payment).State = EntityState.Modified;
                    await db.SaveChangesAsync();


                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                }

            }
            ModelState.Clear();
            return EditView(invoice_payment, false);
        }

        // GET: invoice_payment/Delete/5
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

        // POST: invoice_payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            invoice_payment invoice_payment = await db.invoice_payment.Include(p => p.payment_details).Include(p => p.StudentsMain).Include(p => p.invoice_payment2).FirstOrDefaultAsync(s => s.id == id);
            invoice_payment next = await db.invoice_payment.Where(p => p.previous_payment == invoice_payment.id).FirstOrDefaultAsync();
            if (next != null)
            {
                next.previous_payment = invoice_payment.previous_payment;
                db.Entry(next).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
                
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
