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
    public class SIZEsController : Controller
    {
        private ProductManagementEntities db = new ProductManagementEntities();

        // GET: SIZEs
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View(db.SIZEs.ToList());
        }

        // GET: SIZEs/Details/5
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
            SIZE sIZE = db.SIZEs.Find(id);
            if (sIZE == null)
            {
                return HttpNotFound();
            }
            return View(sIZE);
        }

        // GET: SIZEs/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: SIZEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( SIZE sIZE)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            //check if the  size name exists
            try
            {
                var siz = db.SIZEs.SingleOrDefault(x => x.SIZE_NAME == sIZE.SIZE_NAME);
                if (siz == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.SIZEs.Add(sIZE);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["error"] = "Size with the name " + sIZE.SIZE_NAME + " already exists";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            

            return View(sIZE);
        }

        // GET: SIZEs/Edit/5
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
            SIZE sIZE = db.SIZEs.Find(id);
            if (sIZE == null)
            {
                return HttpNotFound();
            }
            return View(sIZE);
        }

        // POST: SIZEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SIZE sIZE)
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
                    db.Entry(sIZE).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(sIZE);
        }

        // GET: SIZEs/Delete/5
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
            SIZE sIZE = db.SIZEs.Find(id);
            if (sIZE == null)
            {
                return HttpNotFound();
            }
            return View(sIZE);
        }

        // POST: SIZEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            SIZE sIZE = db.SIZEs.Find(id);
            db.SIZEs.Remove(sIZE);
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
