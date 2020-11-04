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
    public class ApartmentsController : Controller
    {
        private PropertyRentalManagement_DBEntities db = new PropertyRentalManagement_DBEntities();

        // GET: Apartments
        public ActionResult Index()
        {
            // FROM MANAGER TO BUILDING TO APARTMENTS
            if(TempData["BuilidingId"] != null)
            {
                var buildId = Convert.ToInt32(TempData["BuilidingId"]);
                var apartments = db.Apartments.Where(a => a.BuildingId == buildId);
                //var apartments = db.Apartments.Include(a => a.Building);
                return View(apartments.ToList());
            }
            else
            {
                var apartments = db.Apartments.Include(a => a.Building);
                return View(apartments.ToList());
            }

        }

        public ActionResult OwnerIndex(string name)
        {
            var apt = db.Apartments.Where((a => (a.Building.BuildingName.Contains(name) || name == null)));
            return View(apt.ToList());

        }

        //GET: APARTMENTS FOR TENANTS VIEW
        public ActionResult ViewApartments(string size, float minrange = 0, float maxrange = 0)
        {
            //  FROM USER TO BUILDINGS TO APARTMENTS           
            var apt = db.Apartments.Where((a => (a.Price >= minrange && a.Price <= maxrange) || (a.Size.StartsWith(size) || size == null)));
            return View(apt.ToList());

        }

            // GET: Apartments/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public ActionResult Create()
        {
            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName");
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApartmentId,ApartmentNumber,Size,Utility,Price,Availability,BuildingId")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApartmentId,ApartmentNumber,Size,Utility,Price,Availability,BuildingId")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
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
    }
}
