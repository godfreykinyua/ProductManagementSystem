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
    public class ProductTypesController : Controller
    {
        private ProductManagementEntities db = new ProductManagementEntities();

        // GET: ProductTypes
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View(db.ProductTypes.ToList());
        }

        // GET: ProductTypes/Details/5
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
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // GET: ProductTypes/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductType productType)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            // check if productType exists
            var pType = db.ProductTypes.SingleOrDefault(x => x.ProductTypeName == productType.ProductTypeName);
            if(pType == null)
            {
                if (ModelState.IsValid)
                {
                    db.ProductTypes.Add(productType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["error"] = "Product type with name " + productType.ProductTypeName + " already exists";
            }
            

            return View(productType);
        }

        // GET: ProductTypes/Edit/5
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
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProductType productType)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                db.Entry(productType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productType);
        }

        // GET: ProductTypes/Delete/5
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
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            ProductType productType = db.ProductTypes.Find(id);
            db.ProductTypes.Remove(productType);
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
