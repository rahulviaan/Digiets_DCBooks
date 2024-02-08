
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
    
public partial class MasterClass
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MasterClass()
    {

        this.MasterBooks = new HashSet<MasterBook>();

        this.UserDetails = new HashSet<UserDetail>();

    }


    public long Id { get; set; }

    public string Title { get; set; }

    public Nullable<int> DisplayOrder { get; set; }

    public string Description { get; set; }

    public System.DateTime CreateDate { get; set; }

    public System.DateTime UpdatedDate { get; set; }

    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public string IPAddress { get; set; }

    public int Status { get; set; }



    public virtual AspNetUser AspNetUser { get; set; }

    public virtual AspNetUser AspNetUser1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MasterBook> MasterBooks { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserDetail> UserDetails { get; set; }

}

}
