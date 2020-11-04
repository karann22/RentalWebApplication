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
    public class PropertyOwnersController : Controller
    {
        private PropertyRentalManagement_DBEntities db = new PropertyRentalManagement_DBEntities();

        // GET: PropertyOwners
        public ActionResult Index()
        {
            return View(db.PropertyOwners.ToList());
        }

        // GET: PropertyOwners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyOwner propertyOwner = db.PropertyOwners.Find(id);
            if (propertyOwner == null)
            {
                return HttpNotFound();
            }
            return View(propertyOwner);
        }

        // GET: PropertyOwners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropertyOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerId,OwnerUsername,OwnerPassword")] PropertyOwner propertyOwner)
        {
            if (ModelState.IsValid)
            {
                db.PropertyOwners.Add(propertyOwner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(propertyOwner);
        }

        // GET: PropertyOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyOwner propertyOwner = db.PropertyOwners.Find(id);
            if (propertyOwner == null)
            {
                return HttpNotFound();
            }
            return View(propertyOwner);
        }

        // POST: PropertyOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerId,OwnerUsername,OwnerPassword")] PropertyOwner propertyOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propertyOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(propertyOwner);
        }

        // GET: PropertyOwners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyOwner propertyOwner = db.PropertyOwners.Find(id);
            if (propertyOwner == null)
            {
                return HttpNotFound();
            }
            return View(propertyOwner);
        }

        // POST: PropertyOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PropertyOwner propertyOwner = db.PropertyOwners.Find(id);
            db.PropertyOwners.Remove(propertyOwner);
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

        // GET: Property Owner Login
        public ActionResult OwnerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OwnerLogin(PropertyOwner propertyOwner)
        {
            var owner = db.PropertyOwners.Where(x => x.OwnerUsername == propertyOwner.OwnerUsername && x.OwnerPassword == propertyOwner.OwnerPassword).FirstOrDefault();
            if (owner != null)
            {
                TempData["OwnwerId"] = owner.OwnerId.ToString();
                TempData["OwnerUsername"] = owner.OwnerUsername.ToString();

                return RedirectToAction("OwnerProfile");

            }
            else
            {
                owner.loginErrorMessage = "Wrong username or password";
                return View();
            }
        }

        //================== PROFILE ====================//
        public ActionResult OwnerProfile()
        {
            if (TempData["OwnerId"] != null)
            {
                return View("OwnerProfile");
            }
            else
            {
                return View();
            }
        }
    }
}
