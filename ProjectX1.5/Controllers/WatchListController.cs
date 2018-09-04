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

namespace ProjectX1._5.Controllers
{
    [Authorize]
    public class WatchListController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: WatchList
        public async Task<ActionResult> Index()
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Customer cust = await db.Customers.FindAsync(user.Id.ToString());

            return View(cust.WatchList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> AddToWatchList(int pId, string isBundle)
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Product p = await db.Product.FindAsync(pId);
            Bundle b = await db.Bundles.FindAsync(pId);

            WatchList wl = new WatchList();
            Boolean found;
            wl.customerId = user.Id;

            if (isBundle.Equals("yes"))
            {
                wl.bundleId = b.bundleId;
                wl.productID = 1012;

                found = await (from x in db.WatchList
                               where x.customerId == wl.customerId && x.bundleId == wl.bundleId
                               select x).AnyAsync();
            }
            else
            {
                wl.productID = p.productID;
                wl.bundleId = 3;

                found = await (from x in db.WatchList
                               where x.customerId == wl.customerId && x.productID == wl.productID
                               select x).AnyAsync();
            }
            
            if (!found)
            {
                db.WatchList.Add(wl);
                await db.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RemoveFromWatchList(int pId, string isBundle)
        {
            AspNetUser user = await Utility.Util.getUser(HttpContext.User);
            Product p = await db.Product.FindAsync(pId);
            Bundle b = await db.Bundles.FindAsync(pId);

            WatchList wl=null;

            if (isBundle.Equals("yes"))
            {
                wl = await (from x in db.WatchList
                            where x.customerId == user.Id && x.bundleId == b.bundleId
                            select x).FirstAsync();
            }
            else
            {
                wl = await (from x in db.WatchList
                            where x.customerId == user.Id && x.productID == p.productID
                            select x).FirstAsync();
            }

            if (wl!=null)
            {
                db.WatchList.Remove(wl);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}