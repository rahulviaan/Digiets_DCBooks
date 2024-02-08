
//namespace DC.Web.App.Models
//{
//    public class CategoryViewModel
//    {
//        public short Id { get; set; }
//        [Required(ErrorMessage = "Please enter the category name.")]
//        [Display(Name = "Category Name")]
//        public string Title { get; set; }
//        [Display(Name = "Category Name")]
//        [Required(ErrorMessage = "Please enter the category name.")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Parent Category")]
//        public short? ParentId { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Display(Name = "Image")]       
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();
//        public string ParentCategory { get; set; }
//        [Display(Name = "Gender")]
//        public GenderCategory? Gender { get; set; } = GenderCategory.NA;         
//        [Display(Name = "Type")]
//        public CategoryType? Type { get; set; } = CategoryType.NA;
//        public string OldImage { get; set; }

//    }
//    public class TypeViewModel
//    {
//        public short Id { get; set; }
//        [Required(ErrorMessage = "Please enter the size name.")]
//        [Display(Name = "Type Name")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the size name.")]
//        [Display(Name = "Type Name")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();

//    }

//    public partial class PassowrdViewModel
//    {
//        public long Id { get; set; }

//        [Required(ErrorMessage = "Please enter the title.")]
//        [Display(Name = "Title")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the title.")]
//        [Display(Name = "Title")]
//        public string vTitle { get; set; }
//        [Required(ErrorMessage = "Please enter the password.")]
//        [Display(Name = "Password")]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }
//        [Required(ErrorMessage = "Please enter the controller.")]
//        [Display(Name = "Controller")]
//        public string Controller { get; set; }
//        [Required(ErrorMessage = "Please enter the action.")]
//        [Display(Name = "Action")]
//        public string Action { get; set; }
//        [Required(ErrorMessage = "Page Url.")]
//        [Display(Name = "Page Url")]
//        public string PageUrl { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();
//    }
//    public class PackingTypeViewModel
//    {
//        public long? Id { get; set; }
//        [Required(ErrorMessage = "Please enter the packing type name.")]
//        [Display(Name = "Type Name")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the packing type name.")]
//        [Display(Name = "Type Name")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();

//    }
//    public class SizeViewModel
//    {
//        public long Id { get; set; }
//        [Required(ErrorMessage = "Please select packing type.")]
//        [Display(Name = "Packing Type")]
//        public long? PackingTypeId { get; set; }
//        [Required(ErrorMessage = "Please enter the size name.")]
//        [Display(Name = "Size Name")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the size name.")]
//        [Display(Name = "Size Name")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Required(ErrorMessage = "Required.")]
//        [Display(Name = "Size Unit")]
//        public Unit? Unit { get; set; }
//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;

//        public string IPaddress { get; set; } = Utility.GetIpAddress();

//    }

//    public class SpecialFeatureViewModel
//    {
//        public long? Id { get; set; }
//        [Required(ErrorMessage = "Please enter the special feature name.")]
//        [Display(Name = "Special Feature")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the special feature name.")]
//        [Display(Name = "Special Feature")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();

//    }
//    public class SpecialFeatureItemViewModel
//    {
//        public long Id { get; set; }
//        [Required(ErrorMessage = "Please select special feature.")]
//        [Display(Name = "Special Feature")]
//        public long? SpecialFeatureId { get; set; }
//        [Required(ErrorMessage = "Please enter the feature item name.")]
//        [Display(Name = "Feature Item Name")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the feature item name.")]
//        [Display(Name = "Feature Item Name")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }

//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;

//        public string IPaddress { get; set; } = Utility.GetIpAddress();

//    }
//    public class ClassViewModel
//    {
//        public short Id { get; set; }
//        [Required(ErrorMessage = "Please enter the class name.")]
//        [Display(Name = "Type Name")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please enter the class name.")]
//        [Display(Name = "Type Name")]
//        public string vTitle { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Display Order")]
//        public short? DisplayOrder { get; set; }
//        [Display(Name = "Hide")]
//        public bool? Hide { get; set; }
//        [Display(Name = "Image")]
//        public string Image { get; set; } = "";
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();

