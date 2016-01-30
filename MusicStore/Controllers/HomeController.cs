using MusicStore.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(DbModule.GetInstance().Albumy);
        }

        public ActionResult Album(int Id)
        {
            return View(DbModule.GetInstance().Albumy.Where(x => x.Id == Id).First());
        }

        public ActionResult Utwor(int Id)
        {
            return View(DbModule.GetInstance().Utwory.Where(x => x.Id == Id).First());
        }
    }
}