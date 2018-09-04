namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Country")]
    public partial class Country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            AlcoholProducts = new HashSet<AlcoholProduct>();
            CigaresProducts = new HashSet<CigaresProduct>();
            Customers = new HashSet<Customer>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int countryId { get; set; }

        [Required]
        [StringLength(500)]
        public string pic { get; set; }

        [Key]
        [Display(Name = "Country")]
        [StringLength(50)]
        public string countryName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlcoholProduct> AlcoholProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CigaresProduct> CigaresProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
