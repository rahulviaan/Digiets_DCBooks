
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Database
{

using System;
    using System.Collections.Generic;
    
public partial class UserCartItem
{

    public string Id { get; set; }

    public string UserCartId { get; set; }

    public string MasterBookId { get; set; }

    public string CartUniqueId { get; set; }

    public Nullable<decimal> EbookPrice { get; set; }

    public Nullable<decimal> Discount { get; set; }

    public Nullable<int> Quantity { get; set; }

    public Nullable<decimal> SubTotal { get; set; }

    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public Nullable<System.DateTime> CreateDate { get; set; }

    public Nullable<System.DateTime> UpdateDate { get; set; }

    public Nullable<byte> Status { get; set; }

    public string IPaddress { get; set; }



    public virtual AspNetUser AspNetUser { get; set; }

    public virtual AspNetUser AspNetUser1 { get; set; }

    public virtual MasterBook MasterBook { get; set; }

    public virtual UserCart UserCart { get; set; }

    public virtual UserCartItem UserCartItem1 { get; set; }

    public virtual UserCartItem UserCartItem2 { get; set; }

}

}
