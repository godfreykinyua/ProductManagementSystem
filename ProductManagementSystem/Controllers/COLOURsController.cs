using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class COLOURsController : Controller
    {
        private ProductManagementEntities db = new ProductManagementEntities();

        // GET: COLOURs
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View(db.COLOURs.ToList());
        }

        // GET: COLOURs/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COLOUR cOLOUR = db.COLOURs.Find(id);
            if (cOLOUR == null)
            {
                return HttpNotFound();
            }
            return View(cOLOUR);
        }

        // GET: COLOURs/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: COLOURs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COLOUR cOLOUR)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                //Check if the color exists
                var color = db.COLOURs.SingleOrDefault(x => x.COLOUR_NAME == cOLOUR.COLOUR_NAME);
                if (color == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.COLOURs.Add(cOLOUR);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "Colour with the name " + cOLOUR.COLOUR_NAME + " already exists";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;


            }

            

            return View(cOLOUR);
        }

        // GET: COLOURs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COLOUR cOLOUR = db.COLOURs.Find(id);
            if (cOLOUR == null)
            {
                return HttpNotFound();
            }
            return View(cOLOUR);
        }

        // POST: COLOURs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COLOUR cOLOUR)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cOLOUR).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            return View(cOLOUR);
        }

        // GET: COLOURs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COLOUR cOLOUR = db.COLOURs.Find(id);
            if (cOLOUR == null)
            {
                return HttpNotFound();
            }
            return View(cOLOUR);
        }

        // POST: COLOURs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            COLOUR cOLOUR = db.COLOURs.Find(id);
            db.COLOURs.Remove(cOLOUR);
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
