using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX1._5.Models
{
    public class Search
    {
        [Display(Name = "Category")]
        public AlcoholCategory categoryID { get; set; }
        [Display(Name = "Origin")]
        public Country alcoholOrigin { get; set; }
        [Display(Name = "Brand")]
        public CigarBrand cigarBrand { get; set; }
        [Display(Name = "Wrapper type")]
        public CigaresWrapper wrapperType { get; set; }
        [Display(Name = "Strength")]
        public CigarStrength strengthID { get; set; }
        [Display(Name = "Volume %")]
        public string volume { get; set; }
        [Display(Name = "Origin")]
        public Country cigarOrigin { get; set; }
        public SelectList AlcoholCategories { get; set; }
        public SelectList Brands { get; set; }
        public SelectList Contries { get; set; }
        public SelectList Wrappers { get; set; }
        public SelectList strength { get; set; }

    }

    public class SearchItem
    {
        [Display(Name = "Product id")]
        public int productID { get; set; }
        [Display(Name = "Product name")]
        public string pName { get; set; }
        public string pType { get; set; }
        [Display(Name = "Origin")]
        public string country { get; set; }
        [Display(Name = "Category")]
        public string alcoholType { get; set; }
        [Display(Name = "Brand")]
        public string cigarBrand { get; set; }
        [Display(Name = "Volum %")]
        public int alcoholVolume { get; set; }
        [Display(Name = "Strength")]
        public string cigarStrength { get; set; }
        [Display(Name = "Wrapper type")]
        public string cigarWrapper { get; set; }
        [Display(Name = "Bundle of")]
        public int cigarBundle { get; set; }
        [Display(Name = "Price")]
        public int price { get; set; }
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
        [Display(Name = "Pic")]
        public string pic { get; set; }
        [Display(Name = "Info")]
        public string info { get; set; }
        public IList<SearchItem> listInBundle { get; set; }
    }
}