using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX1._5.Models
{
    public class CartItem
    {
        [Display(Name = "Product id")]
        public int id { get; set; }

        [Display(Name = "Product name")]
        public string pName { get; set; }

        [Display(Name = "Pic")]
        public string pic { get; set; }

        [Display(Name = "Price")]
        public int price { get; set; }

        public Boolean isBundle { get; set; }

        public int quantity { get; set; }

        public CartItem()
        {
        }

        public CartItem(int id, string pName, string pic, int price, Boolean isBundle, int q)
        {
            this.id = id;
            this.pName = pName;
            this.pic = pic;
            this.price = price;
            this.isBundle = isBundle;
            this.quantity = q;
        }
    }
    public class Cart
    {
        public IList<CartItem> myCart { get; set; }

        public int totalPrice;

        public Cart()
        {
            myCart = new List<CartItem>();
        }
    }

    public class CheckOut
    {
        public CreateHistory history { get; set; }
        public CreditCard card { get; set; }
        public List<SelectListItem> creditTypes { get; set; }
        public List<SelectListItem> months { get; set; }
        public List<SelectListItem> years { get; set; }
    }
}