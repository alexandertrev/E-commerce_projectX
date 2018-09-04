namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CigaresWrapper")]
    public partial class CigaresWrapper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CigaresWrapper()
        {
            CigaresProducts = new HashSet<CigaresProduct>();
        }

        [Key]
        [Display(Name = "Wrapper name")]
        [StringLength(50)]
        public string wrapperName { get; set; }

        [Required]
        [StringLength(100)]
        public string color { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CigaresProduct> CigaresProducts { get; set; }
    }
}
