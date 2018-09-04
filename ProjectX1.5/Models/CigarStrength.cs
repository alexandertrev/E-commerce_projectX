namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CigarStrength")]
    public partial class CigarStrength
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CigarStrength()
        {
            CigaresProducts = new HashSet<CigaresProduct>();
        }

        public int id { get; set; }

        [Required]
        [Display(Name = "Strength")]
        [StringLength(50)]
        public string strengthName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CigaresProduct> CigaresProducts { get; set; }
    }
}
