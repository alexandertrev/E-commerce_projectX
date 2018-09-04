namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class CreditCard
    {
        public int id { get; set; }

        [Required]
        [MinLength(16)]
        [StringLength(300)]
        public string creditCardID { get; set; }

        [Display(Name = "Credit type")]
        [Required(ErrorMessage = "Please select a type")]
        [StringLength(50)]
        public string creditType { get; set; }

        [Required]
        [Display(Name = "Owner name")]
        [StringLength(50)]
        public string ownerName { get; set; }

        [Display(Name = "Expiration month")]
        [Required(ErrorMessage = "Please select a month")]
        [StringLength(10)]
        public string expirationMonth { get; set; }

        [Display(Name = "Expiration year")]
        [Required(ErrorMessage = "Please select a year")]
        [StringLength(10)]
        public string expirationYear { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Card CVV number must be numeric")]
        [MinLength(3)]
        [StringLength(3)]
        public string securityDigits { get; set; }

        [StringLength(128)]
        public string userId { get; set; }

        public virtual Customer Customer { get; set; }
    }

    public partial class ShowCard
    {
        public int id { get; set; }
        public string creditCardID { get; set; }
        public string card4Digits { get; set; }
        public string creditType { get; set; }
        public string ownerName { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }
        public string securityDigits { get; set; }
        public string userId { get; set; }
        public virtual Customer Customer { get; set; }

    }
    public class CreditModelView
    {
        public CreditCard card { get; set; }
        public List<SelectListItem> creditTypes { get; set; }
        public List<SelectListItem> months { get; set; }
        public List<SelectListItem> years { get; set; }
    }

    public class BeforeEdit
    {
        [Required]
        [StringLength(16, ErrorMessage="Enter valid card number with 16 length")]
        public string cardID { get; set; }
        [Required]
        [StringLength(3, ErrorMessage = "Enter valid cvv number with 3 length")]
        public string cvv { get; set; }
        public int id { get; set; }
        public string lastDigits { get; set; }
    }
}
