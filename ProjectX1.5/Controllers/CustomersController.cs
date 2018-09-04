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
    public class CustomersController : Controller
    {
        private DBmodels db = new DBmodels();

        // GET: Customers
        public async Task<ActionResult> Index()
        {
            var customers = db.Customers.Include(c => c.AspNetUser).Include(c => c.Country);
            return View(await customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.customerId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.customerCountry = new SelectList(db.Country, "countryName", "pic");
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "customerId,customerName,customerLastName,customerphoneNumber,customerCountry,customerCity")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.customerId = new SelectList(db.AspNetUsers, "Id", "Email", customer.customerId);
            ViewBag.customerCountry = new SelectList(db.Country, "countryName", "pic", customer.customerCountry);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerId = new SelectList(db.AspNetUsers, "Id", "Email", customer.customerId);
            ViewBag.customerCountry = new SelectList(db.Country, "countryName", "countryName", customer.customerCountry);
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "customerId,customerName,customerLastName,customerphoneNumber,customerCountry,customerCity")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.AspNetUsers, "Id", "Email", customer.customerId);
            ViewBag.customerCountry = new SelectList(db.Country, "countryName", "countryName", customer.customerCountry);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            db.Customers.Remove(customer);
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

        public async Task<ActionResult> UpdateInfo(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            //ViewBag.customerId = new SelectList(db.AspNetUsers, "Id", "Email", customer.customerId);
            ViewBag.customerCountry = new SelectList(db.Country, "countryName", "countryName", customer.customerCountry);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateInfo([Bind(Include = "customerId,customerName,customerLastName,customerphoneNumber,customerCountry,customerCity")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Manage");
            }
            ViewBag.customerCountry = new SelectList(db.Country, "countryName", "countryName");
            return View(customer);
        }
    }
}
