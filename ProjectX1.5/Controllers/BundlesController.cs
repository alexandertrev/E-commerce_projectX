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
    [Authorize(Roles = "Admin")]
    public class BundlesController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: Bundles
        public async Task<ActionResult> Index()
        {
            IList<Bundle> bundles = await db.Bundles.ToListAsync();
            foreach (Bundle b in bundles)
            {
                if (b.bundleName.Equals("Product"))
                {
                    bundles.Remove(b);
                    break;
                }
            }
            return View(bundles);
        }

        // GET: Bundles/Create
        public ActionResult Create()
        {
            Session["BundleList"] = new List<BundleInfo>();
            return View();
        }

        // POST: Bundles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Bundle bundle)
        {
            List<BundleInfo> bundleItems = (List<BundleInfo>)Session["BundleList"];
            if (bundleItems == null || bundleItems.Count == 0)
            {
                ModelState.AddModelError("", "Add items to bundle");
                return View(bundle);
            }
            if (ModelState.IsValid)
            {
                BundleInfo item;
                db.Bundles.Add(bundle);
                await db.SaveChangesAsync();

                Bundle justAddedBundle = await db.Bundles.OrderByDescending(i => i.bundleId).FirstOrDefaultAsync<Bundle>();
                foreach(BundleInfo p in bundleItems)
                {
                    item = new BundleInfo();
                    item.bundleId = justAddedBundle.bundleId;
                    item.productID = p.productID;
                    item.quantity = p.quantity;
                    db.BundleInfos.Add(item);
                }
                await db.SaveChangesAsync();
                Session["BundleList"] = null;
                return RedirectToAction("Index");
            }

            return View(bundle);
        }

        // GET: Bundles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bundle bundle = await db.Bundles.FindAsync(id);

            Session["BundleList"] = bundle.BundleInfos.ToList<BundleInfo>();
            if (bundle == null)
            {
                return RedirectToAction("Index");
            }
            return View(bundle);
        }

        // POST: Bundles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "bundleId,bundleName,info,pic,price")] Bundle bundle)
        {
            if (ModelState.IsValid)
            {
                List<BundleInfo> oldBundle = await (from x in db.BundleInfos
                                                      where x.bundleId == bundle.bundleId
                                                      select x).ToListAsync<BundleInfo>();
                List<BundleInfo> newBundle = (List<BundleInfo>)Session["BundleList"];

                db.Entry(bundle).State = EntityState.Modified;
                await db.SaveChangesAsync();

                foreach (BundleInfo item in oldBundle)
                {
                    db.BundleInfos.Remove(item);
                }
                await db.SaveChangesAsync();

                foreach (BundleInfo item in newBundle)
                {
                    db.BundleInfos.Add(new BundleInfo() { bundleId = bundle.bundleId, productID = item.productID, quantity = item.quantity });
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(bundle);
        }

        // GET: Bundles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bundle bundle = await db.Bundles.FindAsync(id);
            if (bundle == null)
            {
                return RedirectToAction("Index");
            }
            return View(bundle);
        }

        // POST: Bundles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Bundle bundle = await db.Bundles.FindAsync(id);
            db.Bundles.Remove(bundle);
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

        public async Task<ActionResult> getPage()
        {
            string searchedItem = Request.Form["searchedData"].ToString();
            int pageNumber;

            if (Request.Form["pageNumber"] != null)
                pageNumber = int.Parse(Request.Form["pageNumber"].ToString());
            else pageNumber = 1;

            IList<Product> searchedProducts = await (from x in db.Product
                                                     where x.productName.Contains(searchedItem)
                                                     select x).ToListAsync<Product>();
            int total = searchedProducts.Count;
            int start = (pageNumber - 1) * 5;
            int end = start + 5;
            List<Product> finalList = new List<Product>();

            for (int i = start; i < end && i < total; i++)
            {
                if(!searchedProducts[i].productName.Equals("Bundle"))
                    finalList.Add(searchedProducts[i]);
            }
            if (finalList.Count == 0)
                pageNumber = 0;
            var resultArr = from item in finalList select new { id = item.productID.ToString(), pic = item.pic.ToString(), productName = item.productName.ToString(), price = item.price.ToString() };

            var sendData = new { resultArr = resultArr, numOfPages = total, pageNumber = pageNumber };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(sendData), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> addProductToBundle()
        {
            string productId = Request.Form["productId"].ToString();
            Product product = await db.Product.FindAsync(int.Parse(productId));
            List<BundleInfo> bundleItems;

            if (Session["BundleList"]==null)
            {
                bundleItems = new List<BundleInfo>();
                bundleItems.Add(new BundleInfo() {Product = product,quantity = 1,productID=product.productID });
                Session["BundleList"] = bundleItems;
            }
            else
            {
                bundleItems = (List<BundleInfo>)Session["BundleList"];
                Boolean flag = false;
                foreach(BundleInfo p in bundleItems)
                {
                    if (p.Product.productID == product.productID)
                    {
                        p.quantity++;
                        flag = true;
                    }  
                }
                if (!flag)
                    bundleItems.Add(new BundleInfo() { Product = product, quantity = 1, productID = product.productID });
                Session["BundleList"] = bundleItems;
            }
            var itemList = from item in bundleItems select new { id = item.Product.productID.ToString(), pic = item.Product.pic.ToString(), price = item.Product.price.ToString(), quantity = item.quantity.ToString(), productName = item.Product.productName.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(itemList), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> removeProductFromBundle()
        {
            string productId = Request.Form["productId"].ToString();
            Product product = await db.Product.FindAsync(int.Parse(productId));
            List<BundleInfo> bundleItems;
            List<BundleInfo> newList;

            newList = new List<BundleInfo>();
            bundleItems = (List<BundleInfo>)Session["BundleList"];

            foreach (BundleInfo p in bundleItems)
            {
                if (p.Product.productID != product.productID)
                    newList.Add(p);
            }
            Session["BundleList"] = newList;

            var itemList = from item in newList select new { id = item.Product.productID.ToString(), pic = item.Product.pic.ToString(), price = item.Product.price.ToString(), quantity = item.quantity.ToString(), productName = item.Product.productName.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(itemList), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ShowBundles()
        {
            IList<Bundle> bundles = await db.Bundles.ToListAsync<Bundle>();
            foreach(Bundle b in bundles)
            {
                if(b.bundleName.Equals("Product"))
                {
                    bundles.Remove(b);
                    break;
                }
            }
            if (bundles == null)
            {
                return HttpNotFound();
            }
            return View(bundles);
        }

        [AllowAnonymous]
        public async Task<ActionResult> getProductsName()
        {
            IList<Product> products = await db.Product.ToListAsync<Product>();

            string[] arr = new string[products.Count-1];
            for(int i=0,j=0;i<arr.Length;i++)
            {
                if (!products[i].productName.Equals("Bundle"))
                {
                    arr[j] = products[i].productName;
                    j++;
                }

            }

            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(arr), JsonRequestBehavior.AllowGet);
        }
    }
}
