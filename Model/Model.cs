using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DC
{

    public class LibraryVM
    {
        //public string Description { get; set; }
        //public int PageNumber { get; set; }
        //public bool IsUserContent { get; set; }
        //public int ContentId { get; set; }
        //public string UserId { get; set; }
        //public string ClassId { get; set; }
        //public Int64 BookId { get; set; }
        //public string Image { get; set; }
        //public string Author { get; set; }
        //public string BookName { get; set; }
        //public string Chapter { get; set; }
        //public string ChapterContent { get; set; }
        //public bool BundlePackage { get; set; }
        //public bool IsInLibrary { get; set; }
        //public bool IsBundleUploaded { get; set; }
        //[Required(ErrorMessage = "Note is Required")]
        public int startindex { get; set; } = 1;
        public int endindex { get; set; } = 1;
        //public int AllowedCBPages { get; set; }
    }
    public class ResultModel
    {
        public int status { get; set; }
        public long id { get; set; }
        public string strid { get; set; }
        public string message { get; set; }
        public string image { get; set; }
    }
    public class Message<T>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
        public T Data { get; set; }
    }
    public class MessageC<T>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public T Data { get; set; }
    }
    public class DataMessage
    {

        public string Detail { get; set; }


    }
    public class AspNetUserModel
    {
        public string Role { get; set; }
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
        public int Gender { get; set; }
        public int Age { get; set; }
        public string MobNo { get; set; }
        public string EmailId { get; set; }
        public string eUserName { get; set; }
        public string ePassword { get; set; }
        public bool MobValidate { get; set; }
        public bool EmailValidate { get; set; }
        public int LoginMode { get; set; }
        public bool LoginThirdParty { get; set; }
        public DateTime? LastLogin { get; set; }
        public int Religion { get; set; }
        public string Image { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }
        public int Status { get; set; }
        public string TimeZone { get; set; }
        public string AccessLevels { get; set; }
        public int Hide { get; set; }
        public int DisplayOrder { get; set; }

        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }


        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }



    }
    public class Response<T>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
        public T Data { get; set; }

    }
    public class CategoryResultObject
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string SlugUrl { get; set; }
        public short? ParentId { get; set; }
        public int? DisplayOrder { get; set; }
        public int? ChildCatCount { get; set; }
        public int? ProductCount { get; set; }
        public IEnumerable<CategoryResultObject> categories { get; set; }
        public bool IsChecked { get; set; }
    }
    public class KeyValue
    {
        public string Id { get; set; }
        public string group { get; set; }
        public string Title { get; set; }
        public string PinCode { get; set; }
        public bool IsSelected { get; set; }
        public long? DisplayOrder { get; set; }
        public decimal? TaxPercent { get; set; }
        public DateTime? dtmCreate { get; set; }
        public int ChildCount { get; set; } = 0;
        public string Detail { get; set; }
        public string ucName { get; set; }
    }
    public class sKeyValue
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string SlugUrl { get; set; }
    }
    public class CustomMessage<T>
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public int iId { get; set; }
        public string[] Ids { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
        public bool Action { get; set; }
        public bool PasswordRequired { get; set; }
        public string Password { get; set; }
        public T Data { get; set; }
        IEnumerable<T> Datas { get; set; }
    }

    public class BookModel
    {

        public string Id { get; set; }
        [Required(ErrorMessage = "Please select the book category ")]
        [Display(Name = "Book Category")]
        public long? MasterCategoryId { get; set; }
        [Required(ErrorMessage = "Please enter the book name ")]
        [Display(Name = "Book Name")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please select the book class ")]
        [Display(Name = "Book Class")]
        public long? MasterClassId { get; set; }

        [Required(ErrorMessage = "Please select the board")]
        [Display(Name = "Board")]
        public long? MasterBoardId { get; set; }

        [Required(ErrorMessage = "Please select the subject")]
        [Display(Name = "Subject")]
        public long? MasterSubjectId { get; set; }

        [Required(ErrorMessage = "Please select the series")]
        [Display(Name = "series")]
        public long? MasterSeriesId { get; set; }
        public string Subject { get; set; }
        public string Series { get; set; }
        public string Board { get; set; }
        public string Class { get; set; }

        [Required(ErrorMessage = "Please enter the book name ")]
        [Display(Name = "Book Name")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string vTitle { get; set; }

        [Required(ErrorMessage = "Please enter the author name ")]
        [Display(Name = "Author Name")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Please enter the ISBN ")]
        [Display(Name = "ISBN")]
        [MaxLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string ISBN { get; set; }
        [Display(Name = "ISBN")]
        [MaxLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Edition { get; set; }
        [Display(Name = "Cover Image")]
        [Required(ErrorMessage = "[*Required]")]
        public string FileCoverImage { get; set; }

        [Display(Name = "Banner Image")]
        public string FileBannerImage { get; set; }

        [Display(Name = "Description")]
        [MaxLength(1000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Description { get; set; }
        [Display(Name = "Page Title")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string PageTitle { get; set; }
        [Display(Name = "Meta Description")]
        [MaxLength(1000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string MetaDescription { get; set; }
        [Display(Name = "Og Title")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string OgTitle { get; set; }
        [Display(Name = "Og Description")]
        [MaxLength(1000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string OgDescription { get; set; }
        [Display(Name = "Twitter Tiile")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string TwitterTitle { get; set; }
        [Display(Name = "Twitter Description")]
        [MaxLength(1000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string TwitterDescription { get; set; }
        [Display(Name = "Key Words")]
        [MaxLength(1000, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string KeyWords { get; set; }
        [Display(Name = "Parent Category")]
        public long? ParentId { get; set; }
        public string ShortCode { get; set; }
        public short? DisplayOrder { get; set; }
        [Display(Name = "Banner Image")]
        public string BannerImage { get; set; }
        public string ServerId { get; set; }
        [Display(Name = "Encription Key")]
        public string EncriptionKey { get; set; }
        [Display(Name = "Size")]
        public string isSize { get; set; }
        [Display(Name = "Ebook Price")]
        [Required(ErrorMessage = "Please enter the ebook price ")]
        public decimal? EbookPrice { get; set; }
        [Display(Name = "Print Price")]
        [Required(ErrorMessage = "Please enter the print price ")]
        public decimal? PrintPrice { get; set; }
        [Display(Name = "Discount")]
        public decimal? Discount { get; set; }
        [Display(Name = "Colour")]
        public int? Colour { get; set; }
        [Display(Name = "Ebook Size(MB)")]
        public long? EbookSize_MB_ { get; set; }

        [Display(Name = "Ebook")]
        public int? Ebook { get; set; }
        [Display(Name = "Pbook")]
        public int? Pbook { get; set; }
        [Display(Name = "Audio")]
        public int? Audio { get; set; }
        [Display(Name = "Book Type")]
        public BookType EBookType { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }
        public string IPaddress { get; set; }
        public Action Action { get; set; }
    }
    public class SchoolModel
    {
        public string Id { get; set; }
        public string AspNetUserId { get; set; }
        [Required(ErrorMessage = "Please enter the school name ")]
        [Display(Name = "School Name")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter the school name ")]
        [Display(Name = "School Name")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string vTitle { get; set; }

        [Display(Name = "School Code")]
        public string SchoolCode { get; set; }
        [Required(ErrorMessage = "Please enter the email")]
        [Display(Name = "Email")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string EmailId { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }
        [Required(ErrorMessage = "Please enter the contact no")]
        [Display(Name = "Mobile No / Land Line No")]
        [MaxLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string ContactNo { get; set; }
        [Display(Name = "Alternaete Mobile No / Land Line No")]
        [MaxLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string AlterNateContactNo { get; set; }
        [Display(Name = "Principle")]
        [MaxLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Principle { get; set; }
        [Display(Name = "Principle Contact No")]
        [MaxLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string PrincipleContactNo { get; set; }
        [Display(Name = "AddressLine#1")]
        [Required(ErrorMessage = "Please enter addressline#1")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string AddressLine1 { get; set; }
        [Display(Name = "AddressLine#2")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string AddressLine2 { get; set; }
        [Display(Name = "AddressLine#3")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string AddressLine3 { get; set; }
        [Display(Name = "State")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string State { get; set; }
        [Display(Name = "City")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]

        public string City { get; set; }

        [Display(Name = "Board")]
        [Required(ErrorMessage = "Please select board ")]

        public long? MasterBoardId { get; set; }
        [Display(Name = "Strength")]
        public long? Strength { get; set; }
        [Display(Name = "IT Incharge")]
        public string ITIncharge { get; set; }

        [Display(Name = "Pincode")]
        [MaxLength(6, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Pincode { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public Status Status { get; set; }
        public string IPaddress { get; set; }
        public Action Action { get; set; }

        [Required]
        [Display(Name = "User name")]
        [MaxLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MaxLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Password { get; set; }
    }
    public class MyLibraryModel
    {
        public string Id { get; set; }
        public string MasterBookId { get; set; }
        public string AspNetUserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int? Validity { get; set; }
        public DateTime? LastDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }
        public Action Action { get; set; }
        public string IPaddress { get; set; }
    }

    public class UserBookModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AdmissionNo { get; set; }
        public string AccountCode { get; set; }

        [Display(Name = "Class")]
        public long? MasterClassId { get; set; }
        [Display(Name = "Subject")]
        public long? MasterSubjectId { get; set; }
        public string AspNetUserId { get; set; }
        public string MasterBookId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }
        public Action Action { get; set; }
        public string IPaddress { get; set; }
    }
    public class CategoryModel
    {

        public long Id { get; set; }
        [Required(ErrorMessage = "Please enter the category name.")]
        [Display(Name = "Category Name")]
        public string Title { get; set; }
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Please enter the category name.")]
        public string vTitle { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Parent Category")]
        public long? ParentId { get; set; }
        [Display(Name = "Short Code")]
        public string ShortCode { get; set; }
        [Display(Name = "Display Order")]
        public short? DisplayOrder { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "Banner Image")]
        public string BannerImage { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }
        public string IPaddress { get; set; }
        public string ParentCategory { get; set; }

        public string OldImage { get; set; }
        [Display(Name = "Page Title")]
        public string PageTitle { get; set; }
        [Display(Name = "Meta Description")]
        public string MetaDescription { get; set; }
        [Display(Name = "Og Title")]
        public string OgTitle { get; set; }
        [Display(Name = "Og Description")]
        public string OgDescription { get; set; }
        [Display(Name = "Twitter Title")]
        public string TwitterTitle { get; set; }
        [Display(Name = "Twitter Description")]
        public string TwitterDescription { get; set; }
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "KeyWords")]
        public string KeyWords { get; set; }
    }
    public class BoardModel
    {
        public long Id { get; set; }
        public string GlobalId { get; set; }
        [Required(ErrorMessage = "Please enter the board name.")]
        [Display(Name = "Board Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Please select publisher.")]
        //[Display(Name = "Publisher")]
        public int? MasterPublisherId { get; set; }
        [Required(ErrorMessage = "Please enter the board name.")]
        [Display(Name = "Board Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title1 { get; set; }
        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string IPAddress { get; set; }
        public string AspNetUserId { get; set; }
        public Action ActionTaken { get; set; }

        public Status Status { get; set; }
    }
    public class SubjectModel
    {
        public long Id { get; set; }
        public string GlobalId { get; set; }
        [Required(ErrorMessage = "Please enter the subject name.")]
        [Display(Name = "Subject Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title { get; set; }

        public int? MasterPublisherId { get; set; }
        [Required(ErrorMessage = "Please enter the subject name.")]
        [Display(Name = "Subject Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title1 { get; set; }
        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string IPAddress { get; set; }
        public string AspNetUserId { get; set; }
        public Action ActionTaken { get; set; }

        public Status Status { get; set; }
    }
    public class SeriesModel
    {
        public long Id { get; set; }
        public string GlobalId { get; set; }
        [Required(ErrorMessage = "Please enter the series name.")]
        [Display(Name = "Series Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Please select publisher.")]
        //[Display(Name = "Publisher")]
        public int? MasterPublisherId { get; set; }
        [Required(ErrorMessage = "Please select subject.")]
        [Display(Name = "Subject")]
        public int? MasterSubjectId { get; set; }
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter the series name.")]
        [Display(Name = "Series Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title1 { get; set; }
        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string IPAddress { get; set; }
        public string AspNetUserId { get; set; }
        public Action ActionTaken { get; set; }

        public Status Status { get; set; }
    }

    public class ClassModel
    {
        public long Id { get; set; }
        public string GlobalId { get; set; }
        [Required(ErrorMessage = "Please enter the class name.")]
        [Display(Name = "Class Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Please select publisher.")]
        //[Display(Name = "Publisher")]
        public int? MasterPublisherId { get; set; }

        [Required(ErrorMessage = "Please enter the class name.")]
        [Display(Name = "Class Name")]
        [MaxLength(300, ErrorMessage = "{0} cannot be longer than {1} characters.")]
        public string Title1 { get; set; }
        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string IPAddress { get; set; }
        public string AspNetUserId { get; set; }
        public Action ActionTaken { get; set; }
        public Status Status { get; set; }
    }
}
