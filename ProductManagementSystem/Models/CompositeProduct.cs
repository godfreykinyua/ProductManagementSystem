//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProductManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompositeProduct
    {
        public int CompositeId { get; set; }
        public Nullable<int> MainProductId { get; set; }
        public Nullable<int> CompositeProductId { get; set; }
        public Nullable<double> ExtraPrice { get; set; }
        public Nullable<bool> Mandatory { get; set; }
        public Nullable<bool> Costed { get; set; }
    }
}
