using MusicStore.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class UtworyController : Controller
    {
        // GET: Utwory
        public ActionResult Index(int Id)
        {
            ViewBag.AlbumId = Id;
            ViewBag.Title = DbModule.GetInstance().Albumy.Where(x => x.Id == Id).First().Nazwa;
            var utwory = DbModule.GetInstance().Utwory.Where(x => x.AlbumId == Id).ToList();
            return View(utwory);
        }

        public ActionResult Edit(int Id)
        {
            return View(DbModule.GetInstance().Utwory.Where(x => x.Id == Id).First());
        }
        public RedirectToRouteResult Save(Utwor utwor)
        {
            if(utwor.State == RowState.Added)
            {
                DbModule.GetInstance().AddRow(utwor);
                return RedirectToAction("Edit", "Utwory", new { Id = utwor.Id });
            }
            else
            {
                DbModule.GetInstance().Update(utwor);
                return RedirectToAction("Index", "Utwory", new { Id = utwor.AlbumId });
            }
        }

        public RedirectToRouteResult Delete(int Id)
        {
            DbModule module = DbModule.GetInstance();
            var utwor = module.Utwory.Where(x => x.Id == Id).First();

            module.Delete(utwor);

            return RedirectToAction("Index", "Utwory", new { Id = utwor.AlbumId });
        }

        public RedirectToRouteResult AddNew(int Id)
        {
            Utwor utwor = new Utwor(DbModule.GetInstance().Albumy.Where(x => x.Id == Id).First());
            return RedirectToAction("Save", "Utwory", utwor);
        }
    }
}