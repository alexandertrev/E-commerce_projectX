namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CigarBrand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CigarBrand()
        {
            CigaresProducts = new HashSet<CigaresProduct>();
        }

        public int id { get; set; }

        [Required]
        [Display(Name = "Brand name")]
        [StringLength(100)]
        public string brandName { get; set; }

        [Required]
        [StringLength(500)]
        public string pic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CigaresProduct> CigaresProducts { get; set; }
    }
}
