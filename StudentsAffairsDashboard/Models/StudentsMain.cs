namespace StudentsAffairsDashboard.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class StudentsMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentsMain()
        {
            this.StudentGradesHistories = new HashSet<StudentGradesHistory>();
            this.StudentSubjectEnrolls = new HashSet<StudentSubjectEnroll>();
            this.StudentClothes = new HashSet<StudentClothe>();
            this.invoice_payment = new HashSet<invoice_payment>();
        }

        public int StdCode { get; set; }
        [RegularExpression("^[\u0621-\u064A]+$", ErrorMessage = "Only Arabic letters and no spaces")]
        public string StdArabicFristName { get; set; }
        [RegularExpression("^[\u0621-\u064A]+$", ErrorMessage = "Only Arabic letters and no spaces")]
        public string StdArabicMiddleName { get; set; }
        [RegularExpression("^[\u0621-\u064A]+$", ErrorMessage = "Only Arabic letters and no spaces")]
        public string StdArabicLastName { get; set; }
        [RegularExpression("^[\u0621-\u064A ]+$", ErrorMessage = "Only Arabic letters")]
        public string StdArabicFamilyName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only English letters and no spaces")]
        public string StdEnglishFristName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only English letters and no spaces")]
        public string StdEnglishMiddleName { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only English letters and no spaces")]
        public string StdEnglishLastName { get; set; }
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only English letters")]
        public string StdEnglishFamilyName { get; set; }

        [RegularExpression("^[\u0621-\u064A ]+$", ErrorMessage = "Only Arabic letters")]
        public string StdMotherArabicName { get; set; }
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only English letters")]
        public string StdMotherEnglishName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string StdFatherMobilePhone { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string StdMotherMobilePhone { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string StdFatherEmail { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string StdMotherEmail { get; set; }
        public string StdFatherNationality { get; set; }
        public string StdMotherNationality { get; set; }
        public string StdFatherSpokenLanguage { get; set; }
        public string StdMotherSpokenLanguage { get; set; }
        public string StdFatherJob { get; set; }
        public string StdMotherJob { get; set; }
        public string StdFatherQualification { get; set; }
        public string StdMotherQualification { get; set; }
        public string StdStudentsAffairs1 { get; set; }
        public string StdStudentsAffairs2 { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "You need to put the 14 National id numbers With No Spaces")]
        public string StdBirthCode { get; set; }
        public string StdAddressGov { get; set; }
        public string StdEmergencyContact { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string StdEmergencyPhone { get; set; }
        public Nullable<System.DateTime> StdBOD { get; set; }
        public string StdBirthPlace { get; set; }
        public string StdGender { get; set; }
        public string StdReligion { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "You need to put the 14 National id numbers With No Spaces")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "You need to put the 14 National id numbers")]
        public string StdFatherNID { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "You need to put the 14 National id numbers With No Spaces")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "You need to put the 14 National id numbers")]
        public string StdMotherNID { get; set; }
        public string StdCity { get; set; }
        public string StdAddress { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "You need to put the 14 National id numbers With No Spaces")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "You need to put the 14 National id numbers")]
        public string StdNID { get; set; }
        public int StdSchoolID { get; set; }
        public int StdClassID { get; set; }
        public string StdNationality { get; set; }
        public string StdStatus { get; set; }
        public Nullable<System.DateTime> StdJoinYear { get; set; }
        public string StdStaffSon { get; set; }
        public string StdLegalGuardianship { get; set; }
        public Nullable<bool> StdParentsSeparated { get; set; }

        public virtual Class Class { get; set; }
        public virtual NESSchool NESSchool { get; set; }
        public virtual StudentAccount StudentAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentGradesHistory> StudentGradesHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentSubjectEnroll> StudentSubjectEnrolls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentClothe> StudentClothes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoice_payment> invoice_payment { get; set; }
    }
}