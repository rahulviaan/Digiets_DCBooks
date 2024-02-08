using DC.Web.App.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.Mvc;

namespace DC.Web.App.Areas.CP.Controllers
{

    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : WFbaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public DashboardController() { }
        public DashboardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult Index()
        {
            return View();
        }
        #region Dashboard
      
        
        
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

        #region ResetPasswordProfile
        public ActionResult ChangePassword()
        {
            //var user = new AspNetUserRepository().GetById(User.Identity.GetUserId());

            var UserId = User.Identity.GetUserId();
            var user = UserManager.FindById(UserId);
            var model = new ResetPasswordModel
            {
                ePassword = DC. Encryption.DecryptCommon(user.PasswordHash),
                Id = user.Id,
            };
            return View(model);
        }
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

                        if (DC. Encryption.DecryptCommon(user.ePassword) != obj.OldPassword)
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
        #endregion
        #endregion 
    }
}