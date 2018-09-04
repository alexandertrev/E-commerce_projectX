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
    public class AlcoholProductsController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: AlcoholProducts
        public async Task<ActionResult> Index()
        {
            var alcoholProduct = db.AlcoholProduct.Include(a => a.AlcoholCategory).Include(a => a.Country).Include(a => a.Product);
            return View(await alcoholProduct.ToListAsync());
        }

        // GET: AlcoholProducts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholProduct alcoholProduct = await db.AlcoholProduct.FindAsync(id);
            if (alcoholProduct == null)
            {
                return RedirectToAction("Index");
            }
            return View(alcoholProduct);
        }

        // GET: AlcoholProducts/Create
        public ActionResult Create()
        {
            ViewBag.categoryID = new SelectList(db.AlcoholCategory, "id", "categoryName");
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName");
            ViewBag.productID = new SelectList(db.Product, "productID", "productName");
            return View();
        }

        // POST: AlcoholProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AlcoholProduct alcoholProduct)
        {
            if (ModelState.IsValid)
            {
                Product newProdct = new Product();
                //newProdct.productName
                db.AlcoholProduct.Add(alcoholProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.categoryID = new SelectList(db.AlcoholCategory, "id", "categoryName", alcoholProduct.categoryID);
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName", alcoholProduct.origin);
            ViewBag.productID = new SelectList(db.Product, "productID", "productName", alcoholProduct.productID);
            return View(alcoholProduct);
        }

        // GET: AlcoholProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholProduct alcoholProduct = await db.AlcoholProduct.FindAsync(id);
            if (alcoholProduct == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.categoryID = new SelectList(db.AlcoholCategory, "id", "categoryName", alcoholProduct.categoryID);
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName", alcoholProduct.origin);
            ViewBag.productID = new SelectList(db.Product, "productID", "productName", alcoholProduct.productID);
            return View(alcoholProduct);
        }

        // POST: AlcoholProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AlcoholProduct alcoholProduct)
        {
            if (ModelState.IsValid)
            {
                alcoholProduct.Product.productID = alcoholProduct.productID;
                db.Entry(alcoholProduct).State = EntityState.Modified;
                db.Entry(alcoholProduct.Product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.categoryID = new SelectList(db.AlcoholCategory, "id", "categoryName", alcoholProduct.categoryID);
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName", alcoholProduct.origin);
            ViewBag.productID = new SelectList(db.Product, "productID", "productName", alcoholProduct.productID);
            return View(alcoholProduct);
        }

        // GET: AlcoholProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholProduct alcoholProduct = await db.AlcoholProduct.FindAsync(id);
            if (alcoholProduct == null)
            {
                return RedirectToAction("Index");
            }
            return View(alcoholProduct);
        }

        // POST: AlcoholProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AlcoholProduct alcoholProduct = await db.AlcoholProduct.FindAsync(id);
            Product product = await db.Product.FindAsync(id);

            db.AlcoholProduct.Remove(alcoholProduct);
            await db.SaveChangesAsync();

            db.Product.Remove(product);
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

        [AllowAnonymous]
        public async Task<ActionResult> ShowCategoryProducts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholCategory alcoholCategory = await db.AlcoholCategory.FindAsync(id);
            if (alcoholCategory == null)
            {
                return HttpNotFound();
            }

            return View(alcoholCategory.AlcoholProducts);
        }

        [HttpPost]
        public async Task<ActionResult> searchData()
        {
            string searchedItem = Request.Form["searchedData"].ToString();
            List<AlcoholProduct> myList = await (from x in db.AlcoholProduct
                                                  where x.Product.productName.Contains(searchedItem) || x.productID.ToString().Equals(searchedItem)
                                                  select x).ToListAsync<AlcoholProduct>();

            var result = from item in myList select new { productName = item.Product.productName.ToString(), categoryName = item.AlcoholCategory.categoryName.ToString(), volume = item.volume.ToString(), origin = item.Country.countryName.ToString(), price = item.Product.price.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }
    }
}
