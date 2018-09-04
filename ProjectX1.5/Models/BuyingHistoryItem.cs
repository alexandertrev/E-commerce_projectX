namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BuyingHistoryItem
    {
        [Key]
        [Display(Name = "Buying id")]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int buyingId { get; set; }

        [Key]
        [Display(Name = "Product id")]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int productID { get; set; }

        [Key]
        [Display(Name = "Bundle id")]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int bundleId { get; set; }

        public int quantity { get; set; }

        public virtual Bundle Bundles { get; set; }

        public virtual BuyingHistory BuyingHistory { get; set; }

        public virtual Product Product { get; set; }
    }
}
