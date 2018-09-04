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
    [Authorize(Roles ="Admin")]
    public class AlcoholCategoriesController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: AlcoholCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.AlcoholCategory.ToListAsync());
        }

        public async Task<ActionResult> testReturn()
        {
            IList<AlcoholCategory> myList = await db.AlcoholCategory.ToListAsync();

            var result = from item in myList select new { id = item.id.ToString(), pic = item.pic.ToString(), categoryName = item.categoryName.ToString()};
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }
        
        // GET: AlcoholCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholCategory alcoholCategory = await db.AlcoholCategory.FindAsync(id);
            if (alcoholCategory == null)
            {
                return RedirectToAction("Index");
            }
            return View(alcoholCategory);
        }

        // GET: AlcoholCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlcoholCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,categoryName,pic")] AlcoholCategory alcoholCategory)
        {
            if (ModelState.IsValid)
            {
                db.AlcoholCategory.Add(alcoholCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(alcoholCategory);
        }

        // GET: AlcoholCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholCategory alcoholCategory = await db.AlcoholCategory.FindAsync(id);
            if (alcoholCategory == null)
            {
                return RedirectToAction("Index");
            }
            return View(alcoholCategory);
        }

        // POST: AlcoholCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,categoryName,pic")] AlcoholCategory alcoholCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alcoholCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(alcoholCategory);
        }

        // GET: AlcoholCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlcoholCategory alcoholCategory = await db.AlcoholCategory.FindAsync(id);
            if (alcoholCategory == null)
            {
                return RedirectToAction("Index");
            }
            return View(alcoholCategory);
        }

        // POST: AlcoholCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AlcoholCategory alcoholCategory = await db.AlcoholCategory.FindAsync(id);
            db.AlcoholCategory.Remove(alcoholCategory);
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
        public async Task<ActionResult> BrowseAlcoholCategory()
        {
            return View(await db.AlcoholCategory.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> searchData()
        {
            string searchedItem = Request.Form["searchedData"].ToString();
            List<AlcoholCategory> myList = await (from x in db.AlcoholCategory
                   where x.categoryName.Contains(searchedItem) || x.id.ToString().Equals(searchedItem)
                   select x).ToListAsync<AlcoholCategory>();

            var result = from item in myList select new { id = item.id.ToString(), pic = item.pic.ToString(), categoryName = item.categoryName.ToString() };
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }
    }
}
