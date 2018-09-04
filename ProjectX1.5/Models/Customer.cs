namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            BuyingHistories = new HashSet<BuyingHistory>();
            CreditCards = new HashSet<CreditCard>();
            WatchList = new HashSet<WatchList>();
        }

        public string customerId { get; set; }

        [Required]
        [StringLength(20)]
        public string customerName { get; set; }

        [Required]
        [StringLength(20)]
        public string customerLastName { get; set; }

        [StringLength(20)]
        public string customerphoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string customerCountry { get; set; }

        [StringLength(50)]
        public string customerCity { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyingHistory> BuyingHistories { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditCard> CreditCards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WatchList> WatchList { get; set; }
    }
}
