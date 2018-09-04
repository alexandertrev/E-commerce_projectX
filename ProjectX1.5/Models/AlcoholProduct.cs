namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlcoholProduct")]
    public partial class AlcoholProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int productID { get; set; }

        [Display(Name = "Category name")]
        public int? categoryID { get; set; }

        [Range(0,100)]
        [Display(Name = "Volume %")]
        public int volume { get; set; }

        [Display(Name = "Origin")]
        [StringLength(50)]
        public string origin { get; set; }

        public virtual AlcoholCategory AlcoholCategory { get; set; }

        public virtual Country Country { get; set; }

        public virtual Product Product { get; set; }
    }
}
