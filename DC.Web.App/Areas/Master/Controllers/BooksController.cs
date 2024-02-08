using Database;
using Database.Repository.MasterRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DC.Web.App.Models;

namespace DC.Web.App.Areas.Master.Controllers
{

    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BooksController : WFbaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public BooksController() { }
        public BooksController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {

                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Super/Admin
        public ActionResult Index(long id = 0, string _detail = "")
        {
            var parentcategory = new DC.KeyValue
            {
                Id = id.ToString(),
                Detail = _detail,
            };
            return View(parentcategory);
        }
        public ActionResult Manage(short catid = 0, string id = "", string _detail = "")
        {
            var message = new CustomMessage<List<DC.sKeyValue>>
            {
                Data = null,
                Detail = _detail,
                iId = catid,
                Id = id,
                Message = "Request Initialize",
                Status = 201
            };
            try
            {
                if (catid != 0)
                {
                    var objcategory = new CategoryRepository();
                    var categories = objcategory.Gets(catid).ToList();
                    var bc = objcategory.CategoryParent(catid).ToList();
                    var breadcrumb = from c in bc
                                     select new
                                     {
                                         c.Id,
                                         Title = Utility.ToSafeString(c.Title),
                                     };

                    var bclist = new List<sKeyValue>();
                    var title = "";
                    foreach (var item in breadcrumb)
                    {
                        title += item.Title + "-";
                        bclist.Add(new sKeyValue { Id = item.Id, Title = item.Title, SlugUrl = DC.SlugUrl.GenerateSlug(item.Id.ToString(), title) });
                    }


                    message.Data = bclist;
                    message.Detail = _detail;
                    message.iId = catid;
                    message.Id = id;
                    message.Message = "Successfully";
                    message.Status = 200;
                }
                else
                {
                    message.Data = null;
                    message.Detail = _detail;
                    message.iId = catid;
                    message.Id = id;
                    message.Message = "Please provide correct data.";
                    message.Status = 202;
                }
            }
            catch (Exception ex)
            {
                message.Message = ex.Message;
                message.Status = 400;
            }
            return View("~/Areas/Master/Views/Books/Manage.cshtml", message);
        }
        #region ManageCategories
        public JsonResult Gets(short ParentId)
        {
            try
            {
                var objcategory = new CategoryRepository();
                var categories = objcategory.Gets(ParentId);
                var bc = objcategory.CategoryParent(ParentId);
                var breadcrumb = from c in bc
                                 select new
                                 {
                                     c.Id,
                                     Title = Utility.ToSafeString(c.Title),
                                 };
                var title = "";
                foreach (var item in breadcrumb)
                {
                    title = item.Title + "-";
                }
                var v12 = categories.ToList();
                if (categories != null && categories.Count() > 0)
                {
                    var results = from c in categories
                                  orderby c.CreateDate descending
                                  select new
                                  {
                                      c.Id,
                                      c.ParentId,
                                      Title = Utility.ToSafeString(c.Title),
                                      Description = Utility.ToSafeString(c.Description),
                                      DisplayOrder = Utility.ToSafeString(c.DisplayOrder),
                                      dtmCreate = Utility.ToDateTime((DateTime)c.CreateDate),
                                      ShortCode = Utility.ToSafeString(c.ShortCode),
                                      SlugUrl = DC.SlugUrl.GenerateSlug(c.Id.ToString(), title + "-" + c.Title),
                                      Image = Utility.ToSafeString(c.Image),
                                      c.Status,
                                  };

                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "Success",
                        results,
                        bc = breadcrumb,
                    }, JsonRequestBehavior.AllowGet);
                }
                else if (categories.Count() == 0)
                {
                    return Json(new
                    {
                        StatusCode = 202,
                        Message = "Data not found.",
                        bc = breadcrumb,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 201,
                        bc = breadcrumb,
                        Message = "No child category exist. ",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var st = (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    Message = st,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Count(short Id)
        {
            try
            {
                var objcategory = new CategoryRepository();
                var CatCount = objcategory.Count(Id);
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    CatCount = CatCount
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var st = (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    CatCount = 0,
                    Message = st,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult BookCount(short Id)
        {
            try
            {
                var repository = new BookRepository();
                var bookcount = repository.BookCount(Id);
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Success",
                    ProductCount = bookcount
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var st = (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    CatCount = 0,
                    ProductCount = 0,
                    Message = st,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult Add(short Id, short ParentId, string Title)
        {
            try
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "Create New Category" : "Create Category -> " + Title;
                if (Id > 0)
                {
                    var model = new CategoryRepository().Get(Id);
                    if (model != null)
                    {
                        var category = new CategoryModel
                        {
                            Id = model.Id,
                            CreatedBy = model.CreatedBy,
                            UpdateDate = DateTime.Now,

                            DisplayOrder = model.DisplayOrder,
                            Image = model.Image,
                            BannerImage = model.BannerImage,
                            OldImage = model.Image,
                            ParentId = model.ParentId,

                            IPaddress = model.IPaddress,
                            ShortCode = model.ShortCode,
                            Status = (Status)model.Status,
                            Title = model.Title,
                            vTitle = model.Title,
                            UpdatedBy = model.UpdatedBy,
                            Description = model.Description,
                            ParentCategory = Title,
                            Author = model.Author,
                            KeyWords = model.KeyWords,
                            MetaDescription = model.MetaDescription,
                            OgDescription = model.OgDescription,
                            OgTitle = model.OgTitle,
                            PageTitle = model.PageTitle,
                            TwitterDescription = model.TwitterDescription,
                            TwitterTitle = model.TwitterTitle,
                            CreateDate = DateTime.Now,

                        };
                        return PartialView("ucAdd", category);
                    }
                    else
                    {
                        var error = new Error()
                        {
                            Message = "Please try again some problem occur"
                        };
                        return PartialView("ucError", error);
                    }

                }

                else
                {


                    return PartialView("ucAdd", new CategoryModel() { ParentId = ParentId, ParentCategory = Title, });
                }
            }
            catch (Exception ex)
            {

                var error = new Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(FormCollection fc, MasterCategory model)
        {
            try
            {
                string ext = "";
                string rootpath = Server.MapPath("~/Attatchments/Category/");
                string filename = "";
                string tempfilename = "";
                var size = 0;
                foreach (var item in Request.Files)
                {
                    var file = Request.Files[item.ToString()];
                    ext = Path.GetExtension(file.FileName);
                    if (!Utility.supportedTypes(ext))
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = "Please upload valid image type." +
                            "",
                            File = ""
                        }, JsonRequestBehavior.AllowGet);
                    }
                    size += file.ContentLength;
                }
                if (size > 10485760)
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Oops! You can only upload .jpeg, .jpg, .png or .gif files that are less than 10MB in size.",
                        File = ""
                    }, JsonRequestBehavior.AllowGet);
                }
                var i = 1;
                foreach (var item in Request.Files)
                {
                    //var Height = Utility.ToSafeInt(fc["Height" + (i - 1)]);
                    //var Width = Utility.ToSafeInt(fc["Width" + (i - 1)]);
                    var file = Request.Files[item.ToString()];
                    filename = Utility.getFileName();
                    ext = Path.GetExtension(file.FileName);
                    var newfilename = filename + ext;
                    tempfilename = rootpath + newfilename;
                    filename = rootpath + "th_" + newfilename;
                    if (!string.IsNullOrWhiteSpace(model.OldImage))
                    {
                        Utility.Delete(rootpath + model.OldImage);
                        Utility.Delete(rootpath + "th_" + model.OldImage);
                    }
                    file.SaveAs(tempfilename);
                    var resultresize = Utility.Image_resize(tempfilename, filename, 100);
                    if (item.ToString() == "fl0")
                    {
                        model.Image = newfilename;
                    }
                    else if (item.ToString() == "flBanner0")
                    {

                        model.BannerImage = newfilename;
                    }
                }
                if (string.IsNullOrWhiteSpace(model.Image) && !string.IsNullOrWhiteSpace(model.OldImage))
                {
                    model.Image = "";
                    Utility.Delete(rootpath + model.OldImage);
                    Utility.Delete(rootpath + "th_" + model.OldImage);
                }
                var categoryrepository = new CategoryRepository();
                if (model != null && ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        model.CreateDate = DateTime.Now;
                        model.UpdateDate = DateTime.Now;
                        model.Status = (byte)Status.Active.GetHashCode();
                        model.ShortCode = Utility.GetShortCode(model.Title);
                        model.CreatedBy = User.Identity.GetUserId();
                        model.UpdatedBy = User.Identity.GetUserId();
                        model.IPaddress = Utility.GetIpAddress();
                        if (categoryrepository.IsExist(model.Id, model.Title, model.ParentId))
                        {
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = model.Title + " already exist. Please try another category name."
                            }, JsonRequestBehavior.AllowGet);
                        }
                        var result = categoryrepository.CreateAsync(model);
                        if (result != null)
                        {
                            return Json(new
                            {
                                StatusCode = 200,
                                Message = model.Title + " |  inserted successfully."
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = "Some error occur  inserting : [ " + model.Title + " ] please try later."
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (model.Id > 0)
                    {

                        model.UpdateDate = DateTime.Now;
                        model.ShortCode = Utility.GetShortCode(model.Title);
                        model.UpdatedBy = User.Identity.GetUserId();
                        model.IPaddress = Utility.GetIpAddress();
                        if (categoryrepository.IsExist(model.Id, model.Title, (short)model.ParentId))
                        {
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = model.Title + " already exist. Please try another name."
                            }, JsonRequestBehavior.AllowGet);
                        }
                        var result = categoryrepository.UpdateAsync(model);
                        if (result != null)
                        {
                            return Json(new
                            {
                                StatusCode = 200,
                                Message = model.Title + " |  updated successfully."
                            }, JsonRequestBehavior.AllowGet);
                        }

                        else
                        {
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = "Some error occur  updating : [ " + model.Title + " ] please try later."
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new
                        {

                            StatusCode = 400,
                            Message = "Please provide correct data to be inserted."
                        }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new
                    {

                        StatusCode = 400,
                        Message = "Please provide correct data to be inserted."
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new
                {

                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult IsExist(short Id, string CatName, short ParentId)
        {
            var isExist = new CategoryRepository().IsExist(Id, CatName, ParentId);
            if (!isExist)
            {
                return Json(new
                {
                    StatusCode = 200,
                    Message = "Success."
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    StatusCode = 400,
                    Message = CatName + " | already exist please try another name."
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult View(short Id)
        {
            var model = new CategoryRepository().Get(Id);
            if (model != null)
            {
                var category = new CategoryModel
                {
                    Id = model.Id,
                    CreatedBy = model.CreatedBy,
                    UpdateDate = DateTime.Now,
                    DisplayOrder = model.DisplayOrder,
                    Image = model.Image,
                    ParentId = model.ParentId,
                    CreateDate = model.CreateDate,
                    IPaddress = model.IPaddress,
                    ShortCode = model.ShortCode,
                    Status = (Status)model.Status,
                    Title = model.Title,
                    UpdatedBy = model.UpdatedBy,
                    Description = model.Description,
                };
                return PartialView("ucViewCategory", category);
            }
            else
            {
                return PartialView("ucViewCategory", new CategoryModel());
            }

        }
        [HttpPost]
        public ActionResult ActiveInactive(short Id)
        {
            try
            {
                if (Id > 0)
                {
                    var result = new CategoryRepository().ActiInactive(Id);
                    if (result != null)
                    {
                        return Json(new
                        {
                            StatusCode = 200,
                            Message = "Operation done successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = "Some error occur try later."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Please provide Id."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public PartialViewResult CheckForDelete(short Id)
        {
            try
            {
                if (Id > 0)
                {
                    string controllerName = string.Format("{0}{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), "Controller");

                    var result = new CategoryRepository().CheckForDelete(Id, controllerName, "Delete");
                    if (result.Action)
                    {
                        result.Password = string.IsNullOrWhiteSpace(result.Password) ? "" : DC.Encryption.DecryptCommon(result.Password);
                        return PartialView("ucDelete", result);
                    }
                    else
                    {
                        var error = new Error()
                        {
                            Message = result.Message
                        };
                        return PartialView("ucError", error);
                    }
                }
                else
                {
                    var error = new Error()
                    {
                        Message = "Please provide data to be deleted."
                    };
                    return PartialView("ucError", error);

                }
            }
            catch (Exception ex)
            {
                var error = new Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }
        }
        [HttpPost]
        public ActionResult Delete(short Id, string Password, string filename)
        {
            try
            {

                if (Id > 0)
                {
                    Password = string.IsNullOrWhiteSpace(Password) ? "" : DC.Encryption.EncryptCommon(Password);
                    string controllerName = string.Format("{0}{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), "Controller");
                    var result = new CategoryRepository().RemoveAsync(Id, controllerName, "Delete", Password);
                    if (result.Action)
                    {
                        try
                        {
                            if (result.Data != null && !string.IsNullOrWhiteSpace(result.Data.Image))
                            {
                                string rootpath = Server.MapPath("~/Attatchments/Category/");
                                var fn = rootpath + result.Data.Image;
                                var thumbnaile = rootpath + "th_" + result.Data.Image;
                                if (System.IO.File.Exists(fn))
                                {
                                    System.IO.File.Delete(fn);
                                    System.IO.File.Delete(thumbnaile);
                                }
                                fn = rootpath + result.Data.BannerImage;
                                thumbnaile = rootpath + "th_" + result.Data.BannerImage;
                                if (System.IO.File.Exists(fn))
                                {
                                    System.IO.File.Delete(fn);
                                    System.IO.File.Delete(thumbnaile);
                                }

                            }
                        }
                        catch (Exception ex1)
                        {
                            var v2 = ex1.Message;
                        }

                        return Json(new
                        {
                            StatusCode = 200,
                            Message = "Category deleted successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = result.Message
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Please provide Id."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult DeleteImage(short Id, string filename)
        {
            try
            {
                if (Id > 0)
                {
                    var result = new CategoryRepository().DeleteImage(Id, Utility.IPAddress);
                    if (result != null)
                    {
                        string rootpath = Server.MapPath("~/Attatchments/Category/");
                        var fn = rootpath + filename;
                        var thumbnaile = rootpath + "th_" + filename;
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(filename))
                            {
                                if (System.IO.File.Exists(fn))
                                {
                                    System.IO.File.Delete(fn);
                                    System.IO.File.Delete(thumbnaile);
                                }
                            }
                        }
                        catch (Exception ec)
                        {
                            var v2 = ec.Message;
                        }
                        return Json(new
                        {
                            StatusCode = 200,
                            Message = "Category Iamge deleted successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            StatusCode = 401,
                            Message = "Server error exist while deleting category image."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Please provide Id."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteBannerImage(short Id, string filename)
        {
            try
            {
                if (Id > 0)
                {
                    var result = new CategoryRepository().DeleteBannerImage(Id, Utility.IPAddress);
                    if (result != null)
                    {
                        string rootpath = Server.MapPath("~/Attatchments/Category/");
                        var fn = rootpath + filename;
                        var thumbnaile = rootpath + "th_" + filename;
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(filename))
                            {
                                if (System.IO.File.Exists(fn))
                                {
                                    System.IO.File.Delete(fn);
                                    System.IO.File.Delete(thumbnaile);
                                }
                            }
                        }
                        catch (Exception ec)
                        {
                            var v2 = ec.Message;
                        }
                        return Json(new
                        {
                            StatusCode = 200,
                            Message = "Category Banner Iamge deleted successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            StatusCode = 401,
                            Message = "Server error exist while deleting category banner image."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Please provide Id."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region manage books
        public JsonResult GetBooks(long? MasterCategoryId)
        {
            try
            {
                var repository = new BookRepository();
                var datas = repository.Gets(m => m.MasterCategoryId == MasterCategoryId).ToList();
                if (datas != null && datas.Count() > 0)
                {
                    var results = from c in datas
                                  orderby c.CreateDate descending
                                  select new
                                  {
                                      Id = c.Id,
                                      Title = Utility.ToSafeString(c.Title),
                                      Author = Utility.ToSafeString(c.Author),
                                      EbookPrice = Utility.ToSafeDouble(c.EbookPrice).ToString("N2"),
                                      PbookPrice = Utility.ToSafeDouble(c.PrintPrice).ToString("N2"),
                                      ISBN = Utility.ToSafeString(c.ISBN),
                                      dtmCreate = Utility.ToDateTime((DateTime)c.CreateDate),
                                      ShortCode = Utility.ToSafeString(c.ShortCode),
                                      Image = Utility.ToSafeString(c.Image),
                                      c.Status
                                  };

                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "Success",
                        results = results,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 201,
                        Message = "No data exist. ",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var st = (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    Message = st,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        private BookModel GetData(string Id)
        {
            var model = new BookRepository().Get(Id);
            if (model != null)
            {
                return new BookModel
                {
                    Id = Id,
                    MasterCategoryId = model.MasterCategoryId,
                    
                    MasterClassId = model.MasterClassId,
                    MasterBoardId = model.MasterBoardId,
                    MasterSubjectId = model.MasterSubjectid,
                    MasterSeriesId = model.MasterSeriesid,

                    Title = model.Title,
                    vTitle = model.Title,
                    Author = model.Author,
                    ISBN = model.ISBN,
                    Edition = model.Edition,
                    FileCoverImage = model.Image,
                    FileBannerImage = model.BannerImage,
                    Description = model.Description,
                    PageTitle = model.PageTitle,
                    MetaDescription = model.MetaDescription,

                    OgTitle = model.OgTitle,
                    OgDescription = model.OgDescription,
                    TwitterTitle = model.TwitterTitle,
                    TwitterDescription = model.TwitterDescription,
                    KeyWords = model.KeyWords,
                    ParentId = model.ParentId,
                    ShortCode = model.ShortCode,
                    DisplayOrder = model.DisplayOrder,
                    BannerImage = model.BannerImage,
                    ServerId = model.ServerId,
                    EncriptionKey = model.EncriptionKey,
                    isSize = model.isSize,
                    EbookPrice = model.EbookPrice,
                    PrintPrice = model.PrintPrice,
                    Discount = model.Discount,
                    Colour = model.Colour,
                    EbookSize_MB_ = model.EbookSize_MB_,
                    Ebook = model.Ebook,
                    Pbook = model.Pbook,
                    Audio = model.Audio,
                    EBookType = (BookType)Utility.ToSafeInt(model.Status),
                    CreatedBy = model.CreatedBy,
                    UpdatedBy = model.UpdatedBy,
                    CreateDate = model.CreateDate,
                    UpdateDate = model.UpdateDate,
                    Status = (Status)Utility.ToSafeInt(model.Status),
                    IPaddress = model.IPaddress
                };
            }
            else
            {
                return null;
            }
        }
        public PartialViewResult AddBook(string Id, string Title, long? MasterCategoryId)
        {
            try
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "Create New Book" : Title;
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var model = GetData(Id);
                    if (model != null)
                    {
                        return PartialView("ucAddBook", model);
                    }
                    else
                    {
                        var error = new Error()
                        {
                            Message = "Please try again some problem occur"
                        };
                        return PartialView("ucError", error);
                    }
                }
                else
                {
                    return PartialView("ucAddBook", new BookModel { MasterCategoryId = MasterCategoryId });
                }
            }
            catch (Exception ex)
            {

                var error = new Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddBook(FormCollection fc, BookModel model)
        {
            try
            {
                string ext = "";
                model.Title = model.vTitle;
                string rootpath = Server.MapPath("~/Attatchments/Book/");
                string filename = "";
                string tempfilename = "";
                var thfilename = "";
                var repository = new BookRepository();
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.Action = DC.Action.Insert;
                }
                else
                {
                    model.Action = DC.Action.Update;
                }
                if (repository.IsExist(model.Id, model.Title))
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = model.Title + " already exist. Please try another book title."
                    }, JsonRequestBehavior.AllowGet);
                }

                foreach (var item in Request.Files)
                {
                    switch (item.ToString())
                    {
                        case "FileCoverImage":
                            var FileCoverImage = Request.Files["FileCoverImage"];
                            filename = Utility.getFileName();
                            ext = Path.GetExtension(FileCoverImage.FileName);
                            if (!Utility.supportedTypes(ext))
                            {
                                return Json(new
                                {
                                    StatusCode = 400,
                                    Message = "Please upload valid drug licence",
                                    File = ""
                                }, JsonRequestBehavior.AllowGet);
                            }
                            model.FileCoverImage = filename + ext;
                            thfilename = rootpath + "\\" + "th_" + filename + ext;
                            filename = rootpath + "\\" + filename + ext;
                            FileCoverImage.SaveAs(filename);
                            Utility.Image_resize(filename, thfilename, 100);
                            break;
                        case "FileBannerImage":
                            var FileBannerImage = Request.Files["FileBannerImage"];
                            filename = Utility.getFileName();
                            ext = Path.GetExtension(FileBannerImage.FileName);
                            if (!Utility.supportedTypes(ext))
                            {
                                return Json(new
                                {
                                    StatusCode = 400,
                                    Message = "Please upload valid  valid gstin",
                                    File = ""
                                }, JsonRequestBehavior.AllowGet);
                            }
                            model.FileBannerImage = filename + ext;
                            thfilename = rootpath + "\\" + "th_" + filename + ext;
                            filename = rootpath + "\\" + filename + ext;
                            FileBannerImage.SaveAs(filename);
                            Utility.Image_resize(filename, thfilename, 100);
                            break;
                    }
                } 
                model.CreateDate = DateTime.Now;
                model.UpdateDate = DateTime.Now;
                model.CreatedBy = User.Identity.GetUserId();
                model.UpdatedBy = User.Identity.GetUserId();
                model.IPaddress = Utility.GetIpAddress();

                var result = repository.InsertUpdateDelete(model);
                Utility.Delete(result.image.Split(','), Server.MapPath("~/Attatchments/Book/"));
                return Json(new
                {
                    StatusCode = result.status,
                    Message = result.message,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {

                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<PartialViewResult> ViewBook(string Id)
        {
            var model = GetData(Id);
            if (model != null)
            {
                return PartialView("ucViewBook", model);
            }
            else
            {
                var error = new Error()
                {
                    Message = "Data not exist."
                };
                return PartialView("ucError", error);
            }

        }
        [HttpPost]
        public ActionResult ActiveInactiveBook(string Id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var result = new BookRepository().ActiInactive(Id, User.Identity.GetUserId());
                    if (result != null)
                    {
                        return Json(new
                        {
                            StatusCode = 200,
                            Message = "Operation done successfully."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = "Some error occur try later."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Please provide Id."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public PartialViewResult CheckForDeleteBook(string Id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    string controllerName = string.Format("{0}{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), "Controller");

                    var result = new BookRepository().CheckForDelete(Id, controllerName, "Delete");
                    if (result.Action)
                    {
                        result.Password = "";
                        return PartialView("ucDeleteBook", result);
                    }
                    else
                    {
                        var error = new Error()
                        {
                            Message = result.Message
                        };
                        return PartialView("ucError", error);
                    }
                }
                else
                {
                    var error = new Error()
                    {
                        Message = "Please provide data to be deleted."
                    };
                    return PartialView("ucError", error);

                }
            }
            catch (Exception ex)
            {
                var error = new Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteBook(string Id)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var result = new BookRepository().InsertUpdateDelete(new BookModel { Id = Id, Action = DC.Action.Delete });
                    Utility.Delete(result.image.Split(','), Server.MapPath("~/Attatchments/Book/"));
                    return Json(new
                    {
                        StatusCode = result.status,
                        Message = result.message
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Please provide Id."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult RemoveImageFile(string Id, string Abbr)
        {
            try
            {  
                
                var result = new BookRepository().RemoveImageFile(Id, Abbr, User.Identity.GetUserId());
                if (!string.IsNullOrWhiteSpace(result))
                {
                    DeleteFile(result);
                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "Image deleted successfully. ",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 201,
                        Message = "Some error occur while deleting image. Please try again later",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var st = (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) ? ex.InnerException.Message : ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    Message = st,
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [NonAction]
        private void DeleteFile(string file)
        {
            try
            {
                string img = Server.MapPath($"~/Attatchments/Book/{file}");
                string imgthumb = Server.MapPath($"~/Attatchments/Book/th_{file}");
                if (System.IO.File.Exists(img))
                {
                    System.IO.File.Delete(img);

                }
                if (System.IO.File.Exists(imgthumb))
                {

                    System.IO.File.Delete(imgthumb);
                }
            }
            catch (Exception e)
            {


            }
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}