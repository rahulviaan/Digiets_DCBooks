using Database.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DC.Web.App.Models;
using System.IO;
using ErrorLogger;
using Database.Repository.MasterRepository;

namespace DC.Web.App.Areas.School.Controllers
{

    [Authorize(Roles = "SuperAdmin,Admin")]
    [RouteArea("School")]
    public class DashboardController : WFbaseController
    {
        private ILog logerror = ErrorLogger.Logger.GetInstance;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public DashboardController() { }
        public DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            logerror.Logerror(" ReadExcelHeaderAndData Initialize", Server.MapPath("~/AppLog"));
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

        [Route("~/School")]
        [Route("~/manage/School")]
        public ActionResult Index(string _detail = "")
        {
            var vm = new ViewModel<IEnumerable<Models.KeyValue>, string, string>
            {
                Status = 201,
                DatatId = "",
                Message = "Request Initialize",
                Title = "",
                tData = null

            };
            try
            {
                var UserId = User.Identity.GetUserId();

                var roles = WFRoles.GetInstance.GetSchoolRoles();
                vm.tData = roles;
                vm.Status = 200;
                vm.Title = "Manage school ";
                vm.Message = "Successfull";
                Session["cartCount"] = new CartRepository().Count(x => x.CartStatusId == 1);

                return View(vm);
            }
            catch (Exception ex)
            {
                vm.Status = 400;
                vm.Title = "";
                vm.Message = "URL Tampering is not allowed. Please correct url and refresh page.";
                return View(vm);

            }
        }

