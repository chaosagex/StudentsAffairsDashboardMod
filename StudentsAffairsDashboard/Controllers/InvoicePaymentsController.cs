using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentsAffairsDashboard.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace StudentsAffairsDashboard.Controllers
{
    public class InvoicePaymentsController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: InvoicePayments
        public ActionResult Index()
        {
            var invoice_payment = db.invoice_payment.Include(i => i.invoice_payment2).Include(i => i.StudentsMain);
            return View(invoice_payment.ToList());
        }

        // GET: InvoicePayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = db.invoice_payment.Find(id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            return View(invoice_payment);
        }

        // GET: InvoicePayments/Create
        public ActionResult Create()
        {
            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes");
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName");
            return View();
        }

        // POST: InvoicePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,student,date,total_cost,paid,remaining,previous_payment,Notes")] invoice_payment invoice_payment)
        {
            if (ModelState.IsValid)
            {
                db.invoice_payment.Add(invoice_payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes", invoice_payment.previous_payment);
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // GET: InvoicePayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = db.invoice_payment.Find(id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes", invoice_payment.previous_payment);
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // POST: InvoicePayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,student,date,total_cost,paid,remaining,previous_payment,Notes")] invoice_payment invoice_payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice_payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.previous_payment = new SelectList(db.invoice_payment, "id", "Notes", invoice_payment.previous_payment);
            ViewBag.student = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", invoice_payment.student);
            return View(invoice_payment);
        }

        // GET: InvoicePayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_payment invoice_payment = db.invoice_payment.Find(id);
            if (invoice_payment == null)
            {
                return HttpNotFound();
            }
            return View(invoice_payment);
        }

        // POST: InvoicePayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            invoice_payment invoice_payment = db.invoice_payment.Find(id);
            db.invoice_payment.Remove(invoice_payment);
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
//    public class InvoicePaymentsController : Controller
//    {
//        private readonly StudentAffairsDatabaseEntities _context;

//        public InvoicePaymentsController(StudentAffairsDatabaseEntities context)
//        {
//            _context = context;
//        }

//        // GET: InvoicePayments
//        public async Task<IActionResult> Index()
//        {
//            var StudentAffairsDatabaseEntities = _context.InvoicePayments.Include(i => i.PreviousPaymentNavigation).Include(i => i.StudentNavigation);
//            return View(await StudentAffairsDatabaseEntities.ToListAsync());
//        }

//        // GET: InvoicePayments/Details/5
//        public async Task<IActionResult> Details(long? id)
//        {
//            if (id == null || _context.InvoicePayments == null)
//            {
//                return NotFound();
//            }

//            var invoicePayment = await _context.InvoicePayments
//                .Include(i => i.PreviousPaymentNavigation)
//                .Include(i => i.StudentNavigation)
//                .Include(i => i.PaymentItems)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (invoicePayment == null)
//            {
//                return NotFound();
//            }
//            long school = 1;
//            return View(invoicePayment);
//        }

//        // GET: InvoicePayments/Create
//        public IActionResult Create()
//        {
//            return CreateView(1);
//        }
//        public IActionResult CreateView(int st)
//        {
//            long school = 1;
//            st -= 1;
//            List<Student> Students = populateStudents(school);
//            ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);
//            List<PaymentDetail> items = populatePaymentItems(Students[st].School, Students[st].Type, Students[st].Grade);
//            invoiceView inv = new invoiceView();
//            inv.items = items;
//            inv.invoice = new InvoicePayment();
//            inv.invoice.Date = DateTime.Now;
//            InvoicePayment? prev = getPreviousPayment(Students[st].Id);
//            inv.invoice.PreviousPayment = prev.Id;
//            inv.invoice.Notes = prev.Notes;
//            inv.invoice.Student = st + 1;
//            inv.invoice.Remaining = prev.Remaining;
//            return View(inv);
//        }
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        [HttpGet]
//        public async Task<ActionResult> getNewPaymentList(int studentId)
//        {
//            Student s = await _context.Students.FindAsync((long)studentId);
//            if (s == null)
//                return null;
//            List<PaymentDetail> pl = populatePaymentItems(s.School, s.Type, s.Grade);

//            return Ok(pl);

//        }
//        [HttpGet]
//        public async Task<ActionResult> getNewPreviousPayment(int studentId)
//        {
//            InvoicePayment? pp = getPreviousPayment(studentId);

//            return Ok(pp.Id);

//        }
//        private InvoicePayment? getPreviousPayment(long student)
//        {
//            var previousPayment = _context.InvoicePayments.FromSqlInterpolated($"EXECUTE dbo.getPreviousPayment {student}").ToList().FirstOrDefault<InvoicePayment>();
//            if (previousPayment == null)
//                return null;
//            else
//                return previousPayment;
//        }

//        private List<PaymentDetail> populatePaymentItems(long school, short studentType, short grade)
//        {
//            return _context.PaymentDetails.FromSqlInterpolated($"EXECUTE dbo.getPaymentItems {school},{studentType},{grade}").ToList();
//        }
//        private List<Student> populateStudents(long school)
//        {
//            return _context.Students.FromSqlInterpolated($"EXECUTE dbo.getStudentBySchool {school}").ToList();
//        }
//        // POST: InvoicePayments/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(invoiceView inv)
//        {
//            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
//            long school = 1;
//            int st = (int)inv.invoice.Student;
//            st -= 1;
//            List<Student> Students = null;
//            InvoicePayment? prev = null;
//            List<PaymentDetail> items = null;
//            if (isAjaxCall)
//            {
//                ModelState.Clear();
//                Students = populateStudents(school);
//                ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);
//                items = populatePaymentItems(Students[st].School, Students[st].Type, Students[st].Grade);
//                inv.items = items;
//                prev = getPreviousPayment(Students[st].Id);
//                inv.invoice.PreviousPayment = prev.Id;
//                inv.invoice.Remaining = prev.Remaining;
//                inv.invoice.Notes = prev.Notes;
//                return View(inv);
//            }

//            if (ModelState.IsValid && inv.invoice.Paid != 0)
//            {
//                decimal Total = 0;
//                string newNotes = "\r\n";
//                foreach (PaymentDetail pd in inv.items)
//                {
//                    if (pd.selected)
//                    {
//                        PaymentDetail item = await _context.PaymentDetails.FindAsync(pd.Id);
//                        inv.invoice.PaymentItems.Add(item);
//                        Total += pd.Amount;
//                        newNotes += item.Name;
//                        newNotes += "\r\n";
//                    }

//                }
//                inv.invoice.TotalCost = Total;
//                decimal currentRemaining = Total - inv.invoice.Paid;
//                inv.invoice.Remaining += currentRemaining;

//                try
//                {
//                    if (inv.invoice.Remaining < 0)
//                        throw new Exception("you can not pay more than you owe");
//                    _context.Add(inv.invoice);
//                    await _context.SaveChangesAsync();
//                    if (inv.invoice.Remaining == 0)
//                        inv.invoice.Notes = "";
//                    else
//                    {
//                        inv.invoice.Notes = inv.invoice.Notes + inv.invoice.Id + newNotes;
//                        deleteItems(inv.invoice.Id, inv.invoice.PaymentItems);
//                        _context.Update(inv.invoice);
//                        await _context.SaveChangesAsync();
//                    }

//                    return RedirectToAction(nameof(Index));
//                }
//                catch (Exception e)
//                {
//                    ViewBag.Message = e.Message;
//                }

//            }
//            ModelState.Clear();

//            Students = populateStudents(school);
//            ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);
//            items = populatePaymentItems(Students[st].School, Students[st].Type, Students[st].Grade);
//            inv.items = items;
//            inv.invoice.Paid = 0;
//            prev = getPreviousPayment(Students[st].Id);
//            inv.invoice.PreviousPayment = prev.Id;
//            inv.invoice.Remaining = prev.Remaining;
//            inv.invoice.Notes = prev.Notes;
//            return View(inv);
//        }

//        // GET: InvoicePayments/Edit/5
//        public async Task<IActionResult> Edit(long? id)
//        {
//            if (id == null || _context.InvoicePayments == null)
//            {
//                return NotFound();
//            }

//            var invoicePayment = await _context.InvoicePayments.FindAsync(id);
//            if (invoicePayment == null)
//            {
//                return NotFound();
//            }
//            long school = 1;
//            int st = (int)invoicePayment.Student;
//            st -= 1;
//            List<Student> Students = populateStudents(school);
//            InvoicePayment? prev = null;
//            List<PaymentDetail> items = populatePaymentItemsEdit(Students[st].School, Students[st].Type, Students[st].Grade, invoicePayment); ;
//            ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);
//            invoiceView inv = new invoiceView();
//            inv.invoice = invoicePayment;
//            inv.items = items;
//            return View(inv);
//        }

//        private List<PaymentDetail> populatePaymentItemsEdit(long school, short type, short grade, InvoicePayment invoicePayment)
//        {
//            List<PaymentDetail> items = _context.PaymentDetails.FromSqlInterpolated($"EXECUTE dbo.getPaymentItems {school},{type},{grade}").ToList();
//            foreach (PaymentDetail item in items)
//            {
//                var lis = _context.PaymentDetails.FromSqlInterpolated($"EXECUTE dbo.getInvoiceItem {item.Id},{invoicePayment.Id}").ToList();
//                if (lis.Count() > 0)
//                {
//                    item.selected = true;
//                }
//            }
//            return items;
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> EditView(invoiceView inv)
//        {
//            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
//            long school = 1;
//            int st = (int)inv.invoice.Student;
//            st -= 1;
//            List<Student> Students = null;
//            InvoicePayment? prev = null;
//            List<PaymentDetail> items = null;
//            if (isAjaxCall)
//            {
//                ModelState.Clear();
//                Students = populateStudents(school);
//                prev = getPreviousPayment(Students[st].Id);
//                inv.invoice.PreviousPayment = prev.Id;
//                inv.invoice.Remaining = prev.Remaining;
//                inv.invoice.Notes = prev.Notes;
//                items = populatePaymentItems(Students[st].School, Students[st].Type, Students[st].Grade);
//                ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);
//                inv.items = items;
//                return View(inv);
//            }
//            return View(inv);
//        }
//        // POST: InvoicePayments/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(long? id, invoiceView inv)
//        {
//            bool isAjaxCall = Request.Headers["x-requested-with"] == "XMLHttpRequest";
//            if (id != inv.invoice.Id && !isAjaxCall)
//            {
//                return NotFound();
//            }

//            long school = 1;
//            int st = (int)inv.invoice.Student;
//            st -= 1;
//            List<Student> Students = null;
//            InvoicePayment? prev = null;
//            List<PaymentDetail> items = null;
//            if (isAjaxCall)
//            {
//                ModelState.Clear();
//                Students = populateStudents(school);
//                prev = getPreviousPayment(Students[st].Id);
//                inv.invoice.PreviousPayment = prev.Id;
//                inv.invoice.Remaining = prev.Remaining;
//                inv.invoice.Notes = prev.Notes;
//                items = populatePaymentItemsEdit(Students[st].School, Students[st].Type, Students[st].Grade, inv.invoice); ;
//                ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);
//                inv.items = items;
//                return View(inv);
//            }
//            if (ModelState.IsValid && inv.invoice.Paid != 0)
//            {
//                try
//                {
//                    decimal Total = 0;
//                    string newNotes = "\r\n";
//                    foreach (PaymentDetail pd in inv.items)
//                    {
//                        if (pd.selected)
//                        {
//                            PaymentDetail item = await _context.PaymentDetails.FindAsync(pd.Id);
//                            inv.invoice.PaymentItems.Add(item);
//                            Total += pd.Amount;
//                            newNotes += item.Name;
//                            newNotes += "\r\n";
//                        }

//                    }
//                    inv.invoice.TotalCost = Total;
//                    if (Total == 0)
//                        Total = inv.invoice.Remaining + inv.invoice.Paid;
//                    decimal currentRemaining = Total - inv.invoice.Paid;
//                    inv.invoice.Remaining = currentRemaining;

//                    try
//                    {
//                        if (inv.invoice.Remaining < 0)
//                            throw new Exception("you can not pay more than you owe");
//                        deleteItems(inv.invoice.Id, inv.invoice.PaymentItems);
//                        if (inv.invoice.Remaining == 0)
//                            inv.invoice.Notes = "";
//                        else
//                        {
//                            if (inv.invoice.PreviousPayment == null)
//                                inv.invoice.Notes = "";
//                            else
//                            {
//                                InvoicePayment pre = await _context.InvoicePayments.FindAsync(inv.invoice.PreviousPayment);
//                                inv.invoice.Notes = pre.Notes;
//                            }

//                            inv.invoice.Notes = inv.invoice.Notes + inv.invoice.Id + newNotes;
//                        }
//                        _context.Update(inv.invoice);
//                        await _context.SaveChangesAsync();

//                        deleteItems(inv.invoice.Id, inv.invoice.PaymentItems);
//                        _context.Update(inv.invoice);
//                    }
//                    catch (DbUpdateConcurrencyException)
//                    {
//                        if (!InvoicePaymentExists(inv.invoice.Id))
//                        {
//                            return NotFound();
//                        }
//                        else
//                        {
//                            throw;
//                        }
//                    }
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (Exception e)
//                {
//                    ViewBag.Message = e.Message;
//                }
//            }
//            ModelState.Clear();

//            Students = populateStudents(school);
//            ViewData["Student"] = new SelectList(Students, "Id", "Name", st + 1);

//            Students = populateStudents(school);
//            prev = getPreviousPayment(Students[st].Id);
//            inv.invoice.PreviousPayment = prev.Id;
//            inv.invoice.Remaining = prev.Remaining;
//            inv.invoice.Notes = prev.Notes;
//            items = populatePaymentItemsEdit(Students[st].School, Students[st].Type, Students[st].Grade, inv.invoice);
//            inv.items = items;
//            return View(inv);
//        }

//        private void deleteItems(long id, ICollection<PaymentDetail> paymentItems)
//        {
//            foreach (PaymentDetail item in paymentItems)
//            {
//                var x = _context.Database.ExecuteSqlInterpolated($"EXECUTE dbo.deleteInvoiceItem {item.Id},{id}");
//                x = 0;
//            }
//        }

//        // GET: InvoicePayments/Delete/5
//        public async Task<IActionResult> Delete(long? id)
//        {
//            if (id == null || _context.InvoicePayments == null)
//            {
//                return NotFound();
//            }

//            var invoicePayment = await _context.InvoicePayments
//                .Include(i => i.PreviousPaymentNavigation)
//                .Include(i => i.StudentNavigation)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (invoicePayment == null)
//            {
//                return NotFound();
//            }

//            return View(invoicePayment);
//        }

//        // POST: InvoicePayments/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(long id)
//        {
//            if (_context.InvoicePayments == null)
//            {
//                return Problem("Entity set 'StudentAffairsDatabaseEntities.InvoicePayments'  is null.");
//            }
//            var invoicePayment = await _context.InvoicePayments
//               .Include(i => i.PreviousPaymentNavigation)
//               .Include(i => i.StudentNavigation)
//               .Include(i => i.PaymentItems)
//               .FirstOrDefaultAsync(m => m.Id == id);
//            if (invoicePayment != null)
//            {
//                deleteItems(invoicePayment.Id, invoicePayment.PaymentItems);
//                _context.InvoicePayments.Remove(invoicePayment);

//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool InvoicePaymentExists(long id)
//        {
//            return (_context.InvoicePayments?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}