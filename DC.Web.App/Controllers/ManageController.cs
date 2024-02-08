using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Database;
using Database.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DC.Web.App.Models;

namespace DC.Web.App.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }
        public ActionResult AccountDetail()
        {
            Response<AspNetUserModel> message = new Response<AspNetUserModel>
            {
                Title = "Account Detail",
                Detail = "Account detail not found.",
                Status = 201,
                Data = null
            };
            try
            {
                var username = User.Identity.GetUserName();
                var claim = ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("FirstName");
                var AspNetUserId = User.Identity.GetUserId();
                var UserName = User.Identity.GetUserName();
                if (claim != null)
                {

                    UserName = claim.Value;
                }


                message.Title = $"Welcome {UserName}!";
                if (!string.IsNullOrWhiteSpace(AspNetUserId))
                {
                    var result = new AspNetUserModel();
                    var aspnetuser = new AspNetUsersRepository().Get(AspNetUserId);
                    new MapObjects<AspNetUser, AspNetUserModel>().Copy(ref aspnetuser, ref result);
                    var urole = UserManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();


                    result.Role = urole;
                    message.Title = $"Welcome {UserName}!";
                    message.Data = result;
                    message.Status = 200;
                    message.Detail = $"Account Detail";
                    return View(message);
                }
                else
                {
                    message.Title = "Account Details :: Login";
                    message.Status = 203;
                    message.Detail = "Please login to see your account details.";
                    return View(message);
                }
            }
            catch (Exception ex)
            {
                var vq = ex.Message;
                message.Title = "Account Details :: Error";
                message.Status = 400;
                message.Detail = "Account detail not fetched try again";
                return View(message);
            }
        }
        public async Task<ActionResult> UploadUserImage(FormCollection fc)
        {

            try
            {

                var UserId = User.Identity.GetUserId();
                string ext = "";
                string rootpath = Server.MapPath("~/Attatchments/");
                string filename = "";

                var size = 0;
                foreach (var item in Request.Files)
                {
                    var file = Request.Files[item.ToString()];
                    ext = Path.GetExtension(file.FileName);
                    if (!Utility.SupportedTypes(ext))
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = "Please upload valid image type",
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

                var model = new AspNetUser
                {
                    Id = UserId,

                };
                foreach (var item in Request.Files)
                {
                    var file = Request.Files[item.ToString()];
                    filename = Utility.getFileName();
                    ext = Path.GetExtension(file.FileName);
                    var newfilename = filename + ext;
                    var tempfilename = rootpath + newfilename;
                    filename = rootpath + "th_" + newfilename;
                    file.SaveAs(tempfilename);
                    var resultresize = Utility.Image_resize(tempfilename, filename, 100);
                    model.Image = newfilename;
                }
                var repository = new AspNetUsersRepository();
                model = repository.UpdateUserImage(model);

                if (model != null)
                {
                    Utility.Delete(rootpath + model.Image);
                    Utility.Delete(rootpath + "th_" + model.Image);
                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "User image updated sucessfully.",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Some problem occur please try again later."
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

        public JsonResult UpdateMobileNo(string MobNo)
        {
            try
            {
                var repository = new AspNetUsersRepository();
                var UserId = User.Identity.GetUserId();
                var checkphone = repository.IsPhoneNoExist(UserId, MobNo).Result;
                if (string.IsNullOrWhiteSpace(MobNo) || checkphone)
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = MobNo + " The mobile number you have entered is already in use. Please enter a different one!"
                    }, JsonRequestBehavior.AllowGet);
                }
                var result = repository.UpdateUserMobileNo(new AspNetUser { Id = UserId, PhoneNumber = MobNo, MobNo = MobNo });
                if(result!=null)
                {
                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "User mobile number updated sucessfully.",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Some problem occur please try again later."
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

        public JsonResult UpdateUser(AspNetUser model)
        {
            try
            {
                var repository = new AspNetUsersRepository();
                var UserId = User.Identity.GetUserId();
                var checkphone = repository.IsEmailExist(UserId, model.Email).Result;
                if (!string.IsNullOrWhiteSpace(model.Email) && checkphone)
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = model.Email + " The email you have entered is already in use. Please enter a different one!"
                    }, JsonRequestBehavior.AllowGet);
                }
                model.Id = UserId;
                var result = repository.UpdateUser(model);
                if (result != null)
                {
                    return Json(new
                    {
                        StatusCode = 200,
                        Message = "User detail updated sucessfully.",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Some problem occur please try again later."
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

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            var UserId = User.Identity.GetUserId();
            var user = UserManager.FindById(UserId);
            var model = new ResetPasswordModel
            {
                ePassword = DC.Encryption.DecryptCommon(user.PasswordHash),
                Id = user.Id,
            };
            return View(model);
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(ResetPasswordModel obj)
        {
            try
            {
                if (obj != null)
                {
                    dynamic result = null;
                    if (obj.Id != "")
                    {
                        var password = obj.Password;
                        var user = UserManager.FindById(obj.Id);

                        if (DC.Encryption.DecryptCommon(user.ePassword) != obj.OldPassword)
                        {
                            return Json(new { data = "Please enter correct old password." });
                        }
                        user.ePassword = DC.Encryption.EncryptCommon(password);
                        user.dtmUpdate = DateTime.Now;
                        result = UserManager.Update(user);
                        if (result.Succeeded)
                        {
                            UserManager.RemovePassword(user.Id);
                            UserManager.AddPassword(user.Id, password);
                        }
                        return Json(new { data = "1" });
                    }
                    else
                    {
                        return Json(new { data = "Error occured try later." });
                    }
                }
                else
                {
                    return Json(new { data = "Please provide data object." });
                }
            }
            catch (Exception ex)
            {

                return Json(new { data = ex.Message });
            }

        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}