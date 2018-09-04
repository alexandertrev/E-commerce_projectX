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
    public class CigaresWrappersController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: CigaresWrappers
        public async Task<ActionResult> Index()
        {
            return View(await db.CigaresWrapper.ToListAsync());
        }

        // GET: CigaresWrappers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigaresWrapper cigaresWrapper = await db.CigaresWrapper.FindAsync(id);
            if (cigaresWrapper == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigaresWrapper);
        }

        // GET: CigaresWrappers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CigaresWrappers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "wrapperName,color")] CigaresWrapper cigaresWrapper)
        {
            if (ModelState.IsValid)
            {
                db.CigaresWrapper.Add(cigaresWrapper);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cigaresWrapper);
        }

        // GET: CigaresWrappers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigaresWrapper cigaresWrapper = await db.CigaresWrapper.FindAsync(id);
            if (cigaresWrapper == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigaresWrapper);
        }

        // POST: CigaresWrappers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "wrapperName,color")] CigaresWrapper cigaresWrapper, string oldWrapperName)
        {
            if (ModelState.IsValid)
            {
                CigaresWrapper c = await db.CigaresWrapper.FindAsync(oldWrapperName);
                CigaresWrapper cc = await db.CigaresWrapper.FindAsync(c);
                if (cc != null)
                    goto ret;
                db.CigaresWrapper.Remove(c);
                db.CigaresWrapper.Add(cigaresWrapper);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ret: 

            return View(cigaresWrapper);
        }

        //public async Task<Boolean> searchWrapper(CigaresWrapper cw)
        //{
            


        //}

        // GET: CigaresWrappers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CigaresWrapper cigaresWrapper = await db.CigaresWrapper.FindAsync(id);
            if (cigaresWrapper == null)
            {
                return RedirectToAction("Index");
            }
            return View(cigaresWrapper);
        }

        // POST: CigaresWrappers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            CigaresWrapper cigaresWrapper = await db.CigaresWrapper.FindAsync(id);
            db.CigaresWrapper.Remove(cigaresWrapper);
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

        [HttpPost]
        public async Task<ActionResult> searchData()
        {
            string searchedItem = Request.Form["searchedData"].ToString();
            List<CigaresWrapper> myList = await (from x in db.CigaresWrapper
                                                 where x.wrapperName.Contains(searchedItem) || x.color.ToString().Contains(searchedItem)
                                                 select x).ToListAsync<CigaresWrapper>();

            var result = from item in myList select new { wrapperName = item.wrapperName.ToString(), color = item.color.ToString(),};
            JavaScriptSerializer myJSS = new JavaScriptSerializer();

            return Json(myJSS.Serialize(result), JsonRequestBehavior.AllowGet);
        }
    }
}
