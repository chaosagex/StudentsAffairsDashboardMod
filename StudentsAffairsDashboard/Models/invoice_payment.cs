namespace StudentsAffairsDashboard.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class invoice_payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public invoice_payment()
        {
            this.invoice_payment1 = new HashSet<invoice_payment>();
            this.payment_details = new List<payment_details>();
        }

        public int id { get; set; }
        public int student { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime date { get; set; }

        public decimal total_cost { get; set; }
        public decimal paid { get; set; }
        public decimal remaining { get; set; }
        public Nullable<int> previous_payment { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> deposit_date { get; set; }
        public string reference_code { get; set; }
        public string depositer { get; set; }
        public short type { get; set; }
        public Nullable<int> SeqID { get; set; }
        public string STAN { get; set; }
        public string BATCH { get; set; }
        public string machine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoice_payment> invoice_payment1 { get; set; }
        public virtual invoice_payment invoice_payment2 { get; set; }
        public virtual StudentsMain StudentsMain { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<payment_details> payment_details { get; set; }
        public virtual Machine Machine1 { get; set; }
    }
}