        #region ManageSchool 
        public JsonResult Gets(string roleId, int maxRows, int page, int currentRow)
        {
            try
            {
                var repository = new SchoolRepository();
                var datas = repository.GetSchools(roleId, maxRows, page, currentRow).ToList();
                var ucName = "ucViewSchool";
                if (datas != null && datas.Count() > 0)
                {
                    var results = from c in datas
                                  select new
                                  {
                                      c.Id,
                                      c.Name,
                                      c.ContactNo,
                                      c.State,
                                      c.City,
                                      c.PinCode,
                                      c.Email,
                                      c.AspNetUserId,
                                      c.Status,
                                      c.UserName,
                                      c.SchoolCode,
                                      c.Strength,
                                      c.MasterBoard,
                                      c.ITIncharge,
                                      c.MasterBoardId,
                                      Password = Encryption.DecryptCommon(c.PasswordHash),
                                      ucName,
                                  };

                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "Success",
                        results,
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
        private SchoolModel GetData(string Id)
        {
            var model = new SchoolRepository().Get(Id);
            if (model != null)
            {
                var user = new AspNetUsersRepository().Get(m => m.Id == model.AspNetUserId);
                var schoolmodel = new SchoolModel
                {
                    Id = model.Id,
                    SchoolCode = model.SchoolCode,
                    AlterNateContactNo = model.AlterNateContactNo,
                    ContactNo = model.ContactNo,
                    EmailId = model.EmailId,
                    Status = (Status)model.Status,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    AddressLine3 = model.AddressLine3,
                    AspNetUserId = model.AspNetUserId,
                    Description = model.Description,
                    City = model.City,
                    Logo = model.Logo,
                    Pincode = model.Pincode,
                    Principle = model.Principle,
                    PrincipleContactNo = model.PrincipleContactNo,
                    State = model.State,
                    Title = model.Title,
                    vTitle = model.Title,
                    IPaddress = model.IPAddress,
                    CreateDate = model.CreateDate,
                    UpdatedDate = model.UpdatedDate,
                    CreatedBy = model.CreatedBy,
                    UpdatedBy = model.UpdatedBy,
                    UserName = Utility.ToSafeStringReplaced(user.UserName),
                    Password = DC.Encryption.DecryptCommon(user.PasswordHash),
                    Strength=model.Strength,
                    ITIncharge=model.ITIncharge,
                    MasterBoardId=model.MasterBoardId


                };
                return schoolmodel;
            }
            else
            {
                return null;
            }
        }

        public PartialViewResult AddSchhol(string Id, string Title)
        {
            try
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "Create New School" : Title;
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var model = GetData(Id);
                    model.UserName = Utility.ToSafeStringReplaced(model.UserName);

                    return PartialView("ucAddSchool", model);
                }
                else
                {

                    return PartialView("ucAddSchool", new SchoolModel());
                }
            }
            catch (Exception ex)
            {

                var error = new Web.App.Models.Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }
        }
        public PartialViewResult ViewSchool(string Id)
        {
            try
            {
                var model = GetData(Id);
                if (model != null)
                {
                    return PartialView("ucViewSchool", model);
                }
                else
                {
                    var error = new Web.App.Models.Error()
                    {
                        Message = "Data not exist."
                    };
                    return PartialView("ucError", error);
                }

            }
            catch (Exception ex)
            {
                var error = new Web.App.Models.Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }

        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddSchool(FormCollection fc, SchoolModel model)
        {
            try
            {
                string ext = "";
                model.Title = model.vTitle;
                string rootpath = Server.MapPath("~/Attatchments/");
                string filename = "";
                var thfilename = "";
                var repository = new SchoolRepository();
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
                var userrepository = new AspNetUsersRepository();
                if (!string.IsNullOrWhiteSpace(model.EmailId))
                {
                    var checkmail = userrepository.IsEmailExist(model.AspNetUserId, model.EmailId).Result;
                    if (checkmail)
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = model.EmailId + " The email you have entered is already in use. Please enter a different one!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrWhiteSpace(model.ContactNo))
                {
                    var checkphone = userrepository.IsPhoneNoExist(model.AspNetUserId, model.ContactNo).Result;
                    if (string.IsNullOrWhiteSpace(model.ContactNo) || checkphone)
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = model.ContactNo + " The contact number you have entered is already in use. Please enter a different one!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                foreach (var item in Request.Files)
                {

                    var FileCoverImage = Request.Files["Logo"];
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
                    model.Logo = filename + ext;
                    thfilename = rootpath + "\\" + "th_" + filename + ext;
                    filename = rootpath + "\\" + filename + ext;
                    FileCoverImage.SaveAs(filename);
                    Utility.Image_resize(filename, thfilename, 100);
                    break;
                }
                model.CreateDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                model.CreatedBy = User.Identity.GetUserId();
                model.UpdatedBy = User.Identity.GetUserId();
                model.IPaddress = Utility.GetIpAddress();
                model.Password = Encryption.Password(6);

                var result = repository.InsertUpdateSchool(model);
                Utility.Delete(result.image.Split(','), Server.MapPath("~/Attatchments/"));
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
        public PartialViewResult AssignBooks(string Id, string Title)
        {
            try
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "Assign books to school" : Title;
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var model = GetData(Id);
                    model.UserName = Utility.ToSafeStringReplaced(model.UserName);
                    return PartialView("ucAddSchool", model);
                }
                else
                {

                    return PartialView("ucAddSchool", new SchoolModel());
                }
            }
            catch (Exception ex)
            {

                var error = new Web.App.Models.Error()
                {
                    Message = ex.Message
                };
                return PartialView("ucError", error);
            }
        }
        public PartialViewResult CheckForDelete(string Id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    string controllerName = string.Format("{0}{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), "Controller");

                    var result = new SchoolRepository().CheckForDelete(Id, controllerName, "Delete");
                    if (result.Action)
                    {
                        result.Password = "";
                        return PartialView("ucDeleteSchool", result);
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
        public async Task<ActionResult> DeleteScool(string Id)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var result = new SchoolRepository().InsertUpdateSchool(new SchoolModel { Id = Id, Action = DC.Action.Delete });
                    Utility.Delete(result.image.Split(','), Server.MapPath("~/Attatchments/"));
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

        [HttpPost]
        public ActionResult ActiveInactive(string Id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var result = new SchoolRepository().ActiInactive(Id, User.Identity.GetUserId());
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