using MusicStore.Models.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicStore.Controllers
{
    public class AlbumsController : Controller
    {
        // GET: Albums
        public ActionResult Index()
        {
            DbModule module = DbModule.GetInstance();
            return View(module.Albumy);
        }

        public RedirectToRouteResult Delete(int Id)
        {
            DbModule module = DbModule.GetInstance();
            foreach(Utwor utwor in module.Utwory.Where(x=>x.AlbumId == Id))
            {
                module.Delete(utwor);
            }
            module.Delete(module.Albumy.Where(x => x.Id == Id).First());

            return RedirectToAction("Index", "Albums");
        }

        public ActionResult Edit(int Id)
        {
            return View(DbModule.GetInstance().Albumy.Where(x=>x.Id == Id).First());
        }

        public RedirectToRouteResult Save(Album album, HttpPostedFileBase obrazek = null, bool EditAfterSave = false)
        {
            DbModule module = DbModule.GetInstance();
            if(obrazek != null)
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    obrazek.InputStream.CopyTo(stream);
                    var baseStr = Convert.ToBase64String(stream.ToArray());
                    album.Image = Convert.FromBase64String(baseStr);
                }
            }
            if (album.State == RowState.Added) module.AddRow(album);
             else module.Update(album);

            if (EditAfterSave) return RedirectToAction("Edit", "Albums", new { Id = album.Id });

            return RedirectToAction("Index", "Albums");
        }

        public ActionResult AddNew()
        {
            DbModule module = DbModule.GetInstance();
            
            return View(module.Artysci);
        }

        public RedirectToRouteResult NewAlbum(Artysta artysta)
        {
            Album album = new Album(artysta);
            var dict = new Dictionary<string, object>();
            dict.Add("Id", album.Id);
            dict.Add("ArtystaId", album.ArtystaId);
            dict.Add("EditAfterSave", true);
            dict.Add("State", RowState.Added);
            return RedirectToAction("Save", "Albums", new RouteValueDictionary(dict));
        }
    }
}