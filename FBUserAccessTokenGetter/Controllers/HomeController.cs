using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FBUserAccessTokenGetter.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return RedirectToAction("Facebook", "Account");
        }

        public ActionResult AfterLogin()
        {
            return View();
        }

    }
}
