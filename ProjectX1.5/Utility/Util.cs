using ProjectX1._5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ProjectX1._5.Utility
{
    public class Util
    {
        private static DBmodels db = new DBmodels();
        public static List<SelectListItem> getMonthx()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Jun", Value = "Jun" },
                new SelectListItem { Text = "Feb", Value = "Feb"},
                new SelectListItem { Text = "Mar", Value = "Mar"},
                new SelectListItem { Text = "Apr", Value = "Apr"},
                new SelectListItem { Text = "May", Value = "May"},
                new SelectListItem { Text = "June", Value = "June"},
                new SelectListItem { Text = "July", Value = "July"},
                new SelectListItem { Text = "Aug", Value = "Aug"},
                new SelectListItem { Text = "Sep", Value = "Sep"},
                new SelectListItem { Text = "Oct", Value = "Oct"},
                new SelectListItem { Text = "Nov", Value = "Nov"},
                new SelectListItem { Text = "Dec", Value = "Dec"},
            };
        }

        public static List<SelectListItem> getYearx()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "2017", Value = "2017" },
                new SelectListItem { Text = "2018", Value = "2018"},
                new SelectListItem { Text = "2019", Value = "2019"},
                new SelectListItem { Text = "2020", Value = "2020"},
                new SelectListItem { Text = "2021", Value = "2021"},
                new SelectListItem { Text = "2022", Value = "2022"},
                new SelectListItem { Text = "2023", Value = "2023"},
            };
        }

        public static List<SelectListItem> getCreditTypex()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Visa", Value = "Visa" },
                new SelectListItem { Text = "Mastercard", Value = "Mastercard"},
                new SelectListItem { Text = "American Express", Value = "American Express"},
                new SelectListItem { Text = "Diners", Value = "Diners"},
            };
        }

        public static async Task<AspNetUser> getUser(IPrincipal user)
        {
            string email = user.Identity.Name;
            return await (from x in db.AspNetUsers
                          where x.Email.Equals(email)
                          select x).FirstAsync<AspNetUser>();
        }

        public static ShowCard convertToShowCard(CreditCard creditCard)
        {
            ShowCard card = new ShowCard();
            string[] split = creditCard.creditCardID.Split(':');

            card.id = creditCard.id;
            card.creditCardID = "**** **** **** " + split[split.Length - 1];
            card.card4Digits = split[split.Length - 1];
            card.creditType = creditCard.creditType;
            card.expirationMonth = creditCard.expirationMonth;
            card.expirationYear = creditCard.expirationYear;
            card.ownerName = creditCard.ownerName;
            card.securityDigits = creditCard.securityDigits;
            card.userId = creditCard.userId;
            card.Customer = creditCard.Customer;

            return card;
        }
    }
}