//    }
//    public class CountryViewModel
//    {
//        public long CountryCode { get; set; }
//        [Required(ErrorMessage = "Please enter the country name.")]
//        [Display(Name = "Country Name")]
//        public string CountryName { get; set; }
//        [Display(Name = "Capital")]
//        public string Capital { get; set; }
//        [Display(Name = "Population")]
//        public decimal? Population { get; set; }
//        [Display(Name = "Lattitude")]
//        public decimal? Lattitude { get; set; }
//        [Display(Name = "Longitude")]
//        public decimal? Longitude { get; set; }
//        [Display(Name = "Lat Long Address")]
//        public string LatLongAddress { get; set; }
//        [Display(Name = "Address")]
//        public string ActAddress { get; set; }
//        [Display(Name = "Calling Code")]
//        public string IntCallingCode { get; set; }
//        public string Image { get; set; }
//        public long? DisplayOrder { get; set; }
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();


//    }
//    public class StateViewModel
//    {
//        public long StateCode { get; set; }
//        [Required(ErrorMessage = "Please select the country.")]
//        [Display(Name = "Country")]
//        public long? CountryCode { get; set; } 
//        [Required(ErrorMessage = "Please enter the state name.")]
//        [Display(Name = "State Name")]
//        public string StateName { get; set; }
//        [Display(Name = "Capital")]
//        public string Capital { get; set; }
//        [Display(Name = "Population")]
//        public decimal? Population { get; set; }
//        [Display(Name = "Lattitude")]
//        public decimal? Lattitude { get; set; }
//        [Display(Name = "Longitude")]
//        public decimal? Longitude { get; set; }
//        [Display(Name = "Lat Long Address")]
//        public string LatLongAddress { get; set; }
//        [Display(Name = "Address")]
//        public string ActAddress { get; set; }
//        [Display(Name = "Calling Code")]
//        public string CallingCode { get; set; }
//        [Display(Name = "GST State Char")]
//        public string GstStateChar { get; set; }
//        [Display(Name = "GST State Code")]
//        public string GstStateCode { get; set; }
//        public string Image { get; set; }
//        public long? DisplayOrder { get; set; }
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPAddress { get; set; } = Utility.GetIpAddress();
//        [Display(Name = "Union Territory")]
//        public bool UnionTerritory { get; set; } 
//    }
//    public class CityViewModel
//    {
//        public long CityCode { get; set; }
//        [Required(ErrorMessage = "Please select the state.")]
//        [Display(Name = "State")]
//        public long? StateCode { get; set; }
//        [Required(ErrorMessage = "Please select the country.")]
//        [Display(Name = "Country")]
//        public long? CountryCode { get; set; }
//        [Required(ErrorMessage = "Please enter the city name.")]
//        [Display(Name = "City Name")]
//        public string CityName { get; set; }

//        [Display(Name = "Pin Code")]
//        public string PinCode { get; set; }

//        [Display(Name = "Population")]
//        public decimal? Population { get; set; }
//        [Display(Name = "Lattitude")]
//        public decimal? Lattitude { get; set; }
//        [Display(Name = "Longitude")]
//        public decimal? Longitude { get; set; }
//        [Display(Name = "Lat Long Address")]
//        public string LatLongAddress { get; set; }
//        [Display(Name = "Address")]
//        public string ActAddress { get; set; }
//        [Display(Name = "Calling Code")]
//        public string CallingCode { get; set; }

//        public string Image { get; set; }
//        public long? DisplayOrder { get; set; }
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPAddress { get; set; } = Utility.GetIpAddress();

//    }
//    public class AreaViewModel
//    {
//        public long AreaCode { get; set; }

