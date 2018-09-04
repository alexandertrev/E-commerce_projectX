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
    public class CigarBrandsController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: CigarBrands
        public async Task<ActionResult> Index()
        {
            return View(await db.CigarBrands.ToListAsync());
        }

        // GET: CigarBrands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigarBrand cigarBrand = await db.CigarBrands.FindAsync(id);
            if (cigarBrand == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigarBrand);
        }

        // GET: CigarBrands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CigarBrands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,brandName,pic")] CigarBrand cigarBrand)
        {
            if (ModelState.IsValid)
            {
                db.CigarBrands.Add(cigarBrand);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cigarBrand);
        }

        // GET: CigarBrands/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigarBrand cigarBrand = await db.CigarBrands.FindAsync(id);
            if (cigarBrand == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigarBrand);
        }

        // POST: CigarBrands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,brandName,pic")] CigarBrand cigarBrand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cigarBrand).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cigarBrand);
        }

        // GET: CigarBrands/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigarBrand cigarBrand = await db.CigarBrands.FindAsync(id);
            if (cigarBrand == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigarBrand);
        }

        // POST: CigarBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CigarBrand cigarBrand = await db.CigarBrands.FindAsync(id);
            db.CigarBrands.Remove(cigarBrand);
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
        public async Task<ActionResult> BrowseCigarBrands()
        {
            return View(await db.CigarBrands.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> searchData()
        {
            string searchedItem = Request.Form["searchedData"].ToString();
            List<CigarBrand> myList = await (from x in db.CigarBrands
                                                 where x.brandName.Contains(searchedItem) || x.id.ToString().Equals(searchedItem)
                                                 select x).ToListAsync<CigarBrand>();

            var result = from item in myList select new { pic = item.pic.ToString(), brandName = item.brandName.ToString(), id = item.id.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer(); 

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }
    }
}
