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
    public class BuyingHistoriesController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: BuyingHistories
        public async Task<ActionResult> Index(string id)
        {
            var buyingHistory = db.BuyingHistory.Include(b => b.Customer);

            return View(await (from x in buyingHistory
                               where x.userId.ToString().Equals(id)
                               select x).ToListAsync<BuyingHistory>());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> Invoice(int id)
        {
            BuyingHistory history = await db.BuyingHistory.FindAsync(id);
            return View(history);
        }
    }
}
