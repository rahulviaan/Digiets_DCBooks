using System.Linq;
using System.Web.Mvc;

namespace DC.Web.App.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NotAutorize(string message)
        {
            var ipAddress = Request.UserHostAddress;
            ViewBag.Message = message;

            return RedirectToAction("Login", "Account", new { area = "" });

        }
        public ActionResult pagenotfound()
        {

            try
            {
                var headers = Request.Headers;
                ViewBag.URL = headers.GetValues("url").First();
                ViewBag.Message = headers.GetValues("message").First();

            }
            catch (System.Exception ex)
            {

                ViewBag.URL = "";
                ViewBag.Message = "Some error occur try later";

            }

            return View();
        }
        public ActionResult generalerror()
        {
            try
            {
                var headers = Request.Headers;
                ViewBag.URL = headers.GetValues("url").First();
                ViewBag.Message = headers.GetValues("message").First();

            }
            catch (System.Exception ex)
            {

                ViewBag.URL = "";
                ViewBag.Message = "Some error occur try later";

            }
            return View();
        }
    }
}