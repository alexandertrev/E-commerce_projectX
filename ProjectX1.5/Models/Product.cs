namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            BuyingHistoryItems = new HashSet<BuyingHistoryItem>();
            BundleInfo = new HashSet<BundleInfo>();
            WatchList = new HashSet<WatchList>();
        }

        public int productID { get; set; }

        [Required]
        [Display(Name = "Product name")]
        [StringLength(50)]
        public string productName { get; set; }

        [Display(Name = "Price")]
        [Range(0,99999999)]
        public int price { get; set; }

        [Display(Name = "Info")]
        [StringLength(500)]
        public string info { get; set; }

        [Required]
        [Display(Name = "Pic")]
        [StringLength(500)]
        public string pic { get; set; }

        public virtual Accessory Accessory { get; set; }

        public virtual AlcoholProduct AlcoholProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyingHistoryItem> BuyingHistoryItems { get; set; }

        public virtual CigaresProduct CigaresProduct { get; set; }

        public virtual Sale Sale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BundleInfo> BundleInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WatchList> WatchList { get; set; }
    }
}
