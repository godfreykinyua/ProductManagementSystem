using ProductManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductManagementSystem.ViewModels
{
    public class ProductVM
    {
        //product properties
        public int PRODUCT_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_DESCRIPTION { get; set; }
        public Nullable<double> PRODUCT_COST { get; set; }
        public Nullable<double> PRODUCT_PRICE { get; set; }
        public Nullable<int> PRODUCT_TypeID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> PRODUCT_ORDER_NUMBER { get; set; }
        public Nullable<System.DateTime> PRODUCT_CREATE_DATE { get; set; }
        public string PRODUCT_CODE { get; set; }
        public Nullable<int> PRODUCT_QTY { get; set; }
        public Nullable<int> Status { get; set; }
        //product variant properties
        public int ProductVariantID { get; set; }
        public Nullable<double> ProductPrice { get; set; }
        public Nullable<int> ProductQty { get; set; }
        public string SKU { get; set; }
        public Nullable<double> ProductCost { get; set; }
        public Nullable<int> ColourId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public string productName { get; set; }
        //composite product properties
        public int CompositeId { get; set; }
        public Nullable<int> MainProductId { get; set; }
        public Nullable<int> CompositeProductId { get; set; }
        public Nullable<double> ExtraPrice { get; set; }
        public bool Mandatory { get; set; }
        public bool Costed { get; set; }
        public string CompositeProductname { get; set; }
        public string MainProductname { get; set; }

        public List<ProductVariants> vList { get; set; }
        public List<CompositeProducts> cList { get; set; }
        public List<PRODUCT> PList { get; set; }

    }

    public class ProductVariants
    {
        public int ProductVariantID { get; set; }

        public Nullable<int> ProductId { get; set; }
        public Nullable<double> ProductPrice { get; set; }
        public Nullable<int> ProductQty { get; set; }
        public string SKU { get; set; }
        public Nullable<double> ProductCost { get; set; }
        public Nullable<int> ColourId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public string productName { get; set; }

        public string ColourName { get; set; }
        public string SizeName { get; set; }
    }
    public class CompositeProducts
    {
        public int CompositeId { get; set; }
        public Nullable<int> MainProductId { get; set; }
        public Nullable<int> CompositeProductId { get; set; }
        public Nullable<double> ExtraPrice { get; set; }
        public bool Mandatory { get; set; }
        public bool Costed { get; set; }
        public string CompositeProductname { get; set; }
        public string MainProductname { get; set; }
    }
}