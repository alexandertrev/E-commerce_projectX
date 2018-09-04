namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CigaresProduct
    {
        [Key]
        [Display(Name = "Product id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int productID { get; set; }

        [Range(1,999)]
        [Display(Name = "Bundle of")]
        public int bundle { get; set; }

        [Display(Name = "Strength")]
        public int strengthID { get; set; }

        [Display(Name = "Origin")]
        [StringLength(50)]
        public string origin { get; set; }

        [Display(Name = "Wrapper type")]
        [StringLength(50)]
        public string wrapperType { get; set; }

        [Display(Name = "Cigar brand")]
        public int cigarBrand { get; set; }

        public virtual CigarBrand CigarBrand1 { get; set; }

        public virtual Country Country { get; set; }

        public virtual Product Product { get; set; }

        public virtual CigarStrength CigarStrength { get; set; }

        public virtual CigaresWrapper CigaresWrapper { get; set; }
    }
}
