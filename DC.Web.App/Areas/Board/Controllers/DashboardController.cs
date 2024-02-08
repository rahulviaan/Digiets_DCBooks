using Database;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Web.App.Models;
using Database.Repository.MasterRepository;

namespace DC.Web.App.Areas.Board.Controllers   
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

        #region Manage Board
        public JsonResult Board()
        {
            try
            {
                var results = new BoardRepository().SelectList().ToList();
                if (results == null || results.Count() <= 0)
                {
                    return Json(new
                    {
                        StatusCode = 400,
                        Message = "Data Not Found."
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    StatusCode = 200,
                    results = results,
                    Message = "Success. "
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string st =    ex.Message;
                return Json(new
                {
                    StatusCode = 400,
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Gets()
        {
            try
            {
                var repository = new BoardRepository();
                var datas = new List<MasterBoard>();
                datas = repository.Gets().ToList();
                if (datas != null && datas.Count() > 0)
                {
                    var results = from c in datas
                                  orderby c.DisplayOrder ascending
                                  select new
                                  {
                                      Id = c.Id,
                                      Title = Utility.ToSafeString(c.Title),
                                      Image = Utility.ToSafeString(c.Image),
                                      MasterPublisherId = Utility.ToSafeLong(c.MasterPublisherId),
                                      Description = Utility.ToSafeString(c.Description),
                                      DisplayOrder = Utility.ToSafeString(c.DisplayOrder),
                                      CreatedDate = Utility.ToDateTime((DateTime)c.CreatedDate),
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
        private BoardModel GetData(long Id)
        {
            var model = new BoardRepository().Get(Id);
            if (model != null)
            {
                return new BoardModel
                {
                    Id = Utility.ToSafeInt(model.Id),
                    Title = Utility.ToSafeString(model.Title),
                    Title1 = Utility.ToSafeString(model.Title),
                    Description = Utility.ToSafeString(model.Description),
                    DisplayOrder = Utility.ToSafeInt(model.DisplayOrder),                    
                    Image = Utility.ToSafeString(model.Image),
                    AspNetUserId = Utility.ToSafeString(model.CreatedBy),
                    IPAddress = Utility.ToSafeString(model.IPAddress),
                    Status = (Status)Utility.ToSafeInt(model.Status),
                };
            }
            else
            {
                return null;
            }
        }
        public PartialViewResult Add(long Id, int PublisherId = 0, string Title = "")
        {
            try
            {
                Title = string.IsNullOrWhiteSpace(Title) ? "Create New Board" : Title;
                if (Id > 0)
                {
                    var model = GetData(Id);
                    if (model != null)
                    {

                        return PartialView("ucAdd", model);
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
                    return PartialView("ucAdd", new BoardModel { Title = "" });
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
        public ActionResult Add(BoardModel model)
        {
            try
            {
                model.ActionTaken = DC.Action.NA;
                var repository = new BoardRepository();
                if (model != null && ModelState.IsValid)
                {
                    model.UpdatedDate = DateTime.Now;
                    model.Status = Status.Active;
                    model.AspNetUserId = User.Identity.GetUserId();
                    model.IPAddress = Utility.IPAddress;
                    model.GlobalId = Guid.NewGuid().ToString();
                    if (repository.IsExist(model.Id, model.Title))
                    {
                        return Json(new
                        {
                            StatusCode = 400,
                            Message = model.Title + " already exist. Please try another name."
                        }, JsonRequestBehavior.AllowGet);
                    }

                    if (Utility.ToSafeLong(model.Id) == 0)
                    {
                        model.ActionTaken = DC.Action.Insert;
                    }
                    else
                    {
                        model.ActionTaken = DC.Action.Update;
                    }
                    var result = repository.InsertUpdateDelete(model);
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
        public ActionResult IsExist(long Id, string Name)
        {
            var isExist = new BoardRepository().IsExist(Id, Name);
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
                    Message = Name + " | already exist please try another name."
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult View(long Id)
        {
            var model = GetData(Id);
            if (model != null)
            {
                return PartialView("ucView", model);
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
        public ActionResult ActiveInactive(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    var result = new BoardRepository().ActiInactive(Id, User.Identity.GetUserId());
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
        public ActionResult Delete(long Id)
        {
            try
            {
                if (Id > 0)
                {
                    var result = new BoardRepository().InsertUpdateDelete(new BoardModel { Id = Id, ActionTaken = DC.Action.Delete });
                    if (result != null)
                    {
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
                            Message = "Please check internet some error occur while deleting Board.Please try again later."
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
        #endregion

    }
}