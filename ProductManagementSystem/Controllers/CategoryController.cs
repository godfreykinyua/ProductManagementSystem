using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        private ProductManagementEntities db = new ProductManagementEntities();

        // GET: Category
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View(db.CATEGORies.ToList());
        }

        // GET: Category/Details/5
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
            CATEGORY cATEGORY = db.CATEGORies.Find(id);
            if (cATEGORY == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORY);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( CATEGORY cATEGORY)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            //Check if category name exists
            try
            {
                var cat = db.CATEGORies.SingleOrDefault(x => x.CATEGORY_NAME == cATEGORY.CATEGORY_NAME);
                if (cat == null)
                {
                    if (ModelState.IsValid)
                    {
                        cATEGORY.CreatedBy = User.Identity.GetUserId();
                        cATEGORY.CreatedAt = DateTime.Now;
                        db.CATEGORies.Add(cATEGORY);
                        db.SaveChanges();
                        TempData["success"] = "Category successfully created";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "Category with the name " + cATEGORY.CATEGORY_NAME + " already exists";
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            

            return View(cATEGORY);
        }

        // GET: Category/Edit/5
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
            CATEGORY cATEGORY = db.CATEGORies.Find(id);
            if (cATEGORY == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORY);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( CATEGORY cATEGORY)
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
                    db.Entry(cATEGORY).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["success"] = "Category successfully updated";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            return View(cATEGORY);
        }

        // GET: Category/Delete/5
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
            CATEGORY cATEGORY = db.CATEGORies.Find(id);
            if (cATEGORY == null)
            {
                return HttpNotFound();
            }
            return View(cATEGORY);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            CATEGORY cATEGORY = db.CATEGORies.Find(id);
            db.CATEGORies.Remove(cATEGORY);
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
