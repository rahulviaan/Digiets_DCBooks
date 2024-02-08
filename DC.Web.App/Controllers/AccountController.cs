using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using System.Speech.Synthesis;
using DC.Web.App.Models;
using Database;
using Database.Repository;

namespace DC.Web.App.Controllers
{
    [Authorize]
    public class AccountController : WFbaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //public void CreateNewUser()
        //{
        //    var UserId = Guid.NewGuid().ToString();
        //    var user = new ApplicationUser();

        //        user.Id = UserId;
        //        user.UserName = "avanish";
        //        user.PhoneNumber = "9813778613";
        //        user.eUserName = "9813778613";
        //        user.ePassword = DC.Encryption.EncryptCommon("123456");
        //        user.Email = "avanihgkumarsin@gmal.com";
        //        user.AccessLevels = "0";
        //        user.DisplayOrder = 0;
        //        user.DOB = DateTime.Now.AddYears(-45);
        //        user.dtmCreate = DateTime.Now;
        //        user.dtmUpdate = DateTime.Now;
        //        user.EmailConfirmed = false;
        //        user.EmailValidate = false;
        //        user.EmailId = "avanihgkumarsin@gmal.com";
        //        user.FirstName = "Avanish";
        //        user.LastName = "Kumar Singh";
        //        user.Gender = -1;
        //        user.LoginMode = DC.LoginMode.NA.GetHashCode();
        //        user.LoginThirdParty = false;
        //        user.MobNo = "9813778613";
        //        user.MobValidate = false;
        //        user.PhoneNumberConfirmed = false;
        //        user.Image = "";
        //        user.LoginSourse = DC.Sourse.SelfWebsite.GetHashCode();
        //        user.Status = DC.Status.Active.GetHashCode();
        //    user.TimeZone = TimeZone.CurrentTimeZone.StandardName;

        //    var result = UserManager.Create(user, "123456");
        //    if (result.Succeeded)
        //    {
        //        Utility.CheckAndCreateRoles();
        //        new IdentityRoleManager().AddUserToRole(UserId, Roles.User);
        //    }
        //}
        //
        // GET: /Account/Login

        [AllowAnonymous]
        [Route("~/terms-conditions")]
        [Route("~/terms-of-use")]
        public ActionResult _Terms_Conditions()
        {

            return View();
        }

