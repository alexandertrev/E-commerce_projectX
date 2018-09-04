namespace ProjectX1._5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Accessory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int productID { get; set; }

        public int accessoryType { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public virtual AccessoryType AccessoryType1 { get; set; }

        public virtual Product Product { get; set; }
    }
}
