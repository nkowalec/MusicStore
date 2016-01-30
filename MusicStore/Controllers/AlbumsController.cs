using MusicStore.Models.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            module.Delete(module.Albumy.Where(x => x.Id == Id).First());

            return RedirectToAction("Index", "Albums");
        }

        public ActionResult Edit(int Id)
        {
            return View(DbModule.GetInstance().Albumy.Where(x=>x.Id == Id).First());
        }

        public FileContentResult Save(Album album, HttpPostedFileBase obrazek = null)
        {
            DbModule module = DbModule.GetInstance();
            if(obrazek != null)
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    obrazek.InputStream.CopyTo(stream);
                    var baseStr = Convert.ToBase64String(stream.ToArray());
                    album.Image = Convert.FromBase64String(baseStr);

                    return File(album.Image, "image");
                }
            }
            if (album.State == RowState.Added) module.AddRow(album);
             else module.Update(album);

            //  return RedirectToAction("Index", "Albums");
            return null;
        }

        public ActionResult AddNew()
        {
            DbModule module = DbModule.GetInstance();
            
            return View(module.Artysci);
        }

        public RedirectToRouteResult NewAlbum(Artysta artysta)
        {
            Album album = new Album(artysta);

            return RedirectToAction("Edit", "Albums", album);
        }
    }
}