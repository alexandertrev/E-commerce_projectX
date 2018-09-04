using ProjectX1._5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ProjectX1._5.Controllers
{
    public class CartController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
            {
                Cart myCart = new Cart();
                Session["Cart"] = myCart;
            }
            return View();
        }

        public async Task<ActionResult> addItem(int? pId, string returnUrl, int pQuantity, string isBundle)
        {
            Cart myCart;
            TempData["returnUrl"] = returnUrl;

            if (Session["Cart"] == null)
                myCart = new Cart();
            else myCart = (Cart)Session["Cart"];

            if (pId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!(await addBundleOrProduct(pId, pQuantity, isBundle, myCart)))
            {
                return HttpNotFound();
            }


            return RedirectToAction("Index");
        }

        private async Task<Boolean> addBundleOrProduct(int? pId, int pQuantity, string isBundle, Cart myCart)
        {
            Product product = await db.Product.FindAsync(pId);
            Bundle bundle = await db.Bundles.FindAsync(pId);

            if (product == null && bundle == null)
                return false;

            if (isBundle != null && isBundle.Equals("yes") && bundle != null)
            {
                Boolean flag = false;
                foreach (var item in myCart.myCart)
                {
                    if (item.id == pId && item.isBundle == true)
                    {
                        item.quantity += pQuantity;
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    myCart.myCart.Add(new CartItem(bundle.bundleId, bundle.bundleName, bundle.pic, bundle.price, true, pQuantity));
                }
                myCart.totalPrice += bundle.price * pQuantity;
            }
            else
            {
                Boolean flag = false;
                foreach (var item in myCart.myCart)
                {
                    if (item.id == pId && item.isBundle==false)
                    {
                        item.quantity += pQuantity;
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    myCart.myCart.Add(new CartItem(product.productID, product.productName, product.pic, product.price, false, pQuantity));
                }
                myCart.totalPrice += product.price * pQuantity;
            }

            Session["Cart"] = myCart;
            return true;
        }

        [HttpPost]
        public ActionResult removeItem()
        {
            Cart myCart = (Cart)Session["Cart"];
            string itemId = Request.Form["removeItem"].ToString();
            int num = 0;
            if (itemId != null)
                num = int.Parse(itemId);
            CartItem ci = null; 

            foreach (var item in myCart.myCart)
            {
                if (item.id == num)
                {
                    ci = item;   
                }
            }
            myCart.totalPrice -= (ci.quantity*ci.price);
            myCart.myCart.Remove(ci);
            Session["Cart"] = myCart;

            var itemList = from item in myCart.myCart select new { productId = item.id.ToString(), pic = item.pic.ToString(), price = item.price.ToString(), quantity = item.quantity.ToString(), productName = item.pName.ToString() };
            var result = new {totalPrice = myCart.totalPrice.ToString(), cartItems = itemList };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }

        public ActionResult updateCart()
        {
            Cart myCart = (Cart)Session["Cart"];
            Cart newCart = new Cart();
            string strTmp = Request.Form["newQuantity"];
            if(strTmp==null)
            {
                return View("Index");
            }
            string[] quantityList = strTmp.Split(',');

            for(int i=0;i<quantityList.Length;i++)
            {
                int numQuantity = int.Parse(quantityList[i]);
                if(numQuantity > 0)
                {
                    newCart.myCart.Add(new CartItem(myCart.myCart[i].id, myCart.myCart[i].pName, myCart.myCart[i].pic, myCart.myCart[i].price, myCart.myCart[i].isBundle, numQuantity));
                    newCart.totalPrice += numQuantity * myCart.myCart[i].price;
                }
            }

            Session["Cart"] = newCart;

            return View("Index");
        }

        [Authorize]
        public async Task<ActionResult> CheckOut(string error)
        {
            Cart myCart = (Cart)Session["Cart"];
            if (myCart.myCart.Count == 0)
                return View("Index");

            string email = HttpContext.User.Identity.Name;
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);

            List<CreditCard> myList = await (from x in db.CreditCards
                                            where x.userId.ToString().Equals(user.Id)
                                            select x).ToListAsync<CreditCard>();

            IList<ShowCard> newList = new List<ShowCard>();

            foreach(CreditCard c in myList)
                newList.Add(Utility.Util.convertToShowCard(c));

            CreateHistory historyModel = new CreateHistory
            {
                cards = new SelectList(newList, "creditCardID", "creditCardID"),
                country = new SelectList(db.Country, "countryName", "countryName")
            };

            var model = new CheckOut
            {
                creditTypes = Utility.Util.getCreditTypex(),
                months = Utility.Util.getMonthx(),
                years = Utility.Util.getYearx(),
                history = historyModel
            };

            if(myList==null || myList.Count==0)
                ModelState.AddModelError("", "Add credit card before finishing order");
                
            if(error != null)
                ModelState.AddModelError("", error);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CheckOut([Bind(Exclude = "creditCard")]CheckOut newHistory)
        {
            Cart myCart = (Cart)Session["Cart"];

            if (myCart.myCart.Count == 0)
                return View("Index");

            if (ModelState.IsValid)
            {
                Session["Cart"] = null;
                DateTime today = DateTime.Now;
                AspNetUser user = await Utility.Util.getUser(HttpContext.User);

                newHistory.history.bh.userId = user.Id;
                newHistory.history.bh.buyingDate = today.ToString();

                db.BuyingHistory.Add(newHistory.history.bh);
                await db.SaveChangesAsync();

                BuyingHistory ttt = await db.BuyingHistory.OrderByDescending(i => i.buyingId).FirstOrDefaultAsync<BuyingHistory>();
                BuyingHistoryItem historyItem;

                foreach (CartItem p in myCart.myCart)
                {
                    historyItem = new BuyingHistoryItem();
                    historyItem.buyingId = ttt.buyingId;
                    if(p.isBundle)
                    {
                        historyItem.productID = 1012;
                        historyItem.bundleId = p.id;
                    }
                    else
                    {
                        historyItem.bundleId = 3;
                        historyItem.productID = p.id;
                    }
                    historyItem.quantity = p.quantity;
                    newHistory.history.bh.totalPrice += p.price*p.quantity;
                    db.BuyingHistoryItems.Add(historyItem);
                }
                db.Entry(newHistory.history.bh).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //creditCard.card.userId = userId; 

                return RedirectToAction("Invoice","BuyingHistories", new { id = ttt.buyingId});
            }

            return View(newHistory);
        }
    }
}