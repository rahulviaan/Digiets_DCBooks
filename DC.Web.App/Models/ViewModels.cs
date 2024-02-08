using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DC.Web.App.Models
{
    public class PaymentModel
    {
        public string _parameters { get; set; }
        public string strparameters { get; set; }
        public string url { get; set; }
        public string access_code { get; set; }
        public string working_key { get; set; }
    }
    public class KeyValue
    {
        public string Id { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
    }
    public class ViewModel
    {
        public string DatatId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string MapKey { get; set; }

    }
    public class ViewModel<T,U,V>
    {
        public string DatatId { get; set; }
        public int? iDatatId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public T tData { get; set; }
        public U uData { get; set; }
        public V vData { get; set; }

    }
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Student = "Student";
        public const string Teacher = "Teacher";
        public const string School = "School";
    }
    public class RegisterUserModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Please enter your First Name!")]
        [Display(Name = "First Name")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string LastName { get; set; }
        [Display(Name = "D.O.B")]
        public DateTime? DOB { get; set; }
         
        [Required(ErrorMessage = "Please enter your Mobile No.!")]
        [Display(Name = "Mobile No.")]
        [MaxLength(10, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string PhoneNumber { get; set; }
        //[Required(ErrorMessage = "Please enter your E-mail ID!")]
        [EmailAddress]
        [Display(Name = "Email")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        [Required(ErrorMessage = "Please enter your Password!")]
        [MaxLength(40, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [StringLength(100, ErrorMessage = "Your Password must be {2} characters..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter your Confirm Password!")]
        [Compare("Password", ErrorMessage = "Please Enter Same Password!")]
        public string ConfirmPassword { get; set; }
        
        public string MobNo { get; set; }
        public bool MobValidate { get; set; }
        public string AccessLevels { get; set; }
        public string eUserName { get; set; }
        public string ePassword { get; set; }
        [EmailAddress]
        public string EmailId { get; set; }
        public bool EmailValidate { get; set; }
        public DC. LoginMode LoginMode { get; set; }
        public bool LoginThirdParty { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Image { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }
        public DC.Status Status { get; set; }
        public string TimeZone { get; set; }
        public bool Hide { get; set; }
        public int DisplayOrder { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        [Display(Name = "User In Role")]
        [Required(ErrorMessage = "Please select role")]
        public string RoleId { get; set; }
        public string IP { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Gender? Gender { get; internal set; }
        public LoginSourse LoginSourse { get; internal set; }
    }
     
    public class RegisterStudentModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Please enter your First Name!")]
        [Display(Name = "First Name")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string LastName { get; set; }

        public string Name { get; set; }
        public string ContactNo { get; set; }
        [Display(Name = "Iamge")]
        public string Image { get; set; }

        [Display(Name = "D.O.B")]
        public DateTime? DOB { get; set; }
        public int iGender { get; set; }

        [Required(ErrorMessage = "Please enter your Mobile No.!")]
        [Display(Name = "Mobile No.")]
        [MaxLength(10, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your Admission No.!")]
        [Display(Name = "Admission No.")]
        [MaxLength(20, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string AdmissionNo { get; set; }

        [Required(ErrorMessage = "Please enter your Parent Name!")]
        [Display(Name = "Parent Name")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string ParentName { get; set; }

        [Required(ErrorMessage = "Please enter your Account Code!")]
        [Display(Name = "Account Code")]
        [MaxLength(20, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string AccountCode { get; set; }

       

        //[Required(ErrorMessage = "Please enter your E-mail ID!")]
        [EmailAddress]
        [Display(Name = "Email")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        [Required(ErrorMessage = "Please enter your Password!")]
        [MaxLength(40, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        [StringLength(100, ErrorMessage = "Your Password must be {2} characters..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter your Confirm Password!")]
        [Compare("Password", ErrorMessage = "Please Enter Same Password!")]
        public string ConfirmPassword { get; set; }

        public string MobNo { get; set; }
        public bool MobValidate { get; set; }
        public string AccessLevels { get; set; }
        public string eUserName { get; set; }
        public string ePassword { get; set; }
        [EmailAddress]
        public string EmailId { get; set; }
        public bool EmailValidate { get; set; }
        public DC.LoginMode LoginMode { get; set; }
        public bool LoginThirdParty { get; set; }
        public DateTime? LastLogin { get; set; } 
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }
        public DC.Status Status { get; set; }
        public string TimeZone { get; set; }
        public bool Hide { get; set; }
        public int DisplayOrder { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        [Display(Name = "User In Role")]
        [Required(ErrorMessage = "Please select role")]
        public string RoleId { get; set; }
        public string IP { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public Gender? Gender { get; internal set; }
        public LoginSourse LoginSourse { get; internal set; }

      

        [Required(ErrorMessage = "Please select class")]
        [Display(Name = "Class")] 
        public long? MasterClassId { get; set; }

        public string Board { get; set; }
        public string Class { get; set; }
        public string School { get; set; }


        [Required(ErrorMessage = "Please select board")]
        [Display(Name = "Board")]
        public long? MasterBoardId { get; set; }

        [Required(ErrorMessage = "Please select school")]
        [Display(Name = "School")]
        public string SchoolId { get; set; }
        
        [Required(ErrorMessage = "Please enter your Roll No!")]
        [Display(Name = "Roll No")]
        [MaxLength(5, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string RollNo { get; set; }
        [Display(Name = "User Code")]
        public string UserCode { get; set; }
        [Display(Name = "Session")]
        [Required(ErrorMessage = "Please select session!")]
        public string Session { get; set; }
        [Display(Name = "Address")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Address { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime? ExpiyDate { get; set; }
        public Action Action { get; set; }
        public string SchoolCode { get;  set; }
        public string IPAddress { get; set; }
    }
    public class ProductViewModel
    {
        public string Id { get; set; }
        public long ProductId { get; set; }
        [Display(Name = "Currency")]
        public byte? CurrencyId { get; set; }
        [Display(Name = "Packing Type")]
        public long? PackingTypeId { get; set; }
        [Display(Name = "Special Characteristic")]
        public long? MasterSpecialFeatureId { get; set; }
        [Display(Name = "-")]
        public long? MasterSpecialFeatureItemId { get; set; }
        [Display(Name = "HSN Code")]
        public string HSNId { get; set; }
        [Display(Name = "Strength")]
        public string Salt { get; set; }
        [Required(ErrorMessage = "Please enter product name!")]
        [Display(Name = "Product Name")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter product name!")]
        [Display(Name = "Product Name")]
        public string vTitle { get; set; }
        [Display(Name = "Short Name")]
        public string ShortTitle { get; set; }
        [Display(Name = "Brand Name")]
        public string Brand { get; set; }
        [Display(Name = "Company Name")]
        public string Company { get; set; }
        [Display(Name = "Flavour")]
        public string Flavour { get; set; }
        [Display(Name = "Manfacturer Name")]
        public string Manfacturer { get; set; }
        [Display(Name = "Searching Key Words")]
        public string KeyWords { get; set; }
        [Display(Name = "Short Url")]
        public string Url { get; set; }
        [Display(Name = "Dosage Instruction")]
        public string Instruction { get; set; }
        [Display(Name = "Warning & Side Effects")]
        public string UsageInstruction { get; set; }
        [Display(Name = "Material Code")]
        public string MaterialCode { get; set; }
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Display(Name = "Product Class")]
        public string ProductClass { get; set; }
        [Display(Name = "Product Images")]
        public string ProductImage { get; set; }
        [Display(Name = "Product Size")]
        public string ProductSize { get; set; }
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }
        [Display(Name = "Detailed Description")]
        public string Description { get; set; }
        [Display(Name = "Product Image1")]
        public string ProductImage1 { get; set; }
        [Display(Name = "Product Image2")]
        public string ProductImage2 { get; set; }
        [Display(Name = "Product Image3")]
        public string ProductImage3 { get; set; }
        [Display(Name = "Product Image4")]
        public string ProductImage4 { get; set; }
        [Display(Name = "Product Image5")]
        public string ProductImage5 { get; set; }
        [Required(ErrorMessage = "Please enter product MRP!")]
        [Display(Name = "MRP")]
        public decimal? Price { get; set; }
        [Display(Name = "Discount")]
        [Range(0, 99, ErrorMessage = "Please enter valid Discount!")]
        public int? DiscPercent { get; set; }
        [Display(Name = "Discounted Price")]
        public decimal? DiscPrice { get; set; }

        public short? DisplayOrder { get; set; }
        public bool? Hide { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DC. Status Status { get; set; }
        public string IPaddress { get; set; }
    }
    public class ItemModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public int Day { get; set; }
        public int Abbrevation { get; set; }
        public int Quantity { get; set; }
        public string QuantityInStock { get; set; }
        public int SubQuantity { get; set; }
        public string UploadPreId { get; set; }
        public string UserPrescriptionId { get; set; }
        public string DecodeHubId { get; set; }
    }
    public class PrescriptionMedcine
    {
        public byte? MedcineAvailable { get; set; }
        public string UploadPreId { get; set; }
        public List<ItemModel> Items { get; set; }
        public string DecodeHubId { get; set; }
    }
    public class TestModel
    {
        public string Id { get; set; }
        public string TestId { get; set; }
        public string Test { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }

    }
    public class PrescriptionTest
    {
        public string UploadPreId { get; set; }
        public List<TestModel> Items { get; set; }
        public string DecodeHubId { get; set; }
    }

}