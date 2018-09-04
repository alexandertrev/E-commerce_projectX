using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectX1._5.Models;
using System.Web.Script.Serialization;

namespace ProjectX1._5.Controllers
{
    public class CreditCardsController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: CreditCards
        public async Task<ActionResult> Index()
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());
            IList<ShowCard> cards = new List<ShowCard>();

            foreach (CreditCard card in cust.CreditCards)
                cards.Add(Utility.Util.convertToShowCard(card));

            return View(cards);
        }

        // GET: CreditCards/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCard creditCard = await db.CreditCards.FindAsync(id);
            if (creditCard == null)
            {
                return RedirectToAction("Index");
            }
            return View(creditCard);
        }

        // GET: CreditCards/Create
        public async Task<ActionResult> Create()
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());

            ViewBag.userId = cust.customerId;

            var model = new CreditModelView
            {
                creditTypes = Utility.Util.getCreditTypex(),
                months = Utility.Util.getMonthx(),
                years = Utility.Util.getYearx()
            };
            return View(model);
        }

        // POST: CreditCards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreditModelView creditCard, string userId)
        {
            Boolean found = await CheckIfExist(creditCard.card.creditCardID);

            if (ModelState.IsValid && found!=true)
            {
                string card10Digits = creditCard.card.creditCardID.Substring(0, 12);
                string card4Digits = creditCard.card.creditCardID.Substring(12, 4);
                string hashedStr = Utility.Encryption.CreateHash(card10Digits);

                creditCard.card.creditCardID = hashedStr + ":" + card4Digits;
                creditCard.card.userId = userId;
                db.CreditCards.Add(creditCard.card);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            var model = new CreditModelView
            {
                card = creditCard.card,
                creditTypes = Utility.Util.getCreditTypex(),
                months = Utility.Util.getMonthx(),
                years = Utility.Util.getYearx()
            };
            ModelState.AddModelError("", "Credit card with the same number already exist!");
            ViewBag.userId = userId;

            return View(model);
        }

        // GET: CreditCards/BeforeEdit/
        public ActionResult BeforeEdit(int id, string card4Digits, string operation)
        {
            ViewBag.LastDigits = card4Digits;
            ViewBag.operation = operation;

            var model = new BeforeEdit
            {
                id = id,
                lastDigits = card4Digits
            };
            return View(model);
        }

        // POST: CreditCards/BeforeEdit/
        [HttpPost]
        public async Task<ActionResult> BeforeEdit(BeforeEdit card, string operation)
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());

            string card10Digits = card.cardID.Substring(0, 12);
            string card4Digits = card.cardID.Substring(12, 4);

            CreditCard foundCard = await db.CreditCards.FindAsync(card.id);

            if (foundCard != null)
            {
                Boolean match=false;
                if (Utility.Encryption.ValidateString(card10Digits, foundCard.creditCardID))
                    match = true;
                
                if (match)
                {
                    return RedirectToAction(operation, foundCard);
                }
            }

            ModelState.AddModelError("", "Wrong credit card number/cvv");
            return View(card);
        }

        public async Task<Boolean> CheckIfExist(string cardNumber)
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());

            string card10Digits = cardNumber.Substring(0, 12);
            string card4Digits = cardNumber.Substring(12, 4);

            IList<CreditCard> cards = await (from x in db.CreditCards
                                             where x.userId.Equals(cust.customerId) && x.creditCardID.Contains(card4Digits)
                                             select x).ToListAsync<CreditCard>();

            CreditCard foundCard = null;

            if (cards != null)
            {
                
                foreach (CreditCard c in cards)
                {
                    if (Utility.Encryption.ValidateString(card10Digits, c.creditCardID))
                        foundCard = c;
                }
            }

            return foundCard != null;
        }

        //POST: CreditCards/Edit/5
        public ActionResult Edit(CreditCard c)
        {
            if (c == null)
            {
                return RedirectToAction("Index");
            }

            var model = new CreditModelView
            {
                card = c,
                creditTypes = Utility.Util.getCreditTypex(),
                months = Utility.Util.getMonthx(),
                years = Utility.Util.getYearx()
            };

            ViewBag.userId = new SelectList(db.Customers, "customerId", "customerName", c.userId);
            return View(model);
        }

        // POST: CreditCards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreditModelView creditCard)
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());

            if (ModelState.IsValid)
            {
                creditCard.card.userId = cust.customerId;
                db.Entry(creditCard.card).State = EntityState.Modified;
                await db.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            ViewBag.userId = new SelectList(db.Customers, "customerId", "customerName", creditCard.card.userId);
            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        public ActionResult Delete(CreditCard c)
        {
            if (c == null)
            {
                return RedirectToAction("Index");
            }

            return View(Utility.Util.convertToShowCard(c));
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CreditCard creditCard = await db.CreditCards.FindAsync(id);
            db.CreditCards.Remove(creditCard);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> getCredits()
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());

            var result = from item in cust.CreditCards select new { creditCardID = item.creditCardID.ToString(), creditType = item.creditType.ToString(), expirationYear = item.expirationYear.ToString(), ownerName = item.ownerName.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInCheckOut(CheckOut creditCard)
        {
            Boolean found = await CheckIfExist(creditCard.card.creditCardID);

            if (ModelState.IsValid && found != true)
            {
                string card10Digits = creditCard.card.creditCardID.Substring(0, 12);
                string card4Digits = creditCard.card.creditCardID.Substring(12, 4);
                string hashedStr = Utility.Encryption.CreateHash(card10Digits);

                creditCard.card.creditCardID = hashedStr + ":" + card4Digits;
                creditCard.card.userId = (await Utility.Util.getUser(HttpContext.User)).Id;
                db.CreditCards.Add(creditCard.card);
                await db.SaveChangesAsync();

                return RedirectToAction("CheckOut", "cart");
            }

            return RedirectToAction("CheckOut","cart",new { error = "Credit card with the same number already exist!"});
        }
    }
}