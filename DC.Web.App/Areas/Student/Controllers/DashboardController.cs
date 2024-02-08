using Database.Repository.MasterRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using DC.Web.App.Models;
using System;
using System.Linq;
using Database.Repository;
using System.Threading.Tasks;
using System.IO;

namespace DC.Web.App.Areas.Student.Controllers
{

    [Authorize(Roles = "Student")]
    [RouteArea("Student")]
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
        private RegisterStudentModel GetData()
        {
            string Id = User.Identity.GetUserId();
            var model = new AspNetUsersRepository().GetStudent(Id);
            if (model != null)
            {
                var usermodel = new RegisterStudentModel
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Class = model.Class,
                    Board = model.Board,
                    MasterClassId = model.MasterClassId,
                    MasterBoardId = model.MasterBoardId,
                    MobNo = model.MOB,
                    EmailId = model.Email,
                    Status = (Status)model.Status,
                    PhoneNumber = model.MOB,
                    Email = model.Email,
                    UserName = Utility.ToSafeString(model.UserName),
                    Password = DC.Encryption.DecryptCommon(model.PasswordHash),
                    ConfirmPassword = DC.Encryption.DecryptCommon(model.PasswordHash),
                    DOB = model.DOB,
                    Gender = (Gender)model.Gender,
                    iGender = model.Gender,
                    RoleId = "06d971e6-7b5d-4ba9-9269-dc3ff5f2c00b",
                    RollNo = Utility.ToSafeString(model.RollNo),
                    UserCode = Utility.ToSafeString(model.UserCode),
                    Session = Utility.ToSafeString(model.Session),
                    Address = Utility.ToSafeString(model.Address),
                    SchoolId = model.SchoolId,
                    School = model.School,
                    SchoolCode = model.SchoolCode,

                };
                return usermodel;
            }
            else
            {
                return null;
            }
        }

        // GET: Super/Admin

        [Route("~/student-panel/{_detail?}")]
        public ActionResult Index(string _detail = "")
        {
            var model = GetData();
            return View(model);
        }
        #region ManageStudents           
        [HttpPost]
        [Route("~/student-panel/updatestudent")]
        public ActionResult UpdateStudent(RegisterStudentModel model)
        {
            try
            {

                var repository = new AspNetUsersRepository();
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.Action = DC.Action.Insert;
                }
                else
                {
                    model.Action = DC.Action.Update;
                }

                var userrepository = new AspNetUsersRepository();
                if (!string.IsNullOrWhiteSpace(model.EmailId))
                {
                    var checkmail = userrepository.IsEmailExist(model.Id, model.EmailId).Result;
                    if (checkmail)
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = model.EmailId + " The email you have entered is already in use. Please enter a different one!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrWhiteSpace(model.MobNo))
                {
                    var checkphone = userrepository.IsPhoneNoExist(model.Id, model.MobNo).Result;
                    if (string.IsNullOrWhiteSpace(model.MobNo) || checkphone)
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = model.MobNo + " The contact number you have entered is already in use. Please enter a different one!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                model.dtmCreate = DateTime.Now;
                model.dtmUpdate = DateTime.Now;
                model.CreatedBy = User.Identity.GetUserId();
                model.UpdatedBy = User.Identity.GetUserId();
                model.IPAddress = Utility.IPAddress;
                model.Password = Encryption.EncryptCommon(model.Password);
                var result = repository.InsertUpdateSchoolStudent(model);
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

        #endregion ManageStudents 


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