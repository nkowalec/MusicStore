using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public RedirectToRouteResult TryLogin(string Login, string Password)
        {
            string rola = MusicStore.Models.Module.User.TryLogin(Login, Password);
            if(rola != "")
            {
                Session["LoginRole"] = rola;
                Session["Login"] = Login;

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        public RedirectToRouteResult LogOff()
        {
            Session["LoginRole"] = null;
            Session["Login"] = null;

            return RedirectToAction("Index", "Home");
        }

    }
}