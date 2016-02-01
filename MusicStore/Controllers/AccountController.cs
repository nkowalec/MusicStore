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
            return View(DbModule.GetInstance().Users.Where(x => x.Id == Id).First());
        }

        public RedirectToRouteResult AddNew()
        {
            User user = new Models.Module.User();
            return RedirectToAction("Save", "Account", user);
        }

        public RedirectToRouteResult Save(User user)
        {
            DbModule module = DbModule.GetInstance();
            if (user.Id > 0)
            {
                if (module.Users.Where(x => x.Login == user.Login).ToList().Count > 0)
                {
                    module.Delete(user);
                    return RedirectToAction("Index");
                }
                else
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
                Session["LoginId"] = MusicStore.Models.Module.DbModule.GetInstance().Users.Where(x => x.Login == Login).First().Id;

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        public RedirectToRouteResult LogOff()
        {
            Session["LoginRole"] = null;
            Session["Login"] = null;
            Session["LoginId"] = null;

            return RedirectToAction("Index", "Home");
        }

    }
}