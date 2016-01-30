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

        }
    }
}