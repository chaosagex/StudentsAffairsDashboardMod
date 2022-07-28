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
using Microsoft.Reporting.WebForms;

namespace StudentsAffairsDashboard.Controllers
{
    
    public class invoice_paymentController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();
        private const int startID = 1031;//1038
        private const int uniformType = 5;

        // GET: invoice_payment
        public async Task<ActionResult> Index()
        {
            
            var invoice_payment = db.invoice_payment.Include(i => i.invoice_payment2).Include(i => i.StudentsMain);
            return View(await invoice_payment.ToListAsync());
        }
        public ActionResult AllInvoices()
        {
            var invoice_payment = db.invoice_payment.Include(i => i.invoice_payment2).Include(i => i.StudentsMain);
            return View(invoice_payment.ToList());
        }
        public ActionResult Report()
        {
            ReportViewer x = new ReportViewer();
            x.ProcessingMode = ProcessingMode.Local;
            x.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Report1.rdlc";
            ReportDataSource source = new ReportDataSource("Invoices",(from invoice_payment in db.invoice_payment select invoice_payment));
            x.LocalReport.DataSources.Clear();
            x.LocalReport.DataSources.Add(source);

            return View(x);
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
        public ActionResult Create()
        {
            return CreateView(startID);

        }
        public ActionResult CreateView(int st)
        {
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

            ViewBag.student = new SelectList(Students, "StdCode", "StdEnglishFristName", st);
            return View();
        }
        //public ActionResult UniformView(invoice_payment invoice)
        //{
        //    int st = invoice.student;
        //    StudentsMain stud = db.StudentsMains.Include(s => s.StudentGradesHistories).FirstOrDefault(s => s.StdCode == st);
        //    if (stud == null)
        //        throw new Exception("no student with given code");
        //    int school = stud.StdSchoolID;
        //    short studentType = 0;
        //    if (stud.StudentGradesHistories.LastOrDefault().KindBatch == "Normal")
        //        studentType = 1;
        //    else if (stud.StudentGradesHistories.LastOrDefault().KindBatch == "Gold")
        //        studentType = 2;
        //    else
        //        throw new Exception("Student Type isn't known");
        //    short studentGrade = (short)stud.StudentGradesHistories.LastOrDefault().GradeID;
        //    List<StudentsMain> Students = db.StudentsMains.Where(a => a.NESSchool.SchoolID == school).ToList();


        //    string year = stud.StudentGradesHistories.LastOrDefault().StudyYear;
        //    //get items you pay for except uniform
        //    var items = db.payment_details.Where(a => a.type != uniformType).Where(a => a.student_type == studentType).Where(a => a.year.Substring(0, 4) == year).Where(a => a.school == school).Where(a => a.Grade == studentGrade).ToList();
        //    var x = db.getPreviousPayment(st).FirstOrDefault();
        //    if (x != null)
        //        invoice.previous_payment = x.id;
        //    invoice_payment prev = db.invoice_payment.Find(invoice.previous_payment);
        //    invoice.date = DateTime.Now;
        //    invoice.payment_details = items;
        //    invoice.invoice_payment2 = prev;
        //    if (x != null)
        //        invoice.Notes = prev.Notes;
        //    invoice.student = st;
        //    if (x != null)
        //        invoice.remaining = prev.remaining;
        //    payment_details item = invoice.payment_details.First();
        //    item.school = stud.StdSchoolID;
        //    item.Grade = (short)studentGrade;
        //    item.year = year;
        //    item.student_type = studentType;
        //    db.payment_details.Add(item);
        //    db.SaveChanges();
        //    ViewData.Model = invoice;

        //    ViewBag.student = new SelectList(Students, "StdCode", "StdEnglishFristName", st);

        //    return View();
        //}


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
