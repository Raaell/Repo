using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Repositorio.Metodos
{
    public class VerificaSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Usuario"] != null)
            {
                return;
            }else
            {
                filterContext.Result = new RedirectLogin().Redirect();
            }
        }
    }

    class RedirectLogin : Controller
    {
        public ActionResult Redirect()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}