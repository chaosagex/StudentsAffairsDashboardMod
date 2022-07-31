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
            int SchoolIDsession = Int32.Parse(Session["CurrentSchool"].ToString());
            if (Session["CurrentSchool"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                
                if (SchoolIDsession == 1000)
                {
                    var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount);
                    return View(studentsMains.ToList());
                }
                else
                {
                    var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount).Where(a => a.NESSchool.SchoolID == SchoolIDsession);
                    return View(studentsMains.ToList());
                }
                
            }
        }

        // GET: StudentClothes/Receiving
        public ActionResult Receiving()
        {
            var studentClothes = db.StudentClothes.Include(s => s.Cloth).Include(s => s.StudentsMain).GroupBy(a => a.StdCode).Select(a => a.FirstOrDefault());
            List<StudentsMain> All = new List<StudentsMain>();

            foreach (var item in studentClothes)
            {
                if (!db.StudentClothes.Where(a => a.StdCode == item.StdCode).All(a => a.ReceivingStatus == "True"))
                {
                    All.Add(db.StudentsMains.Find(item.StdCode));
                }
            }
            return View(All);
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
                ViewBag.StdGrade = db.StudentsMains.Find(id).StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().Grade.GradeName;
                ViewBag.StdGradeID = db.StudentsMains.Find(id).StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().Grade.GradeID.ToString();
                ViewBag.StdClass = db.StudentsMains.Find(id).Class.ClassName;
                ViewBag.StdCode = id;

                ViewBag.P_KG12Boy = db.PackageClothes.Where(a => a.PackageName == "KGBoy").Select(a => a.PackagePrice).FirstOrDefault();
                ViewBag.P_Grade16Boy = db.PackageClothes.Where(a => a.PackageName == "PrimaryBoyGirl").Select(a => a.PackagePrice).FirstOrDefault();
                ViewBag.P_Grade79Boy = db.PackageClothes.Where(a => a.PackageName == "PreparatoryBoyGirl").Select(a => a.PackagePrice).FirstOrDefault();
                ViewBag.P_Grade1012Boy = db.PackageClothes.Where(a => a.PackageName == "SecondaryBoyGirl").Select(a => a.PackagePrice).FirstOrDefault();

                ViewBag.P_KG12Girl = db.PackageClothes.Where(a => a.PackageName == "KGGirl").Select(a => a.PackagePrice).FirstOrDefault();
                ViewBag.P_Grade16Girl = db.PackageClothes.Where(a => a.PackageName == "PrimaryBoyGirl").Select(a => a.PackagePrice).FirstOrDefault();
                ViewBag.P_Grade79Girl = db.PackageClothes.Where(a => a.PackageName == "PreparatoryBoyGirl").Select(a => a.PackagePrice).FirstOrDefault();
                ViewBag.P_Grade1012Girl = db.PackageClothes.Where(a => a.PackageName == "SecondaryBoyGirl").Select(a => a.PackagePrice).FirstOrDefault();

                return View(db.Clothes.ToList());

            }

        }

        // POST: StudentClothes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult UpdateItems(string searchText, string Code, string packageName)
        {
            string[] words = searchText.Split('&');
            //int i = 0;
            double total = 0;
            List<StudentClothe> ItemsPackage = new List<StudentClothe>();
            if (packageName == "KGBoy")
            {
                foreach (var itemm in db.Clothes)
                {
                    if (itemm.ClothesID == 8
                        || itemm.ClothesID == 2
                        || itemm.ClothesID == 4
                        || itemm.ClothesID == 5
                        || itemm.ClothesID == 6
                        || itemm.ClothesID == 7)
                    {
                        StudentClothe studentClothe = new StudentClothe();
                        studentClothe.StdCode = Int32.Parse(Code);
                        studentClothe.ClothesID = itemm.ClothesID;
                        studentClothe.Quantity = "1";
                        studentClothe.Price = Double.Parse(itemm.ClothesinPackagePrice).ToString();
                        total += Double.Parse(itemm.ClothesinPackagePrice);
                        studentClothe.PaymentStatus = "True";
                        studentClothe.ReceivingStatus = "False";
                        studentClothe.ReceivingQuantity = "0";
                        studentClothe.PackageStatus = "InPackage";
                        ItemsPackage.Add(studentClothe);
                    }

                }
            }
            if (packageName == "KGGirl")
            {
                foreach (var itemm in db.Clothes)
                {
                    if (itemm.ClothesID == 1
                        || itemm.ClothesID == 2
                        || itemm.ClothesID == 3
                        || itemm.ClothesID == 5
                        || itemm.ClothesID == 6
                        || itemm.ClothesID == 7)
                    {
                        StudentClothe studentClothe = new StudentClothe();
                        studentClothe.StdCode = Int32.Parse(Code);
                        studentClothe.ClothesID = itemm.ClothesID;
                        studentClothe.Quantity = "1";
                        studentClothe.Price = Double.Parse(itemm.ClothesinPackagePrice).ToString();
                        total += Double.Parse(itemm.ClothesinPackagePrice);
                        studentClothe.PaymentStatus = "True";
                        studentClothe.ReceivingStatus = "False";
                        studentClothe.ReceivingQuantity = "0";
                        studentClothe.PackageStatus = "InPackage";
                        ItemsPackage.Add(studentClothe);
                    }

                }
            }
            if (packageName == "PrimaryBoyGirl")
            {
                foreach (var itemm in db.Clothes)
                {
                    if (itemm.ClothesID == 9
                        || itemm.ClothesID == 10
                        || itemm.ClothesID == 11
                        || itemm.ClothesID == 12
                        || itemm.ClothesID == 13
                        || itemm.ClothesID == 14)
                    {
                        StudentClothe studentClothe = new StudentClothe();
                        studentClothe.StdCode = Int32.Parse(Code);
                        studentClothe.ClothesID = itemm.ClothesID;
                        studentClothe.Quantity = "1";
                        studentClothe.Price = Double.Parse(itemm.ClothesinPackagePrice).ToString();
                        total += Double.Parse(itemm.ClothesinPackagePrice);
                        studentClothe.PaymentStatus = "True";
                        studentClothe.ReceivingStatus = "False";
                        studentClothe.ReceivingQuantity = "0";
                        studentClothe.PackageStatus = "InPackage";
                        ItemsPackage.Add(studentClothe);
                    }

                }
            }
            if (packageName == "PreparatoryBoyGirl")
            {
                foreach (var itemm in db.Clothes)
                {
                    if (itemm.ClothesID == 15
                        || itemm.ClothesID == 16
                        || itemm.ClothesID == 17
                        || itemm.ClothesID == 18
                        || itemm.ClothesID == 19
                        || itemm.ClothesID == 20)
                    {
                        StudentClothe studentClothe = new StudentClothe();
                        studentClothe.StdCode = Int32.Parse(Code);
                        studentClothe.ClothesID = itemm.ClothesID;
                        studentClothe.Quantity = "1";
                        studentClothe.Price = Double.Parse(itemm.ClothesinPackagePrice).ToString();
                        total += Double.Parse(itemm.ClothesinPackagePrice);
                        studentClothe.PaymentStatus = "True";
                        studentClothe.ReceivingStatus = "False";
                        studentClothe.ReceivingQuantity = "0";
                        studentClothe.PackageStatus = "InPackage";
                        ItemsPackage.Add(studentClothe);
                    }

                }
            }
            if (packageName == "SecondaryBoyGirl")
            {
                foreach (var itemm in db.Clothes)
                {
                    if (itemm.ClothesID == 25
                        || itemm.ClothesID == 26
                        || itemm.ClothesID == 27
                        || itemm.ClothesID == 28
                        || itemm.ClothesID == 29
                        || itemm.ClothesID == 30)
                    {
                        StudentClothe studentClothe = new StudentClothe();
                        studentClothe.StdCode = Int32.Parse(Code);
                        studentClothe.ClothesID = itemm.ClothesID;
                        studentClothe.Quantity = "1";
                        studentClothe.Price = itemm.ClothesinPackagePrice;
                        total += Double.Parse(itemm.ClothesinPackagePrice);
                        studentClothe.PaymentStatus = "True";
                        studentClothe.ReceivingStatus = "False";
                        studentClothe.ReceivingQuantity = "0";
                        studentClothe.PackageStatus = "InPackage";
                        ItemsPackage.Add(studentClothe);
                    }

                }
            }



            for (int i = 0; i < words.Length; i++)
            {
                string[] wordd = words[i].Split('=');
                System.Diagnostics.Debug.WriteLine(wordd[0] + "   " + wordd[1]);
                if (Int32.Parse(wordd[1]) > 0)
                {
                    int CID = Int32.Parse(wordd[0]);
                    StudentClothe studentClothe = new StudentClothe();
                    studentClothe.StdCode = Int32.Parse(Code);
                    studentClothe.ClothesID = db.Clothes.Find(CID).ClothesID;
                    studentClothe.Quantity = wordd[1];
                    studentClothe.Price = (Double.Parse(wordd[1]) * Double.Parse(db.Clothes.Find(CID).ClothesPrice)).ToString();
                    total += (Double.Parse(wordd[1]) * Double.Parse(db.Clothes.Find(CID).ClothesPrice));
                    studentClothe.PaymentStatus = "True";
                    studentClothe.ReceivingStatus = "False";
                    studentClothe.ReceivingQuantity = "0";
                    studentClothe.PackageStatus = "OutPackage";
                    ItemsPackage.Add(studentClothe);

                }
            }
            if (total != 0)
            {
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
                int studentGrade = stud.StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().GradeID;


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
                item.Grade = studentGrade;
                item.year = year;
                item.student_type = studentType;
                item.amount = (decimal)(total);
                invoice.paid += item.amount;
                //newNotes += item.name; ;
                invoice.payment_details.Add(item);
                invoice.total_cost = item.amount;
                invoice.type = 1; // POS

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

                foreach (var items in ItemsPackage)
                {
                    items.InvoiceID = invoice.id;
                    db.StudentClothes.Add(items);
                }
                db.SaveChanges();
                LogsController logs = new LogsController();
                DateTime now = DateTime.Now;
                Log log = new Log();
                log.UserName = Session["UserName"].ToString();
                log.Times = now.ToString();
                log.LogContent = "Uniform : Account (" + Session["UserName"].ToString() + ") Received Student Uniform With ID: (" + Code + ") and invoice with Total Amount = " + item.amount + " EGP";
                bool checklog = logs.Create(log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult UpdateItemsReceived(string searchText, string Code)
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

            LogsController logs = new LogsController();
            DateTime now = DateTime.Now;
            Log log = new Log();
            log.UserName = Session["UserName"].ToString();
            log.Times = now.ToString();
            log.LogContent = "Uniform : Representative (" + Session["UserName"].ToString() + ") Student handed Pieces of Uniform With ID: (" + Code + ")";
            bool checklog = logs.Create(log);

            return RedirectToAction("Index");
        }
        // GET: StudentClothes/Edit/5
        public ActionResult UniformReport()
        {
            int SchoolIDsession = Int32.Parse(Session["CurrentSchool"].ToString());
            var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount).Where(a => a.NESSchool.SchoolID == SchoolIDsession);

            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName");
            var ListGrades = db.Grades.ToList();
            ListGrades.Add(new Grade() { GradeID = -1, GradeName = "All" });
            var GradesResult = ListGrades.OrderBy(d => d.GradeID).ToList();
            ViewBag.GradeID = new SelectList(GradesResult, "GradeID", "GradeName");

            return View(studentsMains.ToList());
        }
        public ActionResult UniformReportResult(string searchText, string GradeID)
        {
            var ListGrades = db.Grades.ToList();
            ListGrades.Add(new Grade() { GradeID = -1, GradeName = "All" });
            var GradesResult = ListGrades.OrderBy(d => d.GradeID).ToList();
            ViewBag.GradeID = new SelectList(GradesResult, "GradeID", "GradeName");
            String[] FilterData = new string[9];

            //FilterData[0] = "first%20installment";
            //FilterData[1] = "2nd%20installment";
            //FilterData[2] = "Activity";
            //FilterData[3] = "Resorce";
            //FilterData[4] = "Uniform";
            //FilterData[5] = "Year";
            //FilterData[6] = "From";
            //FilterData[7] = "To";
            //FilterData[8] = "GradeID";

            FilterData[0] = "0";
            FilterData[1] = "0";
            FilterData[2] = "0";
            FilterData[3] = "0";
            FilterData[4] = "0";
            FilterData[5] = "";
            FilterData[6] = "";
            FilterData[7] = "";
            FilterData[8] = "";
            string[] words = searchText.Split('&');
            for (int i = 0; i < words.Length; i++)
            {
                string[] wordd = words[i].Split('=');
                System.Diagnostics.Debug.WriteLine(wordd[0] + "   " + wordd[1]);
                if (wordd[0].Equals("first%20installment"))
                    FilterData[0] = "1";
                if (wordd[0].Equals("2nd%20installment"))
                    FilterData[1] = "2";
                if (wordd[0].Equals("Activity"))
                    FilterData[2] = "3";
                if (wordd[0].Equals("Resorce"))
                    FilterData[3] = "4";
                if (wordd[0].Equals("Uniform"))
                    FilterData[4] = "5";
                if (wordd[0].Equals("Year"))
                    FilterData[5] = wordd[1];
                if (wordd[0].Equals("From"))
                    FilterData[6] = wordd[1];
                if (wordd[0].Equals("To"))
                    FilterData[7] = wordd[1];
                if (wordd[0].Equals("GradeID"))
                    FilterData[8] = wordd[1];
            }
            List<payment_details> All = new List<payment_details>();
            List<invoice_payment> Dataa = new List<invoice_payment>();
            HashSet<StudentsMain> StudentData = new HashSet<StudentsMain>();
            string Year = FilterData[5];
            int ScID = Int32.Parse(Session["CurrentSchool"].ToString());
            if (FilterData[8].Equals("-1"))
            {
                if (!FilterData[5].Equals("0"))
                {
                    All = db.payment_details.Where(a => a.year.Equals(Year)).Where(a => a.school == ScID).ToList();
                }
            }
            else
            {
                int GrID = Int32.Parse(FilterData[8]);
                if (!FilterData[5].Equals("0"))
                {
                    All = db.payment_details.Where(a => a.year.Equals(Year)).Where(a => a.school == ScID).Where(a => a.Grade == GrID).ToList();
                }
            }
            

            

            foreach (payment_details item in All)
            {
                if (item.type == Int32.Parse(FilterData[0]))
                {
                    Dataa.AddRange(item.invoice_payment);
                }
                if (item.type == Int32.Parse(FilterData[1]))
                {
                    Dataa.AddRange(item.invoice_payment);
                }
                if (item.type == Int32.Parse(FilterData[2]))
                {
                    Dataa.AddRange(item.invoice_payment);
                }
                if (item.type == Int32.Parse(FilterData[3]))
                {
                    Dataa.AddRange(item.invoice_payment);
                }
                if (item.type == Int32.Parse(FilterData[4]))
                {
                    Dataa.AddRange(item.invoice_payment);
                }

            }

            foreach (var item in Dataa)
            {
                System.Diagnostics.Debug.WriteLine(item.id + "   ");
                if (item.date >= DateTime.Parse(FilterData[6]) && item.date <= DateTime.Parse(FilterData[7]))
                {
                    StudentData.Add(item.StudentsMain);
                }
            }


            //if (Int32.Parse(wordd[1]) > 0)
            //{
            //    StudentClothe studentClothe = new StudentClothe();
            //    studentClothe.StdCode = Int32.Parse(Code);
            //    studentClothe.ClothesID = itemm.ClothesID;
            //    studentClothe.Quantity = wordd[1];
            //    studentClothe.Price = (Int32.Parse(wordd[1]) * Int32.Parse(itemm.ClothesPrice)).ToString();
            //    total += (Int32.Parse(wordd[1]) * Int32.Parse(itemm.ClothesPrice));
            //    studentClothe.PaymentStatus = "True";
            //    studentClothe.ReceivingStatus = "False";
            //    studentClothe.ReceivingQuantity = "0";
            //    db.StudentClothes.Add(studentClothe);
            //}

            return View(StudentData.ToList());
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

                return View(db.StudentClothes.Where(a => a.StdCode == id).ToList());

            }

        }

        // POST: StudentClothes/Details/5

        [HttpPost]
        public ActionResult Details(string customerId)
        {
            int Code = Int32.Parse(customerId);
            ViewBag.StdName = db.StudentsMains.Find(Code).StdEnglishFristName + " " + db.StudentsMains.Find(Code).StdEnglishMiddleName + " " + db.StudentsMains.Find(Code).StdEnglishLastName + " " + db.StudentsMains.Find(Code).StdEnglishFamilyName;
            ViewBag.StdSchool = db.StudentsMains.Find(Code).NESSchool.SchoolName;
            ViewBag.StdGrade = db.StudentsMains.Find(Code).StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().Grade.GradeName;
            ViewBag.StdGradecustomerId = db.StudentsMains.Find(Code).StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().Grade.GradeID.ToString();
            ViewBag.StdClass = db.StudentsMains.Find(Code).Class.ClassName;

            var StudentsClothesData = db.StudentClothes.Where(a => a.StdCode == Code);
            ViewBag.Total = 0;
            double Total = 0;

            foreach (var item in StudentsClothesData.ToList())
            {
                Total += Double.Parse(item.Price);
            }
            ViewBag.Total = Total;
            return PartialView("Details", StudentsClothesData.ToList());
        }

        [HttpPost]
        public ActionResult DetailsReceipt(string customerId, string Typ)
        {
            int Code = Int32.Parse(customerId);
            ViewBag.StdName = db.StudentsMains.Find(Code).StdEnglishFristName + " " + db.StudentsMains.Find(Code).StdEnglishMiddleName + " " + db.StudentsMains.Find(Code).StdEnglishLastName + " " + db.StudentsMains.Find(Code).StdEnglishFamilyName;
            ViewBag.StdSchool = db.StudentsMains.Find(Code).NESSchool.SchoolName;
            ViewBag.StdGrade = db.StudentsMains.Find(Code).StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().Grade.GradeName;
            ViewBag.StdGradecustomerId = db.StudentsMains.Find(Code).StudentGradesHistories.OrderBy(a => a.GradeID).LastOrDefault().Grade.GradeID.ToString();
            ViewBag.StdClass = db.StudentsMains.Find(Code).Class.ClassName;

            var StudentsInvoicesData = db.invoice_payment.Where(a => a.student == Code);
            decimal TRemaing = StudentsInvoicesData.ToList().Last().remaining;

            ViewBag.TotalReminig = TRemaing;
            return PartialView("DetailsReceipt", StudentsInvoicesData.ToList());
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //List<StudentClothe> StudentClotheD = new List<StudentClothe>();
            //StudentClotheD = db.StudentClothes.Where(a => a.StdCode == id).ToList();
            //List<invoice_payment> StudentClotheD = new List<StudentClothe>();
            //StudentClotheD = db.StudentClothes.Where(a => a.StdCode == id).ToList();
            //List<StudentClothe> StudentClotheD = new List<StudentClothe>();
            //StudentClotheD = db.StudentClothes.Where(a => a.StdCode == id).ToList();
            //db.Grades.Remove(grade);
            //db.SaveChanges();
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
