using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PropertyRentalManagement_FinalProject.Models;

namespace PropertyRentalManagement_FinalProject.Controllers
{
    public class TenantsController : Controller
    {
        private PropertyRentalManagement_DBEntities db = new PropertyRentalManagement_DBEntities();

        // GET: Tenants
        public ActionResult Index(string searching)
        {

            return View(db.Tenants.Where(t => t.FirstName.Contains(searching) || searching == null).ToList());

        }

        // GET: Tenants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Email,TenantUsername,TenantPassword")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("TenantLogin");
            }

            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Email,TenantUsername,TenantPassword")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
            db.SaveChanges();
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

        //============= LOGIN ==========================//

        // GET: Tenants/Create
        public ActionResult TenantLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TenantLogin(Tenant user)
        {
            var potentialTentant = db.Tenants.Where(x => x.TenantUsername == user.TenantUsername && x.TenantPassword == user.TenantPassword).FirstOrDefault();
            if (potentialTentant != null)
            {
                TempData["UserId"] = potentialTentant.UserId.ToString();
                TempData["FirstName"] = potentialTentant.FirstName.ToString();

                return RedirectToAction("../Buildings/ViewTenantBuilding");

            }
            else
            {
                user.loginErrorMessage = "Wrong username or password";
                return View("TenantLogin", user);
            }
        }

        //================== DISPLAY BUILDINGS ====================//
        // GET: Buildings
        public ActionResult ShowBuildings()
        {
            TempData["BuildingId"] = db.Buildings.ToString();
            return View(db.Buildings.ToList());
        }


        /// Manager TENANTS INDEX
        // GET: Tenants
        public ActionResult IndexManager()
        {

            return View(db.Tenants.ToList());

        }
    }
}
