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
    public class CigaresProductsController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: CigaresProducts
        public async Task<ActionResult> Index()
        {
            var cigaresProducts = db.CigaresProducts.Include(c => c.CigarBrand1).Include(c => c.CigaresWrapper).Include(c => c.CigarStrength).Include(c => c.Country).Include(c => c.Product);
            return View(await cigaresProducts.ToListAsync());
        }

        // GET: CigaresProducts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigaresProduct cigaresProduct = await db.CigaresProducts.FindAsync(id);
            if (cigaresProduct == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigaresProduct);
        }

        // GET: CigaresProducts/Create
        public ActionResult Create()
        {
            ViewBag.cigarBrand = new SelectList(db.CigarBrands, "id", "brandName");
            ViewBag.wrapperType = new SelectList(db.CigaresWrapper, "wrapperName", "wrapperName");
            ViewBag.strengthID = new SelectList(db.CigarStrengths, "id", "strengthName");
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName");
            ViewBag.productID = new SelectList(db.Product, "productID", "productName");
            return View();
        }

        // POST: CigaresProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CigaresProduct cigaresProduct)
        {
            if (ModelState.IsValid)
            {
                db.CigaresProducts.Add(cigaresProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.cigarBrand = new SelectList(db.CigarBrands, "id", "brandName", cigaresProduct.cigarBrand);
            ViewBag.wrapperType = new SelectList(db.CigaresWrapper, "wrapperName", "wrapperName", cigaresProduct.wrapperType);
            ViewBag.strengthID = new SelectList(db.CigarStrengths, "id", "strengthName", cigaresProduct.strengthID);
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName", cigaresProduct.origin);
            ViewBag.productID = new SelectList(db.Product, "productID", "productName", cigaresProduct.productID);
            return View(cigaresProduct);
        }

        // GET: CigaresProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigaresProduct cigaresProduct = await db.CigaresProducts.FindAsync(id);
            if (cigaresProduct == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.cigarBrand = new SelectList(db.CigarBrands, "id", "brandName", cigaresProduct.cigarBrand);
            ViewBag.wrapperType = new SelectList(db.CigaresWrapper, "wrapperName", "wrapperName", cigaresProduct.wrapperType);
            ViewBag.strengthID = new SelectList(db.CigarStrengths, "id", "strengthName", cigaresProduct.strengthID);
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName", cigaresProduct.origin);
            ViewBag.productID = new SelectList(db.Product, "productID", "productName", cigaresProduct.productID);
            return View(cigaresProduct);
        }

        // POST: CigaresProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CigaresProduct cigaresProduct)
        {
            if (ModelState.IsValid)
            {
                cigaresProduct.Product.productID = cigaresProduct.productID;
                db.Entry(cigaresProduct).State = EntityState.Modified;
                db.Entry(cigaresProduct.Product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.cigarBrand = new SelectList(db.CigarBrands, "id", "brandName", cigaresProduct.cigarBrand);
            ViewBag.wrapperType = new SelectList(db.CigaresWrapper, "wrapperName", "wrapperName", cigaresProduct.wrapperType);
            ViewBag.strengthID = new SelectList(db.CigarStrengths, "id", "strengthName", cigaresProduct.strengthID);
            ViewBag.origin = new SelectList(db.Country, "countryName", "countryName", cigaresProduct.origin);
            ViewBag.productID = new SelectList(db.Product, "productID", "productName", cigaresProduct.productID);
            return View(cigaresProduct);
        }

        // GET: CigaresProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigaresProduct cigaresProduct = await db.CigaresProducts.FindAsync(id);
            if (cigaresProduct == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigaresProduct);
        }

        // POST: CigaresProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CigaresProduct cigaresProduct = await db.CigaresProducts.FindAsync(id);
            Product product = await db.Product.FindAsync(id);

            db.CigaresProducts.Remove(cigaresProduct);
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
        public async Task<ActionResult> ShowBrandProducts(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BrowseCigarBrands", "CigarBrands");
            }
            CigarBrand cigarBrand = await db.CigarBrands.FindAsync(id);
            if (cigarBrand == null)
            {
                return HttpNotFound();
            }

            return View(cigarBrand.CigaresProducts);
        }

        [HttpPost]
        public async Task<ActionResult> searchData()
        {
            string searchedItem = Request.Form["searchedData"].ToString();
            List<CigaresProduct> myList = await (from x in db.CigaresProducts
                                             where x.Product.productName.Contains(searchedItem) || x.productID.ToString().Equals(searchedItem)
                                             select x).ToListAsync<CigaresProduct>();

            var result = from item in myList select new { id = item.productID.ToString(), productName = item.Product.productName.ToString(), brandName = item.CigarBrand1.brandName.ToString(), origin = item.Country.countryName.ToString(), bundle = item.bundle.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }
    }
}
