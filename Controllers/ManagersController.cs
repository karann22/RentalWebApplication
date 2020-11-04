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
    public class ManagersController : Controller
    {
        private PropertyRentalManagement_DBEntities db = new PropertyRentalManagement_DBEntities();

        // GET: Managers
        public ActionResult Index(string searching)
        {
                var managers = db.Managers.Include(m => m.Building).Where(m => m.FirstName.Contains(searching) || searching == null);
                return View(managers.ToList());
            
        }

        // GET: Managers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Managers/Create
        public ActionResult Create()
        {
            ViewBag.BuilidingId = new SelectList(db.Buildings, "BuildingId", "BuildingName");
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManagerId,FirstName,LastName,Email,PhoneNumber,ManagerUsername,ManagerPassword,BuilidingId")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuilidingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", manager.BuilidingId);
            return View(manager);
        }


        // GET: Managers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuilidingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", manager.BuilidingId);
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManagerId,FirstName,LastName,Email,PhoneNumber,ManagerUsername,ManagerPassword,BuilidingId")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuilidingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", manager.BuilidingId);
            return View(manager);
        }

        // GET: Managers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
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

        // GET: MANAGER/Login
        public ActionResult ManagerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagerLogin(Manager manager)
        {
            var regManager = db.Managers.Where(x => x.ManagerUsername == manager.ManagerUsername && x.ManagerPassword == manager.ManagerPassword && x.Building.BuildingName == manager.Building.BuildingName).FirstOrDefault();
            if (regManager != null)
            {
                TempData["ManagerId"] = regManager.ManagerId.ToString();
                TempData["FirstName"] = regManager.FirstName.ToString();
                TempData["BuilidingId"] = regManager.BuilidingId.ToString();
           
                return RedirectToAction("ManagerHomePage");     
            }
            else
            {
                manager.loginErrorMessage = "Invalid Login";
                return View("ManagerLogin", manager);
            }
        }

        public ActionResult ManagerHomePage()
        {
            if (TempData["ManagerId"] != null)
            {
                TempData["ManagerId"].ToString();
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}
