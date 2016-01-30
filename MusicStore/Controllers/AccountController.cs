using MusicStore.Models.Module;
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
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Admin) return RedirectToAction("Index", "Home");

            return View(DbModule.GetInstance().Users);
        }
        public ActionResult Login()
        {
            return View();
        }

        public RedirectToRouteResult Delete(int Id)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Admin) return RedirectToAction("Index", "Home");
            DbModule module = DbModule.GetInstance();
            var user = module.Users.Where(x => x.Id == Id).First();

            module.Delete(user);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Edit(int Id)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Admin) return RedirectToAction("Index", "Home");
            return View(DbModule.GetInstance().Users.Where(x => x.Id == Id).First());
        }

        public RedirectToRouteResult AddNew()
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Admin) return RedirectToAction("Index", "Home");
            User user = new Models.Module.User();
            return RedirectToAction("Save", "Account", user);
        }

        public RedirectToRouteResult Save(User user)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Admin) return RedirectToAction("Index", "Home");
            DbModule module = DbModule.GetInstance();
            if (user.Id > 0)
            {
                module.Update(user);
            }
            else
            {
                module.AddRow(user);
                return RedirectToAction("Edit", "Account", new { Id = user.Id });
            }
            return RedirectToAction("Index", "Account");
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