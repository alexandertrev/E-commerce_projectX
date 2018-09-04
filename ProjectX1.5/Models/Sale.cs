namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int productID { get; set; }

        [Required]
        [StringLength(200)]
        public string saleName { get; set; }

        [Column(TypeName = "date")]
        public DateTime startDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime endDate { get; set; }

        public int discount { get; set; }

        [StringLength(300)]
        public string info { get; set; }

        public virtual Product Product { get; set; }
    }
}
