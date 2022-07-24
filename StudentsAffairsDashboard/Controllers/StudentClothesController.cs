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
    public class StudentClothesController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        // GET: StudentClothes
        public ActionResult Index()
        {
            var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount);
            return View(studentsMains.ToList());
        }

        // GET: StudentClothes/Receiving
        public ActionResult Receiving()
        {
            var studentClothes = db.StudentClothes.Include(s => s.Cloth).Include(s => s.StudentsMain).GroupBy(a=>a.StdCode).Select(a=>a.FirstOrDefault());
            List <StudentsMain> All = new List<StudentsMain> ();
            
            foreach (var item in studentClothes)
            {
                if (!db.StudentClothes.Where(a=>a.StdCode == item.StdCode).All(a => a.ReceivingStatus == "True"))
                {
                    All.Add(db.StudentsMains.Find(item.StdCode));
                }
            }
            return View(All);


            //var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount);            
            //return View(studentsMains.ToList());
        }

        // GET: StudentClothes/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.StdName = db.StudentsMains.Find(id).StdEnglishFristName + " " + db.StudentsMains.Find(id).StdEnglishMiddleName + " " + db.StudentsMains.Find(id).StdEnglishLastName + " " + db.StudentsMains.Find(id).StdEnglishFamilyName;
                ViewBag.StdSchool = db.StudentsMains.Find(id).NESSchool.SchoolName;
                ViewBag.StdGrade = db.StudentsMains.Find(id).StudentGradesHistories.LastOrDefault().Grade.GradeName;
                ViewBag.StdClass = db.StudentsMains.Find(id).Class.ClassName;
                ViewBag.StdCode = id;

                return View(db.Clothes.ToList());

            }
           
        }

        // POST: StudentClothes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult UpdateItems(string searchText, string Code)
        {
            string[] words = searchText.Split('&');
            int i = 0;
            decimal total = 0;
            foreach (var itemm in db.Clothes)
            {
                string[] wordd = words[i].Split('=');
                System.Diagnostics.Debug.WriteLine(wordd[0] + "   " + wordd[1]);
                if (Int32.Parse(wordd[1]) > 0)
                {
                    StudentClothe studentClothe = new StudentClothe();
                    studentClothe.StdCode = Int32.Parse(Code);
                    studentClothe.ClothesID = itemm.ClothesID;
                    studentClothe.Quantity = wordd[1];
                    studentClothe.Price = (Int32.Parse(wordd[1]) * Int32.Parse(itemm.ClothesPrice)).ToString();
                    total += (Int32.Parse(wordd[1]) * Int32.Parse(itemm.ClothesPrice));
                    studentClothe.PaymentStatus = "True";
                    studentClothe.ReceivingStatus = "False";
                    studentClothe.ReceivingQuantity = "0";
                    db.StudentClothes.Add(studentClothe);
                }
                i++;
                
            }
                invoice_payment invoice = new invoice_payment();
                invoice.student = Int32.Parse(Code);
                int st = invoice.student;
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
                short studentGrade = (short)stud.StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().GradeID;


                string year = stud.StudentGradesHistories.LastOrDefault().StudyYear;
                //get items you pay for except uniform
                var x = db.getPreviousPayment(st).FirstOrDefault();
                if (x != null)
                    invoice.previous_payment = x.id;
                invoice_payment prev = db.invoice_payment.Find(invoice.previous_payment);
                invoice.date = DateTime.Now;
                invoice.invoice_payment2 = prev;
                if (x != null)
                    invoice.Notes = prev.Notes;
                invoice.student = st;
                if (x != null)
                    invoice.remaining = prev.remaining;

                payment_details item = new payment_details();
                //string newNotes = "\r\n";
                invoice.StudentsMain = stud;
                invoice.date = DateTime.Now;
                item.type = 5;
                item.name = "uniform";
                item.selected = true;
                item.school = stud.StdSchoolID;
                item.Grade = (short)studentGrade;
                item.year = year;
                item.student_type = studentType;
                item.amount = total;
                invoice.paid += item.amount;
                //newNotes += item.name; ;
                invoice.payment_details.Add(item);
                if (invoice.Notes == null)
                    invoice.Notes = "";
                    db.payment_details.Add(item);
                    db.SaveChanges();
                    db.invoice_payment.Add(invoice);
                    db.SaveChanges();
                    //invoice.total_cost += item.amount;
                    //invoice.Notes = invoice.Notes + invoice.id + newNotes;
                    //db.Entry(invoice).State = EntityState.Modified;
                    //db.SaveChanges();

            db.SaveChanges();
            return Json(new { code = 1 });
        }

        public JsonResult UpdateItemsReceived(string searchText, string Code)
        {

            string[] words = searchText.Split('&');
            int i = 0;
            int Codee = Int32.Parse(Code);
            foreach (var item in db.StudentClothes.Where(a => a.StdCode == Codee).ToList())
            {
                if (item.ReceivingStatus.Equals("False"))
                {
                    string[] wordd = words[i].Split('=');
                    System.Diagnostics.Debug.WriteLine(wordd[0] + "   " + wordd[1]);
                    int ReQ = Int32.Parse(wordd[1]);
                    int Q = Int32.Parse(item.Quantity);

                    if (Q > ReQ)
                    {
                        StudentClothe studentClothe = db.StudentClothes.Find(item.SCID);
                        studentClothe.ReceivingQuantity = wordd[1];
                        studentClothe.ReceivingStatus = "False";
                    }
                    if (Q == ReQ)
                    {
                        StudentClothe studentClothe = db.StudentClothes.Find(item.SCID);
                        studentClothe.ReceivingQuantity = wordd[1];
                        studentClothe.ReceivingStatus = "True";
                    }
                    i++;
                }
            }              
            

            db.SaveChanges();
            return Json(new { code = 1 });
        }
        // GET: StudentClothes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentClothe studentClothe = db.StudentClothes.Find(id);
            if (studentClothe == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClothesID = new SelectList(db.Clothes, "ClothesID", "ClothesName", studentClothe.ClothesID);
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", studentClothe.StdCode);
            return View(studentClothe);
        }

        // POST: StudentClothes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SCID,StdCode,ClothesID,Quantity,Price,ReceivingStatus,PaymentStatus")] StudentClothe studentClothe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentClothe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClothesID = new SelectList(db.Clothes, "ClothesID", "ClothesName", studentClothe.ClothesID);
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", studentClothe.StdCode);
            return View(studentClothe);
        }

        // GET: StudentClothes/Delete/5
        public ActionResult Received(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.StdName = db.StudentsMains.Find(id).StdEnglishFristName + " " + db.StudentsMains.Find(id).StdEnglishMiddleName + " " + db.StudentsMains.Find(id).StdEnglishLastName + " " + db.StudentsMains.Find(id).StdEnglishFamilyName;
                ViewBag.StdSchool = db.StudentsMains.Find(id).NESSchool.SchoolName;
                ViewBag.StdGrade = db.StudentsMains.Find(id).StudentGradesHistories.LastOrDefault().Grade.GradeName;
                ViewBag.StdClass = db.StudentsMains.Find(id).Class.ClassName;
                ViewBag.StdCode = id;

                return View(db.StudentClothes.Where(a=>a.StdCode == id).ToList());

            }

        }

        // POST: StudentClothes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentClothe studentClothe = db.StudentClothes.Find(id);
            db.StudentClothes.Remove(studentClothe);
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
