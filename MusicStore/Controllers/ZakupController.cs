using MusicStore.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class ZakupController : Controller
    {
        // GET: Zakup
        public ActionResult Index()
        {
            DbModule module = DbModule.GetInstance();
            string[] koszyk = Request.Cookies["Cart"].Value.ToString().Split(',');
            var dict = new System.Collections.Generic.Dictionary<int, int>();

            if(!String.IsNullOrEmpty(Request.Cookies["Cart"].Value))
            foreach(string Ids in koszyk)
{
                int Id = int.Parse(Ids);
                if (!dict.ContainsKey(Id))
                {
                    dict.Add(Id, 1);
                }
                else
                {
                    dict[Id] += 1;
                }
            }

            var dict2 = new System.Collections.Generic.Dictionary<Album, int>();
            decimal wartosc = 0;
            string waluta = "";
            foreach(var obiekt in dict)
            {
                var album = module.Albumy.Where(x => x.Id == obiekt.Key).First();
                dict2.Add(album, obiekt.Value);
                wartosc += album.BruttoValue * obiekt.Value;

                if(waluta == "")
                {
                    waluta = album.BruttoSymbol;
                }
                else if(waluta != album.BruttoSymbol)
                {
                    waluta = "(Wiele walut)";
                    wartosc = 0;
                }
            }
            ViewBag.wartosc = wartosc;
            ViewBag.waluta = waluta;

            return View(dict2);
        }
    }
}