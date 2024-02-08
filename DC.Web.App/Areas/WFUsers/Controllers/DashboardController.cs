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

namespace DC.Web.App.Areas.WFUsers.Controllers
{

    [Authorize(Roles = "SuperAdmin,Admin")]
    [RouteArea("WFUsers")]
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
        [Route("~/musers")]
        [Route("~/users")]
        [Route("~/manage/users")]
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
                var uroles = UserManager.GetRoles(UserId);
                var roles = WFRoles.GetInstance.GetRoles();
                vm.tData = roles;
                vm.Status = 200;
                vm.Title = "Manage Users ";
                vm.Message = "Successfull";
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

        #region ManageUsers   
        public JsonResult Gets(string roleId, int maxRows, int page, int currentRow)
        {
            try
            {
                var repository = new AspNetUsersRepository();
                var datas = repository.GetUsers(roleId, maxRows, page, currentRow).ToList();
                var ucName = "ucViewUser";
                if (datas != null && datas.Count() > 0)
                {
                    var results = from c in datas
                                  select new
                                  {
                                      c.Id,
                                      FirstName = Utility.ToSafeString(c.FirstName) == "" ? "N/A" : Utility.ToSafeString(c.FirstName),
                                      LastName = Utility.ToSafeString(c.LastName) == "" ? "" : Utility.ToSafeString(c.LastName),
                                      RowNum = Utility.ToSafeString(c.RowNum),
                                      PhoneNumber = (Utility.ToSafeString(c.PhoneNumber) == "" ? "N/A" : Utility.ToSafeString(c.PhoneNumber)),
                                      PhoneNumberConfirmed = Utility.ToSafeBool(c.PhoneNumberConfirmed),
                                      Email = Utility.ToSafeString(c.Email) == "" ? "N/A" : Utility.ToSafeString(c.Email),
                                      EmailConfirmed = Utility.ToSafeBool(c.EmailConfirmed),
                                      Log = (DC.LoginSourse)Utility.ToSafeInt(c.LoginSourse),
                                      Gender = (DC.Gender)Utility.ToSafeInt(c.Gender),
                                      UserName = Utility.ToSafeStringReplaced(c.UserName),
                                      DOB = Utility.ToDateTime(c.DOB) == "" ? "N/A" : Utility.ToDateTimeDDMMMYYYY(c.DOB),
                                      dtmCreate = Utility.ToDateTime((DateTime)c.dtmCreate),
                                      c.Status,
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
        private RegisterUserModel GetData(string Id)
        {
            var model = new AspNetUsersRepository().Get(Id);
            if (model != null)
            {
                var urole = UserManager.GetRoles(Id).FirstOrDefault();
                var role = new AspNetRoleRepository().Get(m => m.Name == urole);
                var usermodel = new RegisterUserModel
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MobNo = model.MobNo,
                    EmailId = model.EmailId,
                    Status = (Status)model.Status,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserName = Utility.ToSafeStringReplaced(model.UserName),
                    Password = DC.Encryption.DecryptCommon(model.PasswordHash),
                    DOB = model.DOB,
                    Gender = (Gender)model.Gender,
                    RoleId = role.Id,
                    LoginSourse = (LoginSourse)model.LoginSourse
                };
                return usermodel;
            }
            else
            {
                return null;
            }
        }

        public PartialViewResult Add(string Id, string Title)
        {
            try
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "Create New User" : Title;
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var model = GetData(Id);
                    model.UserName = Utility.ToSafeStringReplaced(model.UserName);

                    return PartialView("ucAddUser", model);
                }
                else
                {

                    return PartialView("ucAddUser", new RegisterUserModel { Gender = DC.Gender.NA });
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
        public PartialViewResult ViewUser(string Id)
        {
            try
            {
                var model = GetData(Id);
                if (model != null)
                {
                    return PartialView("ucViewUser", model);
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
        [HttpPost]
        public async Task<ActionResult> Add(RegisterUserModel model)
        {
            try
            {
                var repository = new AspNetUsersRepository();
                var vvv = ModelState.Values.FirstOrDefault(m => m.Errors.Count == 1);
                if (model != null && ModelState.IsValid)
                {

                    var dtmNow = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        var checkmail = await repository.IsEmailExist(model.Id, model.Email);
                        if (checkmail)
                        {
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = model.Email + " The email you have entered is already in use. Please enter a different one!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var checkphone = await repository.IsPhoneNoExist(model.Id, model.PhoneNumber);
                    if (string.IsNullOrWhiteSpace(model.PhoneNumber) || checkphone)
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = model.PhoneNumber + " The mobile number you have entered is already in use. Please enter a different one!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    model.UpdatedBy = User.Identity.GetUserId();
                    model.IP = Utility.GetIpAddress();
                    if (string.IsNullOrWhiteSpace(model.Id))
                    {

                        model.CreatedBy = User.Identity.GetUserId();
                        var UserId = string.IsNullOrWhiteSpace(model.Id) ? Guid.NewGuid().ToString() : model.Id;
                        var user = new ApplicationUser()
                        {
                            Id = UserId,

                            PhoneNumber = model.PhoneNumber,
                            eUserName = model.Email,
                            UserName = model.Email,
                            ePassword = DC.Encryption.EncryptCommon(model.Password),
                            Email = model.Email,
                            AccessLevels = "0",
                            DisplayOrder = 0,
                            DOB = model.DOB,
                            dtmCreate = dtmNow,
                            dtmUpdate = dtmNow,
                            EmailConfirmed = model.EmailConfirmed,
                            EmailValidate = false,
                            EmailId = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Gender = model.Gender.GetHashCode(),
                            LastLogin = model.LastLogin,
                            LoginMode = DC.LoginMode.NA.GetHashCode(),
                            LoginThirdParty = model.LoginThirdParty,
                            MobNo = model.PhoneNumber,
                            MobValidate = true,
                            PhoneNumberConfirmed = true,
                            Image = "",
                            Status = DC.Status.Active.GetHashCode(),
                            TimeZone = TimeZone.CurrentTimeZone.StandardName,

                        };
                        var identityManager = new IdentityRoleManager();
                        try
                        {
                            // var result=   identityManager.CreateUser(user, model.Password);
                            var result = await UserManager.CreateAsync(user, model.Password);
                            var roleManager = new IdentityRoleManager();
                            if (result.Succeeded)
                            {
                                if (Utility.CheckAndCreateRoles())
                                {
                                    if (roleManager.AddUserToRole(UserId, model.Role))
                                    {
                                        return Json(new
                                        {
                                            StatusCode = 200,
                                            Message = "User created successfully."
                                        }, JsonRequestBehavior.AllowGet);

                                    }
                                    else
                                    {

                                        return Json(new
                                        {
                                            StatusCode = 400,
                                            Message = "User can not be added in role!"
                                        }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {

                                    return Json(new
                                    {
                                        StatusCode = 400,
                                        Message = "Role not exist!."
                                    }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new
                                {
                                    StatusCode = 400,
                                    Message = result.Errors
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception ex)
                        {
                            var v1 = ex.Message;
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = v1
                            }, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else if (!string.IsNullOrWhiteSpace(model.Id))
                    {
                        model.dtmUpdate = dtmNow;
                        model.UpdatedBy = User.Identity.GetUserId();
                        model.IP = Utility.GetIpAddress();
                        var user = UserManager.FindById(model.Id);
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.eUserName = model.PhoneNumber;
                        user.ePassword = DC.Encryption.EncryptCommon(model.Password);
                        user.Email = model.Email;
                        user.DOB = model.DOB;
                        user.dtmUpdate = dtmNow;
                        user.EmailId = model.Email;
                        user.Gender = model.Gender.GetHashCode();
                        user.MobNo = model.PhoneNumber;
                        var result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            UserManager.RemovePassword(user.Id);
                            UserManager.AddPassword(user.Id, model.Password);
                            return Json(new
                            {
                                StatusCode = 200,
                                Message = model.FirstName + " |  updated successfully."
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                StatusCode = 400,
                                Message = "Some error occur  updating : [ " + model.FirstName + " ] please try later."
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
                        Message = "Please provide correct data to be insert/update."
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
        private void DeleteFile(string file)
        {
            try
            {
                string img = Server.MapPath($"~/Attatchments/{file}");
                string imgthumb = Server.MapPath($"~/Attatchments/th_{file}");
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
                    var result = new AspNetUsersRepository().ActiInactive(Id);
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


        #region UploadUserexcel
        [HttpPost]
        public JsonResult UploadUserExcel(HttpPostedFileBase uploadedFile, FormCollection fc)
        {
            var filename = "";
            logerror.Logerror("UploadUserExcel", Server.MapPath("~/AppLog"));

            try
            {
                //var HubId = fc["HubId"];
                //var HubName = fc["HubName"]; 

                if ((uploadedFile != null && uploadedFile.ContentLength <= 0))
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Upload file is not valid format (.xls|.xlsx) file.",
                    }, JsonRequestBehavior.AllowGet);
                }

                if (uploadedFile != null && uploadedFile.ContentLength > 0)
                {
                    HttpPostedFileBase postedFile = uploadedFile;
                    string fn = Utility.FileName;
                    var UploadedFileName = "";
                    string ext = Path.GetExtension(postedFile.FileName).ToLower();
                    if (Utility.SupportedExcelTypes(ext))
                    {
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            var temp = postedFile.FileName.Split('.');
                            var postedFileName = temp[0];
                            var name = postedFile.FileName;
                            UploadedFileName = name;
                            var contetntype = postedFile.ContentType;
                            if (contetntype != "application/vnd.ms-excel" && contetntype != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            {
                                return Json(new
                                {
                                    statusCode = 400,
                                    Message = "Upload file is not valid format (.xls|.xlsx) file. "
                                }, JsonRequestBehavior.AllowGet);
                            }
                            filename = Server.MapPath("~/UploadFiles/" + name);
                            Utility.Delete(filename);
                            string extension = Path.GetExtension(name);
                            name = Guid.NewGuid().ToString() + extension;
                            filename = Server.MapPath("~/UploadFiles/" + name);
                            postedFile.SaveAs(filename);
                            var uid = User.Identity.GetUserId();
                            logerror.Logerror(filename, Server.MapPath("~/AppLog"));
                            //var excelData = new ProcessExcel().ValidateExcel(filename, uid); 
                            var result = new ProcessExcel().ReadExcelHeaderAndData(filename);
                            if (result.Status == 200)
                            {
                                result.Data._FilePath = name;
                                result.Data.FilePath = name;
                                result.Data.MetaData = "";
                                result.Data.UploadedFileName = UploadedFileName;
                                result.Data.SheetName = result.Data.FileName;
                                var partialviewresult = RenderViewString(ControllerContext, "ucUploadExcel", result.Data);
                                return Json(new
                                {
                                    uc = partialviewresult,
                                    StatusCode = 200,
                                    result.Title,
                                    Message = result.Detail,
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                Utility.Delete(filename);
                                return Json(new
                                {
                                    StatusCode = result.Status,
                                    Title = result.Title,
                                    Message = result.Detail
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            Utility.Delete(filename);
                            return Json(new
                            {
                                StatusCode = 205,
                                Title = "Error Occur",
                                Message = "Please upload file again some problem occur."
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Utility.Delete(filename);
                        return Json(new
                        {
                            StatusCode = 206,
                            Title = "Error Occur",
                            Message = "Please upload file again some problem occur. "
                        }, JsonRequestBehavior.AllowGet);

                    }
                }

                Utility.Delete(filename);
                return Json(new
                {
                    StatusCode = 207,
                    Title = "Error occur",
                    Message = "Please upload file again some problem occur.",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.Delete(filename);
                return Json(new
                {
                    StatusCode = 400,
                    ex.Message,
                    Title = "Error occur",
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SubmitUploadedExcelData(string file, string UploadedFileName, string SheetName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Data file not found."
                    }, JsonRequestBehavior.AllowGet);
                }
                if (!System.IO.File.Exists(Server.MapPath("~/UploadFiles/" + file + "")))
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Data file not exist at given path. "
                    }, JsonRequestBehavior.AllowGet);
                }
                int cols, rows;

                var result = new ProcessExcel().ReadExcelHeaderAndAllData(Server.MapPath("~/UploadFiles/" + file + ""), out cols, out rows);

                if (result.Status == 200)
                {
                    var model = new InserUserExcelMetaData
                    {
                        AspNetUserId = User.Identity.GetUserId(),
                        Cols = cols,
                        Rows = rows,
                        CreatedBy = User.Identity.GetUserId(),
                        Description = "",
                        FileMetaData = "",
                        FileName = file,
                        IPAddress = Utility.IPAddress,
                        SheetName = SheetName,
                        UploadedFileName = UploadedFileName,
                        Students = result.Data
                    };


                    
                    var iresult = new AspNetUsersRepository().InserUserExcel(model);
                    return Json(new
                    {
                        StatusCode = iresult.status,
                        Message = iresult.message,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = result.Status,
                        Title = result.Title,
                        Message = result.Detail
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteExcelFile(string file)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Data not found."
                    }, JsonRequestBehavior.AllowGet);
                }
                if (System.IO.File.Exists(Server.MapPath("~/UploadFiles/" + file + "")))
                {
                    System.IO.File.Delete(Server.MapPath("~/UploadFiles/" + file + ""));
                }
                return Json(new
                {
                    StatusCode = 200,
                    Message = "File deleted succefully. "
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string st = ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

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