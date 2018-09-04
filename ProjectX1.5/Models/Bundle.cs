namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bundle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bundle()
        {
            BundleInfos = new HashSet<BundleInfo>();
            BuyingHistoryItems = new HashSet<BuyingHistoryItem>();
            WatchList = new HashSet<WatchList>();
        }

        [Display(Name = "Bundle id")]
        public int bundleId { get; set; }

        [Required]
        [Display(Name = "Bundle name")]
        [StringLength(50)]
        public string bundleName { get; set; }

        [Display(Name = "Info")]
        [StringLength(500)]
        public string info { get; set; }

        [Display(Name = "Price")]
        [Range(1,999999999)]
        public int price { get; set; }

        [Url]
        [Display(Name = "Pic")]
        [StringLength(300)]
        public string pic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BundleInfo> BundleInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyingHistoryItem> BuyingHistoryItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WatchList> WatchList { get; set; }
    }
}
