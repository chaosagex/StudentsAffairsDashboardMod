﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentsAffairsDashboard.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class StudentAffairsDatabaseEntities : DbContext
    {
        public StudentAffairsDatabaseEntities()
            : base("name=StudentAffairsDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<NESSchool> NESSchools { get; set; }
        public virtual DbSet<StudentAccount> StudentAccounts { get; set; }
        public virtual DbSet<StudentGradesHistory> StudentGradesHistories { get; set; }
        public virtual DbSet<StudentsMain> StudentsMains { get; set; }
        public virtual DbSet<StudentSubjectEnroll> StudentSubjectEnrolls { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<StudentClothe> StudentClothes { get; set; }
        public virtual DbSet<invoice_payment> invoice_payment { get; set; }
        public virtual DbSet<payment_details> payment_details { get; set; }
        public virtual DbSet<Cloth> Clothes { get; set; }
        public virtual DbSet<PackageClothe> PackageClothes { get; set; }
        public virtual DbSet<Machine> Machines { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<StudentsMainCustom> StudentsMainCustoms { get; set; }
        
    
        public virtual int deleteInvoice(Nullable<int> invoiceID)
        {
            var invoiceIDParameter = invoiceID.HasValue ?
                new ObjectParameter("invoiceID", invoiceID) :
                new ObjectParameter("invoiceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("deleteInvoice", invoiceIDParameter);
        }
    
        public virtual int deleteInvoiceItem(Nullable<int> itemID, Nullable<int> invoiceID)
        {
            var itemIDParameter = itemID.HasValue ?
                new ObjectParameter("itemID", itemID) :
                new ObjectParameter("itemID", typeof(int));
    
            var invoiceIDParameter = invoiceID.HasValue ?
                new ObjectParameter("invoiceID", invoiceID) :
                new ObjectParameter("invoiceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("deleteInvoiceItem", itemIDParameter, invoiceIDParameter);
        }
    
        public virtual int deletePayment(Nullable<int> paymentID)
        {
            var paymentIDParameter = paymentID.HasValue ?
                new ObjectParameter("paymentID", paymentID) :
                new ObjectParameter("paymentID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("deletePayment", paymentIDParameter);
        }
    
        public virtual ObjectResult<getInvoiceById_Result> getInvoiceById(Nullable<int> invoiceId)
        {
            var invoiceIdParameter = invoiceId.HasValue ?
                new ObjectParameter("invoiceId", invoiceId) :
                new ObjectParameter("invoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getInvoiceById_Result>("getInvoiceById", invoiceIdParameter);
        }
    
        public virtual ObjectResult<getInvoiceItem_Result> getInvoiceItem(Nullable<int> itemID, Nullable<int> invoiceID)
        {
            var itemIDParameter = itemID.HasValue ?
                new ObjectParameter("itemID", itemID) :
                new ObjectParameter("itemID", typeof(int));
    
            var invoiceIDParameter = invoiceID.HasValue ?
                new ObjectParameter("invoiceID", invoiceID) :
                new ObjectParameter("invoiceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getInvoiceItem_Result>("getInvoiceItem", itemIDParameter, invoiceIDParameter);
        }
    
        public virtual ObjectResult<getInvoicesById_Result> getInvoicesById(Nullable<int> invoiceId)
        {
            var invoiceIdParameter = invoiceId.HasValue ?
                new ObjectParameter("invoiceId", invoiceId) :
                new ObjectParameter("invoiceId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getInvoicesById_Result>("getInvoicesById", invoiceIdParameter);
        }
    
        public virtual ObjectResult<getPayment_Result> getPayment(string paymentSchool, Nullable<System.DateTime> paymentYear, Nullable<short> paymentStudentType)
        {
            var paymentSchoolParameter = paymentSchool != null ?
                new ObjectParameter("paymentSchool", paymentSchool) :
                new ObjectParameter("paymentSchool", typeof(string));
    
            var paymentYearParameter = paymentYear.HasValue ?
                new ObjectParameter("paymentYear", paymentYear) :
                new ObjectParameter("paymentYear", typeof(System.DateTime));
    
            var paymentStudentTypeParameter = paymentStudentType.HasValue ?
                new ObjectParameter("paymentStudentType", paymentStudentType) :
                new ObjectParameter("paymentStudentType", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getPayment_Result>("getPayment", paymentSchoolParameter, paymentYearParameter, paymentStudentTypeParameter);
        }
    
        public virtual ObjectResult<getPaymentById_Result> getPaymentById(Nullable<int> paymentID)
        {
            var paymentIDParameter = paymentID.HasValue ?
                new ObjectParameter("paymentID", paymentID) :
                new ObjectParameter("paymentID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getPaymentById_Result>("getPaymentById", paymentIDParameter);
        }
    
        public virtual ObjectResult<getPaymentItems_Result> getPaymentItems(Nullable<int> school, Nullable<short> student_type, Nullable<short> grade)
        {
            var schoolParameter = school.HasValue ?
                new ObjectParameter("school", school) :
                new ObjectParameter("school", typeof(int));
    
            var student_typeParameter = student_type.HasValue ?
                new ObjectParameter("student_type", student_type) :
                new ObjectParameter("student_type", typeof(short));
    
            var gradeParameter = grade.HasValue ?
                new ObjectParameter("grade", grade) :
                new ObjectParameter("grade", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getPaymentItems_Result>("getPaymentItems", schoolParameter, student_typeParameter, gradeParameter);
        }
    
        public virtual ObjectResult<getPreviousPayment_Result> getPreviousPayment(Nullable<int> student)
        {
            var studentParameter = student.HasValue ?
                new ObjectParameter("student", student) :
                new ObjectParameter("student", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getPreviousPayment_Result>("getPreviousPayment", studentParameter);
        }
    
        public virtual int insertInvoiceItem(Nullable<int> itemID, Nullable<int> invoiceID)
        {
            var itemIDParameter = itemID.HasValue ?
                new ObjectParameter("itemID", itemID) :
                new ObjectParameter("itemID", typeof(int));
    
            var invoiceIDParameter = invoiceID.HasValue ?
                new ObjectParameter("invoiceID", invoiceID) :
                new ObjectParameter("invoiceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insertInvoiceItem", itemIDParameter, invoiceIDParameter);
        }
    
        public virtual int insertPaymentDetail(string paymentName, Nullable<int> paymentType, Nullable<decimal> paymentAmount, string paymentSchool, Nullable<System.DateTime> paymentYear, Nullable<short> paymentStudentType)
        {
            var paymentNameParameter = paymentName != null ?
                new ObjectParameter("paymentName", paymentName) :
                new ObjectParameter("paymentName", typeof(string));
    
            var paymentTypeParameter = paymentType.HasValue ?
                new ObjectParameter("paymentType", paymentType) :
                new ObjectParameter("paymentType", typeof(int));
    
            var paymentAmountParameter = paymentAmount.HasValue ?
                new ObjectParameter("paymentAmount", paymentAmount) :
                new ObjectParameter("paymentAmount", typeof(decimal));
    
            var paymentSchoolParameter = paymentSchool != null ?
                new ObjectParameter("paymentSchool", paymentSchool) :
                new ObjectParameter("paymentSchool", typeof(string));
    
            var paymentYearParameter = paymentYear.HasValue ?
                new ObjectParameter("paymentYear", paymentYear) :
                new ObjectParameter("paymentYear", typeof(System.DateTime));
    
            var paymentStudentTypeParameter = paymentStudentType.HasValue ?
                new ObjectParameter("paymentStudentType", paymentStudentType) :
                new ObjectParameter("paymentStudentType", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insertPaymentDetail", paymentNameParameter, paymentTypeParameter, paymentAmountParameter, paymentSchoolParameter, paymentYearParameter, paymentStudentTypeParameter);
        }
    
        public virtual int insertStudentFees(Nullable<int> feeActivity, Nullable<int> feeResources, Nullable<int> feeFirstInstallment, Nullable<int> feeSecondInstallment, Nullable<int> feeUniform, Nullable<int> feeTotalFees, Nullable<System.DateTime> feeYear)
        {
            var feeActivityParameter = feeActivity.HasValue ?
                new ObjectParameter("feeActivity", feeActivity) :
                new ObjectParameter("feeActivity", typeof(int));
    
            var feeResourcesParameter = feeResources.HasValue ?
                new ObjectParameter("feeResources", feeResources) :
                new ObjectParameter("feeResources", typeof(int));
    
            var feeFirstInstallmentParameter = feeFirstInstallment.HasValue ?
                new ObjectParameter("feeFirstInstallment", feeFirstInstallment) :
                new ObjectParameter("feeFirstInstallment", typeof(int));
    
            var feeSecondInstallmentParameter = feeSecondInstallment.HasValue ?
                new ObjectParameter("feeSecondInstallment", feeSecondInstallment) :
                new ObjectParameter("feeSecondInstallment", typeof(int));
    
            var feeUniformParameter = feeUniform.HasValue ?
                new ObjectParameter("feeUniform", feeUniform) :
                new ObjectParameter("feeUniform", typeof(int));
    
            var feeTotalFeesParameter = feeTotalFees.HasValue ?
                new ObjectParameter("feeTotalFees", feeTotalFees) :
                new ObjectParameter("feeTotalFees", typeof(int));
    
            var feeYearParameter = feeYear.HasValue ?
                new ObjectParameter("feeYear", feeYear) :
                new ObjectParameter("feeYear", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insertStudentFees", feeActivityParameter, feeResourcesParameter, feeFirstInstallmentParameter, feeSecondInstallmentParameter, feeUniformParameter, feeTotalFeesParameter, feeYearParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int updatePaymentDetail(Nullable<int> paymentId, string paymentName, Nullable<int> paymentType, Nullable<decimal> paymentAmount, string paymentSchool, Nullable<System.DateTime> paymentYear, Nullable<short> paymentStudentType)
        {
            var paymentIdParameter = paymentId.HasValue ?
                new ObjectParameter("paymentId", paymentId) :
                new ObjectParameter("paymentId", typeof(int));
    
            var paymentNameParameter = paymentName != null ?
                new ObjectParameter("paymentName", paymentName) :
                new ObjectParameter("paymentName", typeof(string));
    
            var paymentTypeParameter = paymentType.HasValue ?
                new ObjectParameter("paymentType", paymentType) :
                new ObjectParameter("paymentType", typeof(int));
    
            var paymentAmountParameter = paymentAmount.HasValue ?
                new ObjectParameter("paymentAmount", paymentAmount) :
                new ObjectParameter("paymentAmount", typeof(decimal));
    
            var paymentSchoolParameter = paymentSchool != null ?
                new ObjectParameter("paymentSchool", paymentSchool) :
                new ObjectParameter("paymentSchool", typeof(string));
    
            var paymentYearParameter = paymentYear.HasValue ?
                new ObjectParameter("paymentYear", paymentYear) :
                new ObjectParameter("paymentYear", typeof(System.DateTime));
    
            var paymentStudentTypeParameter = paymentStudentType.HasValue ?
                new ObjectParameter("paymentStudentType", paymentStudentType) :
                new ObjectParameter("paymentStudentType", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updatePaymentDetail", paymentIdParameter, paymentNameParameter, paymentTypeParameter, paymentAmountParameter, paymentSchoolParameter, paymentYearParameter, paymentStudentTypeParameter);
        }
    
        public virtual ObjectResult<getUniformStudent_Result> getUniformStudent(Nullable<int> stdcode)
        {
            var stdcodeParameter = stdcode.HasValue ?
                new ObjectParameter("stdcode", stdcode) :
                new ObjectParameter("stdcode", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getUniformStudent_Result>("getUniformStudent", stdcodeParameter);
        }
    
        public virtual ObjectResult<getSchoolFullyOrPartial_Result> getSchoolFullyOrPartial(Nullable<int> school)
        {
            var schoolParameter = school.HasValue ?
                new ObjectParameter("School", school) :
                new ObjectParameter("School", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getSchoolFullyOrPartial_Result>("getSchoolFullyOrPartial", schoolParameter);
        }
    
        public virtual ObjectResult<getSchoolFullyRecieved_Result> getSchoolFullyRecieved(Nullable<int> school)
        {
            var schoolParameter = school.HasValue ?
                new ObjectParameter("School", school) :
                new ObjectParameter("School", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getSchoolFullyRecieved_Result>("getSchoolFullyRecieved", schoolParameter);
        }
    
        public virtual ObjectResult<getSchoolInvoiceReport_Result> getSchoolInvoiceReport(Nullable<int> school)
        {
            var schoolParameter = school.HasValue ?
                new ObjectParameter("School", school) :
                new ObjectParameter("School", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getSchoolInvoiceReport_Result>("getSchoolInvoiceReport", schoolParameter);
        }
    
        public virtual ObjectResult<getSchoolNotRecieved_Result> getSchoolNotRecieved(Nullable<int> school)
        {
            var schoolParameter = school.HasValue ?
                new ObjectParameter("School", school) :
                new ObjectParameter("School", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getSchoolNotRecieved_Result>("getSchoolNotRecieved", schoolParameter);
        }
    
        public virtual ObjectResult<getUniformByInvoice_Result> getUniformByInvoice(Nullable<int> invoice)
        {
            var invoiceParameter = invoice.HasValue ?
                new ObjectParameter("invoice", invoice) :
                new ObjectParameter("invoice", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getUniformByInvoice_Result>("getUniformByInvoice", invoiceParameter);
        }
    }
}
