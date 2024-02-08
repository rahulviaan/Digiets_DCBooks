using System;
using System.Web.Mvc;
using System.Web;
using ErrorLogger;
using System.IO;

namespace DC.Web.App.Models
{

    public class Error
    {
        public string Message { get; set; }
    }
    [AuthorizeIPAddress]
    public class WFbaseController : Controller
    {
        private ILog logerror;

        public WFbaseController()
        {
            logerror = Logger.GetInstance;
        }
        public string RenderViewString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");
            var viewData = new ViewDataDictionary(model);
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
        protected override void HandleUnknownAction(string actionName)
        {
            Exception ex = Server.GetLastError();
            try
            {
                var path = Server.MapPath("~/AppLog");
                var err = ex == null ? "" : ex.ToString();
                logerror.Logerror("ControllerUnknownAction:: " + err, path);
            }
            catch
            {


            }

            if (Request.IsAjaxRequest())
            {
                var error = new Models.Error()
                {
                    Message = actionName + " not exist in " + ControllerContext.RouteData.Values["controller"].ToString()
                };
                PartialView("ucError", error).ExecuteResult(this.ControllerContext);
            }
            else
            {
                ViewBag.Message = actionName + " not exist in " + ControllerContext.RouteData.Values["controller"].ToString();
                View("Error").ExecuteResult(this.ControllerContext);
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var path = Server.MapPath("~/AppLog");
                logerror.Logerror("ControllerOnException:: " + filterContext.Exception.ToString(), path);
                var ctrl = ControllerContext.RouteData.Values["controller"].ToString();
                filterContext.ExceptionHandled = true;
                var mesg = filterContext.Exception.Message + "|";
                ViewBag.Message = mesg;
                RedirectToAction("Login", "Account", new { @area = "" });
                if (Request.IsAjaxRequest())
                {
                    var error = new Error()
                    {
                        Message = mesg
                    };
                    PartialView("ucError", error).ExecuteResult(this.ControllerContext);
                }
                else
                {
                    HandleErrorInfo error = new HandleErrorInfo(filterContext.Exception, this.ControllerContext.ToString(), "Temp");
                    View("Error", error).ExecuteResult(this.ControllerContext);
                }
                base.OnException(filterContext);
            }
            catch (Exception ex)
            {

                var v1 = ex.Message;
            }


        }

    }

    public class AuthorizeIPAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        { 
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string actionName = context.ActionDescriptor.ActionName;
            string controllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
             
            base.OnActionExecuting(context);
        }

     
    }
}