        [AllowAnonymous]
        [Route("~/privacy-policy")]
        public ActionResult _Privacy_Policy()
        {

            return View();
        }
        [AllowAnonymous]
        [Route("~/contact-us")]
        public ActionResult _Contact_Us()
        {

            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {



            LoginViewModel obj = new LoginViewModel();
            obj.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
            obj.CapImageText = Convert.ToString(Session["Captcha"]);
            ViewBag.ReturnUrl = returnUrl;
            return View(obj);
        }
        [AllowAnonymous]
        public async Task<JsonResult> SpeechPlay()
        {
            try
            {
                Task<JsonResult> task = Task.Run(() =>
                {
                    using (SpeechSynthesizer synth = new SpeechSynthesizer())
                    {

                        using (MemoryStream stream = new MemoryStream())
                        {

                            MemoryStream streamAudio = new MemoryStream();
                            System.Media.SoundPlayer m_SoundPlayer = new System.Media.SoundPlayer();
                            synth.SetOutputToWaveStream(streamAudio);
                            synth.Speak(Convert.ToString(Session["Captcha"]));
                            //streamAudio.Position = 0;
                            //m_SoundPlayer.Stream = streamAudio;
                            //m_SoundPlayer.Play();
                            var base64string = Convert.ToBase64String(streamAudio.ToArray());

                            synth.SetOutputToNull();
                            return Json(new
                            {
                                base64string = base64string,
                                StatusCode = 200,
                                Message = "Success"
                            }, JsonRequestBehavior.AllowGet);

                        }
                    }
                });
                return await task;
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    base64string = "",
                    StatusCode = 400,
                    Message = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]

        [AllowAnonymous]
        public JsonResult RefreshCapcha()
        {
            var img = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
            var CapImageText = Convert.ToString(Session["Captcha"]);

            return Json(new
            {
                StatusCode = 200,
                Message = "Success",
                img,
                CapImageText
            }, JsonRequestBehavior.AllowGet);
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {


            if (!ModelState.IsValid)
            {
                model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
                model.CapImageText = Convert.ToString(Session["Captcha"]);
                model.CaptchaCodeText = "";
                ModelState.AddModelError("", "!! Please check your Captcha Code Text.");
                return View(model);
            }
            if (!model.NotRobot)
            {
                model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
                model.CapImageText = Convert.ToString(Session["Captcha"]);
                model.CaptchaCodeText = "";
                ModelState.AddModelError("", "!! Please check checkbox I am not robot.");
                return View(model);
            }
            if (!ModelState.IsValid || ((Session["Captcha"]) == null && (Convert.ToString(Session["Captcha"]).Trim().Length != 4)))
            {
                model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
                model.CapImageText = Convert.ToString(Session["Captcha"]);
                model.CaptchaCodeText = "";
                ModelState.AddModelError("", "!! Please check your Captcha Code Text.");
                return View(model);
            }
            var CapImageText = Convert.ToString(Session["Captcha"]);
            if (CapImageText != model.CaptchaCodeText)
            {
                model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
                model.CapImageText = Convert.ToString(Session["Captcha"]);
                model.CaptchaCodeText = "";
                ModelState.AddModelError("", "!! Please check your Captcha Code Text.");
                return View(model);
            }
            model.Email = model.Email.Trim();
            string errorMessage = string.Empty;
            model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
            model.CapImageText = Convert.ToString(Session["Captcha"]);
            model.CaptchaCodeText = "";


            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            AspNetUser aspnetuser = new AspNetUser();
            var userRepository = new AspNetUsersRepository();
            if (!(result == SignInStatus.Success))
            {
                aspnetuser = userRepository.FindByUserName(model.Email);
                if (aspnetuser != null)
                {
                    result = await SignInManager.PasswordSignInAsync(aspnetuser.UserName, model.Password, model.RememberMe, shouldLockout: false);
                }
            }

            switch (result)
            {

                case SignInStatus.Success:
                    string userId = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                    var user = SignInManager.UserManager.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
                    var roles = await UserManager.GetRolesAsync(userId);
                    var role = roles.FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(aspnetuser.Id))
                        aspnetuser = userRepository.Get(userId);
                    //switch (roles.Contains(Roles.SuperAdmin))
                    //{
                    //    default:
                    //        break;
                    //}
                    if (Utility.ToSafeInt(user.Status) != 1)
                    {
                        model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
                        model.CapImageText = Convert.ToString(Session["Captcha"]);
                        model.CaptchaCodeText = "";

                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        ModelState.AddModelError("", "Please contact admin.In active user , you are not authorized to Access. ");
                        return View(model);
                    }
                    var isexist = await UserManager.IsInRoleAsync(userId, Roles.SuperAdmin);

                    if (isexist || roles.Contains(Roles.SuperAdmin) || roles.Contains(Roles.Admin))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "CP" });
                    }
                    else if (isexist || roles.Contains(Roles.User))
                    {
                        return RedirectToAction("dashboard", "my", new { area = "" });
                    }
                    else if (isexist || roles.Contains(Roles.School))
                    {
                        return RedirectToAction(model.Email, "school_admin", new { area = "" });
                    }
                    else if (isexist || roles.Contains(Roles.Student))
                    {
                        return RedirectToAction(model.Email, "student-panel", new { area = "" });
                    }



                    else
                    {
                        model.CapImage = "data:image/png;base64," + Convert.ToBase64String(new CaptchaHelper().VerificationTextGenerator());
                        model.CapImageText = Convert.ToString(Session["Captcha"]);
                        model.CaptchaCodeText = "";
                        ModelState.AddModelError("", "!! Please check your Captcha Code Text.");
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        ModelState.AddModelError("", "Please contact admin. you are not authorized to Access.");
                        return View(model);
                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }




        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var claim = ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("FirstName");
            if (claim != null)
            {
                UserManager.RemoveClaim(User.Identity.GetUserId(), new System.Security.Claims.Claim("FirstName", claim.Value));
            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account", new { area = "" });
        }



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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login", "Account");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}