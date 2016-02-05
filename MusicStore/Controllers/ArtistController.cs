using MusicStore.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class ArtistController : Controller
    {
        // GET: Artist
        public ActionResult Index()
        {
            return View(DbModule.GetInstance().Artysci.ToList());
        }

        public ActionResult Edit(Artysta artysta)
        {
            return View(artysta);
        }

        public RedirectToRouteResult AddNew()
        {
            Artysta artysta = new Artysta();
            artysta.State = RowState.Added;

            return RedirectToAction("Edit", "Artist", artysta);
        }

        public RedirectToRouteResult Save(Artysta artysta)
        {
            DbModule module = DbModule.GetInstance();
            if (artysta.State == RowState.Added) module.AddRow(artysta);
            else module.Update(artysta);
            return RedirectToAction("Index", "Artist");
        }
    }
}