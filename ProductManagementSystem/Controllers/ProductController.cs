using ProductManagementSystem.Models;
using ProductManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace ProductManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        private ProductManagementEntities db = new ProductManagementEntities();
        // GET: Product
        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var product = db.PRODUCTs.Include(x => x.ProductType).Include(x => x.CATEGORY);
                if (product != null)
                {
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View();

        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            ProductVM productVM = new ProductVM();
            try
            {
                var category = db.CATEGORies.ToList();
                SelectList list = new SelectList(category, "CATEGORYID", "CATEGORY_NAME");
                ViewBag.List = list;

                var ptype = db.ProductTypes.ToList();
                SelectList list1 = new SelectList(ptype, "ProductTypeID", "ProductTypeName");
                ViewBag.List1 = list1;
                var product = db.PRODUCTs.ToList();
                SelectList pList = new SelectList(product, "PRODUCT_ID", "PRODUCT_NAME");
                ViewBag.List2 = pList;

                var color = db.COLOURs.ToList();
                SelectList pColor = new SelectList(color, "COLOUR_ID", "COLOUR_NAME");
                ViewBag.List3 = pColor;

                var size = db.SIZEs.ToList();
                SelectList pSize = new SelectList(size, "SIZE_ID", "SIZE_NAME");
                ViewBag.List4 = pSize;
                int id = 0;
               
                    id = Convert.ToInt32(Session["ProductId"]);
                
              
              
                List<PRODUCT> productLists = new List<PRODUCT>();
                var productList = db.PRODUCTs.Where(x => x.PRODUCT_ID == id).ToList();
                foreach (var item in productList)
                {
                    PRODUCT prod = new PRODUCT();
                    prod.PRODUCT_NAME = item.PRODUCT_NAME;
                    prod.PRODUCT_DESCRIPTION = item.PRODUCT_DESCRIPTION;
                    prod.PRODUCT_ID = item.PRODUCT_ID;
                    prod.PRODUCT_PRICE = item.PRODUCT_PRICE;
                    prod.PRODUCT_QTY = item.PRODUCT_QTY;
                    prod.PRODUCT_COST = item.PRODUCT_COST;
                    prod.PRODUCT_CODE = item.PRODUCT_CODE;
                    prod.PRODUCT_TypeID = item.PRODUCT_TypeID;
                    prod.CategoryID = item.CategoryID;

                }
                //populate product variant table
                List<ProductVariants> pVariantList = new List<ProductVariants>();
                var productVariant = db.ProductVariants.Include(x=>x.PRODUCT).Where(x => x.ProductId == id).ToList();
                foreach (var item in productVariant)
                {
                    ProductVariants variant = new ProductVariants();
                    variant.ProductVariantID = item.ProductVariantID;
                    variant.ProductId = item.ProductId;
                    variant.ColourName = item.COLOUR.COLOUR_NAME;
                    variant.SizeName = item.SIZE.SIZE_NAME;
                    variant.ProductPrice = item.ProductPrice;
                    variant.ProductCost = item.ProductCost;
                    variant.ProductQty = item.ProductQty;
                    variant.SizeId = item.SizeId;
                    variant.ColourId = item.ColourId;
                    variant.SKU = item.SKU;
                    variant.productName = item.PRODUCT.PRODUCT_NAME;
                    pVariantList.Add(variant);
                }
                productVM.vList = pVariantList;

                //populate composite product table
                int pid = 0;
                if(Session["compositeId"] != null)
                {
                    pid = Convert.ToInt32(Session["compositeId"]);
                }

                List<CompositeProducts> cPList = new List<CompositeProducts>();
                var compositeProduct = from x in db.CompositeProducts
                                       join y in db.PRODUCTs on x.MainProductId equals y.PRODUCT_ID
                                       where x.CompositeProductId == pid 
                                       select new ProductVM
                                       {
                                           CompositeId = x.CompositeId,
                                           MainProductId = x.MainProductId,
                                           CompositeProductId = x.CompositeProductId,
                                           Mandatory = (bool)x.Mandatory,
                                           Costed = (bool)x.Costed,
                                           ExtraPrice= x.ExtraPrice,
                                           productName = y.PRODUCT_NAME,
                                           MainProductname = y.PRODUCT_NAME


                                       };
                foreach (var item in compositeProduct)
                {
                    CompositeProducts prod = new CompositeProducts();
                    prod.CompositeId = item.CompositeId;
                    prod.MainProductId = item.MainProductId;
                    prod.Mandatory = item.Mandatory;
                    prod.Costed = item.Costed;
                    prod.ExtraPrice = item.ExtraPrice;
                    prod.MainProductname = item.productName;
                    prod.CompositeProductname = item.productName;
                    cPList.Add(prod);
                }
                productVM.cList = cPList;
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM model)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 0;
            try
            {
                var product = db.PRODUCTs.SingleOrDefault(x => x.PRODUCT_NAME == model.PRODUCT_NAME);
                if(product == null)
                {
                    //Code block for creating standard product
                    if (model.PRODUCT_TypeID == 1)
                    {
                        PRODUCT prod = new PRODUCT();
                        prod.PRODUCT_NAME = model.PRODUCT_NAME;
                        prod.PRODUCT_DESCRIPTION = model.PRODUCT_DESCRIPTION;
                        prod.PRODUCT_COST = model.PRODUCT_COST;
                        prod.PRODUCT_PRICE = model.PRODUCT_PRICE;
                        prod.PRODUCT_QTY = model.PRODUCT_QTY;
                        prod.PRODUCT_TypeID = model.PRODUCT_TypeID;
                        prod.CategoryID = model.CategoryID;
                        prod.PRODUCT_CODE = model.PRODUCT_CODE;
                        prod.Status = 1;
                        prod.PRODUCT_CREATE_DATE = DateTime.Now;
                        db.PRODUCTs.Add(prod);
                        db.SaveChanges();
                        Session["ProductId"] = prod.PRODUCT_ID;
                        TempData["success"] = "Product with Id " + prod.PRODUCT_ID + " successfully created";
                        return RedirectToAction("Create", "Product", new { step = 1 });
                    }
                    //Code block for creating product with variant
                    else if (model.PRODUCT_TypeID == 2)
                    {
                        PRODUCT prod = new PRODUCT();
                        prod.PRODUCT_NAME = model.PRODUCT_NAME;
                        prod.PRODUCT_DESCRIPTION = model.PRODUCT_DESCRIPTION;
                        prod.PRODUCT_TypeID = model.PRODUCT_TypeID;
                        prod.CategoryID = model.CategoryID;
                        prod.PRODUCT_CODE = model.PRODUCT_CODE;
                        prod.PRODUCT_COST = 0;
                        prod.PRODUCT_PRICE = 0;
                        prod.PRODUCT_QTY = 0;
                        prod.Status = 1;
                        prod.PRODUCT_CREATE_DATE = DateTime.Now;
                        db.PRODUCTs.Add(prod);

                        db.SaveChanges();
                        Session["ProductId"] = prod.PRODUCT_ID;
                        TempData["success"] = "Product with Id " + prod.PRODUCT_ID + " successfully created";
                        return RedirectToAction("Create", "Product", new { step = 2 });
                    }
                    //Code block for creating product with composite product
                    else
                    {
                        PRODUCT prod = new PRODUCT();
                        prod.PRODUCT_NAME = model.PRODUCT_NAME;
                        prod.PRODUCT_DESCRIPTION = model.PRODUCT_DESCRIPTION;
                        prod.PRODUCT_COST = model.PRODUCT_COST;
                        prod.PRODUCT_PRICE = model.PRODUCT_PRICE;
                        prod.PRODUCT_QTY = model.PRODUCT_QTY;
                        prod.PRODUCT_TypeID = model.PRODUCT_TypeID;
                        prod.CategoryID = model.CategoryID;
                        prod.PRODUCT_CODE = model.PRODUCT_CODE;
                        prod.Status = 1;
                        prod.PRODUCT_CREATE_DATE = DateTime.Now;
                        db.PRODUCTs.Add(prod);
                        db.SaveChanges();
                        Session["ProductId"] = prod.PRODUCT_ID;
                        TempData["success"] = "Product with Id " + prod.PRODUCT_ID + " successfully created";
                        return RedirectToAction("Create", "Product", new { step = 3 });
                    }
                }
                else
                {
                    TempData["error"] = "Product with name " + model.PRODUCT_NAME + " Already exists";
                    return RedirectToAction("Create", "Product", new { step = 1 });
                }
                
            
               
  
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            var category = db.CATEGORies.ToList();
            SelectList list = new SelectList(category, "CATEGORYID", "CATEGORY_NAME");
            ViewBag.List = list;

            var ptype = db.ProductTypes.ToList();
            SelectList list1 = new SelectList(ptype, "ProductTypeID", "ProductTypeName");
            ViewBag.List1 = list1;
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            Session["ProductId"] = id;
            ProductVM productVM = new ProductVM();
            try
            {
                var category = db.CATEGORies.ToList();
                SelectList list = new SelectList(category, "CATEGORYID", "CATEGORY_NAME");
                ViewBag.List = list;

                var ptype = db.ProductTypes.ToList();
                SelectList list1 = new SelectList(ptype, "ProductTypeID", "ProductTypeName");
                ViewBag.List1 = list1;
                var product = db.PRODUCTs.ToList();
                SelectList pList = new SelectList(product, "PRODUCT_ID", "PRODUCT_NAME");
                ViewBag.List2 = pList;

                var color = db.COLOURs.ToList();
                SelectList pColor = new SelectList(color, "COLOUR_ID", "COLOUR_NAME");
                ViewBag.List3 = pColor;

                var size = db.SIZEs.ToList();
                SelectList pSize = new SelectList(size, "SIZE_ID", "SIZE_NAME");
                ViewBag.List4 = pSize;
                


                List<PRODUCT> productLists = new List<PRODUCT>();
                var productList = db.PRODUCTs.Where(x => x.PRODUCT_ID == id).ToList();
                foreach (var item in productList)
                {
                    PRODUCT prod = new PRODUCT();
                    productVM.PRODUCT_NAME = item.PRODUCT_NAME;
                    productVM.PRODUCT_DESCRIPTION = item.PRODUCT_DESCRIPTION;
                    productVM.PRODUCT_ID = item.PRODUCT_ID;
                    productVM.PRODUCT_PRICE = item.PRODUCT_PRICE;
                    productVM.PRODUCT_QTY = item.PRODUCT_QTY;
                    productVM.PRODUCT_COST = item.PRODUCT_COST;
                    productVM.PRODUCT_CODE = item.PRODUCT_CODE;
                    productVM.PRODUCT_TypeID = item.PRODUCT_TypeID;
                    productVM.CategoryID = item.CategoryID;
                }

                //populate product variant table
                List<ProductVariants> pVariantList = new List<ProductVariants>();
                var productVariant = db.ProductVariants.Include(x => x.PRODUCT).Where(x => x.ProductId == id).ToList();
                foreach (var item in productVariant)
                {
                    ProductVariants variant = new ProductVariants();
                    variant.ProductVariantID = item.ProductVariantID;
                    variant.ProductId = item.ProductId;
                    variant.ColourName = item.COLOUR.COLOUR_NAME;
                    variant.SizeName = item.SIZE.SIZE_NAME;
                    variant.ProductPrice = item.ProductPrice;
                    variant.ProductCost = item.ProductCost;
                    variant.ProductQty = item.ProductQty;
                    variant.SizeId = item.SizeId;
                    variant.ColourId = item.ColourId;
                    variant.SKU = item.SKU;
                    variant.productName = item.PRODUCT.PRODUCT_NAME;
                    pVariantList.Add(variant);
                }
                productVM.vList = pVariantList;

                //populate composite product table
                int pid = 0;
                if (Session["compositeId"] != null)
                {
                    pid = Convert.ToInt32(Session["compositeId"]);
                }

                List<CompositeProducts> cPList = new List<CompositeProducts>();
                var compositeProduct = from x in db.CompositeProducts
                                       join y in db.PRODUCTs on x.MainProductId equals y.PRODUCT_ID
                                       where x.CompositeProductId == pid
                                       select new ProductVM
                                       {
                                           CompositeId = x.CompositeId,
                                           MainProductId = x.MainProductId,
                                           Mandatory = (bool) x.Mandatory,
                                           Costed = (bool)x.Costed,
                                           ExtraPrice = x.ExtraPrice,
                                           productName = y.PRODUCT_NAME,
                                           MainProductname = y.PRODUCT_NAME


                                       };
                foreach (var item in compositeProduct)
                {
                    CompositeProducts prod = new CompositeProducts();
                    prod.CompositeId = item.CompositeId;
                    prod.MainProductId = item.MainProductId;
                    prod.Mandatory = item.Mandatory;
                    prod.Costed = item.Costed;
                    prod.ExtraPrice = item.ExtraPrice;

                    prod.MainProductname = item.productName;
                    prod.CompositeProductname = item.productName;
                    cPList.Add(prod);
                }
                productVM.cList = cPList;
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            
            return View(productVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVM model)
        {
            //Code block for updating standard product
            try
            {
                if (model.PRODUCT_TypeID == 1)
                {
                    int id = Convert.ToInt32(Session["ProductId"]);
                    PRODUCT prod = db.PRODUCTs.Find(id);
                    prod.PRODUCT_NAME = model.PRODUCT_NAME;
                    prod.PRODUCT_DESCRIPTION = model.PRODUCT_DESCRIPTION;
                    prod.PRODUCT_COST = model.PRODUCT_COST;
                    prod.PRODUCT_PRICE = model.PRODUCT_PRICE;
                    prod.PRODUCT_QTY = model.PRODUCT_QTY;
                    prod.PRODUCT_TypeID = model.PRODUCT_TypeID;
                    prod.CategoryID = model.CategoryID;
                    prod.PRODUCT_CODE = model.PRODUCT_CODE;
                    db.Entry(prod).State = EntityState.Modified;
                    db.PRODUCTs.Add(prod);
                    db.SaveChanges();
                    TempData["success"] = "Product with Id " + prod.PRODUCT_ID + " successfully updated";
                    return RedirectToAction("Edit", "Product", new { step = 1 });
                }
                //Code block for updating product with variant
                else if (model.PRODUCT_TypeID == 2)
                {
                    int id = Convert.ToInt32(Session["ProductId"]);
                    PRODUCT prod = db.PRODUCTs.Find(id);
                    prod.PRODUCT_NAME = model.PRODUCT_NAME;
                    prod.PRODUCT_DESCRIPTION = model.PRODUCT_DESCRIPTION;
                    prod.PRODUCT_TypeID = model.PRODUCT_TypeID;
                    prod.CategoryID = model.CategoryID;
                    prod.PRODUCT_CODE = model.PRODUCT_CODE;
                    prod.PRODUCT_COST = 0;
                    prod.PRODUCT_PRICE = 0;
                    prod.PRODUCT_QTY = 0;
                    db.Entry(prod).State = EntityState.Modified;
                    TempData["success"] = "Product with Id " + prod.PRODUCT_ID + " successfully updated";
                    return RedirectToAction("Edit", "Product", new { step = 2 });
                }
                //Code block for updating product with composite product
                else
                {
                    int id = Convert.ToInt32(Session["ProductId"]);
                    PRODUCT prod = db.PRODUCTs.Find(id);
                    prod.PRODUCT_NAME = model.PRODUCT_NAME;
                    prod.PRODUCT_DESCRIPTION = model.PRODUCT_DESCRIPTION;
                    prod.PRODUCT_COST = model.PRODUCT_COST;
                    prod.PRODUCT_PRICE = model.PRODUCT_PRICE;
                    prod.PRODUCT_QTY = model.PRODUCT_QTY;
                    prod.PRODUCT_TypeID = model.PRODUCT_TypeID;
                    prod.CategoryID = model.CategoryID;
                    prod.PRODUCT_CODE = model.PRODUCT_CODE;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["success"] = "Product with Id " + prod.PRODUCT_ID + " successfully updated";
                    return RedirectToAction("Edit", "Product", new { step = 3 });
                }
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }
            var category = db.CATEGORies.ToList();
            SelectList list = new SelectList(category, "CATEGORYID", "CATEGORY_NAME");
            ViewBag.List = list;

            var ptype = db.ProductTypes.ToList();
            SelectList list1 = new SelectList(ptype, "ProductTypeID", "ProductTypeName");
            ViewBag.List1 = list1;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVariant(ProductVM model)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 2;
            try
            {

                ProductVariant variant = new ProductVariant();
                variant.ProductId = Convert.ToInt32(Session["ProductId"]);
                variant.ProductCost = model.ProductCost;
                variant.ProductPrice = model.ProductPrice;
                variant.ProductQty = model.ProductQty;
                variant.ColourId = model.ColourId;
                variant.SizeId = model.SizeId;
                variant.SKU = model.SKU;
                db.ProductVariants.Add(variant);
                db.SaveChanges();
                TempData["success"] = "Product Variant with Id " + variant.ProductVariantID + " successfully created";
                return RedirectToAction("Create", "Product", new { step });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("Create", "Product", new { step });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCompositeProduct(ProductVM model)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 3;
            try
            {
                CompositeProduct compositeProduct = new CompositeProduct();
                compositeProduct.CompositeProductId = model.CompositeProductId;
                Session["compositeId"] = compositeProduct.CompositeProductId;
                compositeProduct.MainProductId = Convert.ToInt32(Session["ProductId"]);
                compositeProduct.Mandatory = model.Mandatory;
                compositeProduct.Costed = model.Costed;
                compositeProduct.ExtraPrice = model.ExtraPrice;
                db.CompositeProducts.Add(compositeProduct);
                db.SaveChanges();

                TempData["success"] = "Product Composite with Id " + model.CompositeProductId + " successfully created";
                return RedirectToAction("Create", "Product", new { step });
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }

            return RedirectToAction("Create", "Product", new { step });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductVariant(ProductVM model)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 2;
            try
            {

                ProductVariant variant = new ProductVariant();
                variant.ProductId = Convert.ToInt32(Session["ProductId"]);
                variant.ProductCost = model.ProductCost;
                variant.ProductPrice = model.ProductPrice;
                variant.ProductQty = model.ProductQty;
                variant.ColourId = model.ColourId;
                variant.SizeId = model.SizeId;
                variant.SKU = model.SKU;
                db.ProductVariants.Add(variant);
                db.SaveChanges();
                TempData["success"] = "Product Variant with Id " + variant.ProductVariantID + " successfully created";
                return RedirectToAction("Create", "Product", new { step });
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("Create", "Product", new { step });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCompositeProduct(ProductVM model)
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 3;
            try
            {
                CompositeProduct compositeProduct = new CompositeProduct();
                compositeProduct.CompositeProductId = model.CompositeProductId;
                Session["compositeId"] = compositeProduct.CompositeProductId;
                compositeProduct.MainProductId = Convert.ToInt32(Session["ProductId"]);
                compositeProduct.Mandatory = model.Mandatory;
                compositeProduct.Costed = model.Costed;
                compositeProduct.ExtraPrice = model.ExtraPrice;
                db.CompositeProducts.Add(compositeProduct);
                db.SaveChanges();

                TempData["success"] = "Product Composite with Id " + model.CompositeProductId + " successfully created";
                return RedirectToAction("Create", "Product", new { step });
            }
            catch (Exception ex)
            {

                TempData["error"] = ex.Message;
            }

            return RedirectToAction("Create", "Product", new { step });
        }

        public ActionResult Delete(int? id)
        {
            int step = 2;
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductVariant variant = db.ProductVariants.Find(id);
            if (variant == null)
            {
                return HttpNotFound();
            }
            db.ProductVariants.Remove(variant);
            db.SaveChanges();
            TempData["success"] = "Product Variants with id " + id + " successfully deleted";
            return RedirectToAction("Create", "Product", new { step });
        }
        public ActionResult DeleteCompositeProduct(int? id)
        {
            int step = 3;
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompositeProduct compositeProduct = db.CompositeProducts.Find(id);
            if (compositeProduct == null)
            {
                return HttpNotFound();
            }
            db.CompositeProducts.Remove(compositeProduct);
            db.SaveChanges();
            TempData["success"] = "Composite Product with id " + id + " successfully deleted";
            return RedirectToAction("Create", "Product", new { step });
        }

        public ActionResult Previous()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 1;
            return RedirectToAction("Create", "Product", new { step });
        }

        public ActionResult Previous1()
        {
            if (Session["Username"] == null)
            {
                TempData["error"] = "Your session has expired.Please Login Again !";
                return RedirectToAction("Login", "Account");
            }
            int step = 1;
            return RedirectToAction("Create", "Product", new { step });
        }
    }
    }