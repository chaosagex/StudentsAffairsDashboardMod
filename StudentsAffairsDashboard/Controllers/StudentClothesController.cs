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
            //var studentClothes = db.StudentClothes.Include(s => s.Cloth).Include(s => s.StudentsMain);
            //return View(studentClothes.ToList());
            var studentsMains = db.StudentsMains.Include(s => s.Class).Include(s => s.NESSchool).Include(s => s.StudentAccount);
            ViewBag.ClassID = new SelectList(db.Classes, "ClassID", "ClassName");
            ViewBag.SchoolID = new SelectList(db.NESSchools, "SchoolID", "SchoolName");
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName");
            return View(studentsMains.ToList());
        }

        // GET: StudentClothes/Details/5
        public ActionResult Details(int? id)
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
            return View(studentClothe);
        }

        // GET: StudentClothes/Create
        public ActionResult Create(int? id)
        {
            //ViewBag.ClothesID = new SelectList(db.Clothes, "ClothesID", "ClothesName");
            //ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName" , id);
            return View(db.Clothes.ToList());
        }

        // POST: StudentClothes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SCID,StdCode,ClothesID,Quantity,Price,ReceivingStatus,PaymentStatus")] StudentClothe studentClothe)
        {
            if (ModelState.IsValid)
            {
                db.StudentClothes.Add(studentClothe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClothesID = new SelectList(db.Clothes, "ClothesID", "ClothesName", studentClothe.ClothesID);
            ViewBag.StdCode = new SelectList(db.StudentsMains, "StdCode", "StdArabicFristName", studentClothe.StdCode);
            return View(studentClothe);
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
        public ActionResult Delete(int? id)
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
            return View(studentClothe);
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
