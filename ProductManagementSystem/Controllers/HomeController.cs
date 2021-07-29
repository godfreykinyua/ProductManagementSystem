using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private ProductManagementEntities db = new ProductManagementEntities();
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var standardProducts = db.PRODUCTs.Where(x => x.PRODUCT_TypeID == 1).ToList();
                Session["standardProducts"] = standardProducts.Count;
                var variantProducts = db.PRODUCTs.Where(x => x.PRODUCT_TypeID == 2).ToList();
                Session["variantProducts"] = variantProducts.Count;
                var compositeProducts = db.PRODUCTs.Where(x => x.PRODUCT_TypeID == 3).ToList();
                Session["compositeProducts"] = compositeProducts.Count;
                var allProducts = db.PRODUCTs.ToList();
                Session["allProducts"] = allProducts.Count;
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}