//        [Required(ErrorMessage = "Please select the city.")]
//        [Display(Name = "City")]
//        public long? CityCode { get; set; }
//        [Required(ErrorMessage = "Please select the state.")]
//        [Display(Name = "State")]
//        public long? StateCode { get; set; }
//        [Required(ErrorMessage = "Please select the country.")]
//        [Display(Name = "Country")]
//        public long? CountryCode { get; set; }
//        [Required(ErrorMessage = "Please enter the city name.")]
//        [Display(Name = "Area Name")]
//        public string AreaName { get; set; }
//        [Display(Name = "Pin Code")]
//        public string PinCode { get; set; }
//        [Display(Name = "Population")]
//        public decimal? Population { get; set; }
//        [Display(Name = "Lattitude")]
//        public decimal? Lattitude { get; set; }
//        [Display(Name = "Longitude")]
//        public decimal? Longitude { get; set; }
//        [Display(Name = "Lat Long Address")]
//        public string LatLongAddress { get; set; }
//        [Display(Name = "Address")]
//        public string ActAddress { get; set; }
//        [Display(Name = "Calling Code")]
//        public string CallingCode { get; set; }
//        public string Image { get; set; }
//        public long? DisplayOrder { get; set; }
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPAddress { get; set; } = Utility.GetIpAddress();
//    }
//    public class CompanyViewModel
//    {
//        public long Id { get; set; }
//        [Required(ErrorMessage = "Please select the company name.")]
//        [Display(Name = "Company Name")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please select the company name.")]
//        [Display(Name = "Company Name")]
//        public string vTitle { get; set; }
//        [Display(Name = "Contact No.")]
//        public string ContactNo { get; set; }
//        [Display(Name = "Preferred Call Time")]
//        public string CallTime { get; set; }
//        [Display(Name = "Description")]
//        public string Description { get; set; }
//        [Display(Name = "Short Code")]
//        public string ShortCode { get; set; }
//        [Display(Name = "Area")]
//        public string Area { get; set; }
//        public long? AreaCode { get; set; }        
//        [Display(Name = "City")]
//        public long? CityCode { get; set; }
//        public string City { get; set; }
//        [Display(Name = "State")]
//        public long? StateCode { get; set; }
//        public string State { get; set; }
//        [Display(Name = "Country")]
//        public long? CountryCode { get; set; }
//        public string Country { get; set; }
//        [Display(Name = "Pin Code")]
//        public string PinCode { get; set; }
//        public decimal? Lattitude { get; set; }
//        [Display(Name = "Longitude")]
//        public decimal? Longitude { get; set; }
//        [Display(Name = "Lat Long Address")]
//        public string LatLongAddress { get; set; }
//        [Display(Name = "Address")]
//        public string ActAddress { get; set; } 
//        public string Image { get; set; }
//        public long? DisplayOrder { get; set; }
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();     
//        public bool? Hide { get; set; }
//    }
//    public class HSNViewModel
//    { 
//        public long Id { get; set; }
//        [Required(ErrorMessage = "Please select the company name.")]
//        [Display(Name = "HSN")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please select the company name.")]
//        [Display(Name = "HSN")]
//        public string vTitle { get; set; }
//        [Display(Name = "Goods and Services Tax")]        
//        public decimal? GSTTax { get; set; }
//        [Display(Name = "Central GST")]        
//        public decimal? CGSTTax { get; set; }
//        [Display(Name = "State GST")]      
//        public decimal? SGSTTax { get; set; }
//        [Display(Name = "Integrated  GST")]        
//        public decimal? IGSTTax { get; set; }
//        [Display(Name = "Union Territory GST")]        
//        public decimal? UGSTTax { get; set; }
//        [Display(Name = "Cess Tax")]       
//        public decimal? CessTax { get; set; }
//        [Display(Name = "Other Tax")]
//        public decimal? OtherTax { get; set; }
//        public string Description { get; set; }        
//        public int? DisplayOrder { get; set; }
//        public string CreatedBy { get; set; }
//        public string UpdatedBy { get; set; }
//        public string DeletedBy { get; set; }
//        public DateTime? dtmCreate { get; set; } = DateTime.Now;
//        public DateTime? dtmUpdate { get; set; }
//        public Status Status { get; set; } = Status.Active;
//        public string IPaddress { get; set; } = Utility.GetIpAddress();
//        public bool? Hide { get; set; } 

//    }
//    public class UserUploadViewModel
//    {
//        public string Id { get; set; }
//        [Required(ErrorMessage = "Please select the title.")]
//        [Display(Name = "Title")]
//        public string Title { get; set; }
//        [Required(ErrorMessage = "Please select the quantity.")]
//        [Display(Name = "Quantity")]
//        public int? Quantity { get; set; }        
//        [Display(Name = "Display Order")]
//        public int? Order { get; set; }       

//    }
//}