namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("BuyingHistory")]
    public partial class BuyingHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuyingHistory()
        {
            BuyingHistoryItems = new HashSet<BuyingHistoryItem>();
        }

        [Key]
        public int buyingId { get; set; }

        [StringLength(128)]
        public string userId { get; set; }

        [Required]
        [Display(Name = "First name")]
        [StringLength(100)]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [StringLength(100)]
        public string lastName { get; set; }

        [Display(Name = "Phone number")]
        [StringLength(15)]
        public string phoneNumber { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        [StringLength(20)]
        public string postalCode { get; set; }

        [Display(Name = "Buying date")]
        [StringLength(50)]
        public string buyingDate { get; set; }

        [Required]
        [Display(Name = "Credit card")]
        [StringLength(300)]
        public string creditCardID { get; set; }

        [Required]
        [Display(Name = "Country")]
        [StringLength(50)]
        public string shippmentCountry { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(150)]
        public string shippmentCity { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(150)]
        public string shippmentAddress { get; set; }

        [Display(Name = "Total price")]
        public int totalPrice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyingHistoryItem> BuyingHistoryItems { get; set; }

        public virtual Customer Customer { get; set; }
    }

    public class CreateHistory
    {
        public BuyingHistory bh { get; set; }

        public SelectList cards { get; set; }

        public SelectList country { get; set; }
    }
}
