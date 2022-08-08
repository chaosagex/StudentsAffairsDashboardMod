namespace StudentsAffairsDashboard.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class payment_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public payment_details()
        {
            this.invoice_payment = new HashSet<invoice_payment>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public decimal amount { get; set; }
        public int school { get; set; }
        public string year { get; set; }
        public short student_type { get; set; }
        public int Grade { get; set; }
        [NotMapped]
        public bool selected { get; set; }
        public virtual NESSchool NESSchool { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoice_payment> invoice_payment { get; set; }
    }
}
