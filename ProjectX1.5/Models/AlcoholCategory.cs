namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlcoholCategory")]
    public partial class AlcoholCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AlcoholCategory()
        {
            AlcoholProducts = new HashSet<AlcoholProduct>();
        }

        public int id { get; set; }

        [Required]
        [Display(Name ="Category name")]
        [StringLength(50)]
        public string categoryName { get; set; }

        [Required]
        [Url]
        [Display(Name = "Pic")]
        [StringLength(500)]
        public string pic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlcoholProduct> AlcoholProducts { get; set; }
    }
}
