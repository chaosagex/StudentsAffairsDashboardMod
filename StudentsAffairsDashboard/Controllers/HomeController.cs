using StudentsAffairsDashboard.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using System.Reflection;
using System.Data.SqlClient;

namespace StudentsAffairsDashboard.Controllers
{
    
    public class HomeController : Controller
    {
        private StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();

        
        public ActionResult Index()
        {
            return View(db.NESSchools.ToList());
        }

        [HttpPost]
        public ActionResult Index(string customerName)
        {
            StudentAffairsDatabaseEntities entities = new StudentAffairsDatabaseEntities();
            var searchResult = entities.StudentsMains.Where(
                a=>a.StdEnglishFristName.Contains(customerName)
                || a.StdEnglishLastName.Contains(customerName)
                || a.StdEnglishMiddleName.Contains(customerName)
                || a.StdEnglishFamilyName.Contains(customerName)
                || a.StdArabicFristName.Contains(customerName)
                || a.StdArabicLastName.Contains(customerName)
                || a.StdArabicMiddleName.Contains(customerName)
                || a.StdArabicFamilyName.Contains(customerName)).ToList();
            return View("Search", searchResult);
        }

        public ActionResult Indexx(string SchoolID,string GradeID,string ClassID)
        {
            Debug.Print(SchoolID+GradeID+ClassID);
            int School = Int32.Parse(SchoolID);
            int Class = Int32.Parse(ClassID);
            int Grade = Int32.Parse(GradeID);


            var ListNESSchools = db.NESSchools.ToList();
            ListNESSchools.Add(new NESSchool() { SchoolID = -1, SchoolName = "All", SchooleAbbreviation = "All" });
            var NESSchoolsResult = ListNESSchools.OrderBy(d => d.SchoolID).ToList();
            ViewBag.SchoolID = new SelectList(NESSchoolsResult, "SchoolID", "SchoolName");

            var ListClasses = db.Classes.ToList();
            ListClasses.Add(new Class() { ClassID = -1, ClassName = "All" });
            var ClassesResult = ListClasses.OrderBy(d => d.ClassID).ToList();
            ViewBag.ClassID = new SelectList(ClassesResult, "ClassID", "ClassName");

            var ListGrades = db.Grades.ToList();
            ListGrades.Add(new Grade() { GradeID = -1, GradeName = "All" });
            var GradesResult = ListGrades.OrderBy(d => d.GradeID).ToList();
            ViewBag.GradeID = new SelectList(GradesResult, "GradeID", "GradeName");
            var searchResult = db.StudentGradesHistories.ToList();

            if (School == -1)
            {
                if (Grade == -1)
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).ToList();
                    }
                }
                else 
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.GradeID == Grade).ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.GradeID == Grade).ToList();
                    }
                }
            }
            else
            {
                if (Grade == -1)
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                }
                else 
                {
                    if (Class == -1)
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.GradeID == Grade).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                    else
                    {
                        searchResult = db.StudentGradesHistories.Where(a => a.StudentsMain.StdClassID == Class).Where(a => a.GradeID == Grade).Where(a => a.StudentsMain.NESSchool.SchoolID == School).ToList();
                    }
                }
            }
            
            //.Where(a => a.NESSchool.SchoolID == School)
            //.Where(a => a.StdClassID == Class).ToList();

            return View("Search", searchResult);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult Login()
        {
            Session["Email"] = null;
            Session["UserName"] = null;
            Session["CurrentSchool"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Login(Account objUser)
        {
            
            if (ModelState.IsValid)
            {
                using (StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities())
                {
                    var obj = db.Accounts.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Email"] = obj.Email.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                        Session["CurrentSchool"] = obj.School.ToString();
                        LogsController logs = new LogsController();
                        DateTime now = DateTime.Now;
                        Log log = new Log();
                        log.UserName = Session["UserName"].ToString();
                        log.Times = now.ToString();
                        log.LogContent = "Login : Account (" + Session["UserName"].ToString() + ") login School Managment System";
                        bool checklog = logs.Create(log);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            else
            {
                LogsController logs = new LogsController();
                DateTime now = DateTime.Now;
                Log log = new Log();
                log.UserName = Session["UserName"].ToString();
                log.Times = now.ToString();
                log.LogContent = "Logout : Account (" + Session["UserName"].ToString() + ") logout  School Managment System";
                bool checklog = logs.Create(log);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
        }

        //Excel Section
        // GET: Excel  
        public ActionResult IndexImport()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ImportFile(HttpPostedFileBase importFile)
        {
            if (importFile == null) return Json(new { Status = 0, Message = "No File Selected" });

            try
            {
                var fileData = GetDataFromCSVFile(importFile.InputStream);

                var dtEmployee = fileData.ToDataTable();
                var tblEmployeeParameter = new SqlParameter("@ParSubjectsType", SqlDbType.Structured)
                {
                    TypeName = "dbo.Subjects",
                    Value = dtEmployee
                };
                await db.Database.ExecuteSqlCommandAsync("EXEC UpdateDataFromExcel @ParSubjectsType", tblEmployeeParameter);
                return Json(new { Status = 1, Message = "File Imported Successfully " });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }
        private List<Subject> GetDataFromCSVFile(Stream stream)
        {
            var empList = new List<Subject>();
            try
            {
                //using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                //{
                //    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                //    {
                //        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                //        {
                //            UseHeaderRow = true // To set First Row As Column Names    
                //        }
                //    });

                //    if (dataSet.Tables.Count > 0)
                //    {
                //        var dataTable = dataSet.Tables[0];
                //        foreach (DataRow objDataRow in dataTable.Rows)
                //        {
                //            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;
                //            empList.Add(new Subject()
                //            {
                //                SubjectID = Convert.ToInt32(objDataRow["SubjectID"].ToString()),
                //                SubjectCode = objDataRow["SubjectCode"].ToString(),
                //                SubjectName = objDataRow["SubjectName"].ToString(),
                //                SubjectFees = Convert.ToInt32(objDataRow["SubjectFees"].ToString()),
                //            });
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return empList;
        }



    }
}
public static class Extensions
{
    public static DataTable ToDataTable<T>(this List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties  
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Defining type of data column gives proper data table   
            var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
            //Setting column names as Property names  
            dataTable.Columns.Add(prop.Name, type);
        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
                //inserting property values to datatable rows  
                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }
        return dataTable;
    }
}