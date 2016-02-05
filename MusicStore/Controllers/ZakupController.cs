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

        public ActionResult NewOrder()
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Login", "Account");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Klient) return RedirectToAction("Index", "Home");
            Dokument dok = new Dokument();
            dok.StanDokumentu = StanDokumentu.Bufor;
            dok.State = RowState.Added;
            
            return View(dok);
        }

        public ActionResult Podsumowanie(Dokument dok)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Klient) return RedirectToAction("Index", "Home");
            DbModule module = DbModule.GetInstance();
            string[] koszyk = Request.Cookies["Cart"].Value.ToString().Split(',');
            var dict = new System.Collections.Generic.Dictionary<int, int>();

            if (!String.IsNullOrEmpty(Request.Cookies["Cart"].Value))
                foreach (string Ids in koszyk)
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
            foreach (var obiekt in dict)
            {
                var album = module.Albumy.Where(x => x.Id == obiekt.Key).First();
                dict2.Add(album, obiekt.Value);
                wartosc += album.BruttoValue * obiekt.Value;

                if (waluta == "")
                {
                    waluta = album.BruttoSymbol;
                }
                else if (waluta != album.BruttoSymbol)
                {
                    waluta = "(Wiele walut)";
                    wartosc = 0;
                }
            }
            ViewBag.wartosc = wartosc;
            ViewBag.waluta = waluta;
            ViewBag.Pozycje = dict2;
            return View(dok);
        }

        public ActionResult SaveOrder(Dokument dok)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (MusicStore.Models.Module.User.GetCurrentRole() != MusicStore.Models.Module.User.Klient) return RedirectToAction("Index", "Home");

            DbModule module = DbModule.GetInstance();
            dok.DataZamowienia = DateTime.Now;
            dok.NumerDokumentu = Dokument.GetLastNumber();
            module.AddRow(dok);

            string[] koszyk = Request.Cookies["Cart"].Value.ToString().Split(',');
            var dict = new System.Collections.Generic.Dictionary<int, int>();

            if (!String.IsNullOrEmpty(Request.Cookies["Cart"].Value))
                foreach (string Ids in koszyk)
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
            foreach (var obiekt in dict)
            {
                var album = module.Albumy.Where(x => x.Id == obiekt.Key).First();
                dict2.Add(album, obiekt.Value);
                wartosc += album.BruttoValue * obiekt.Value;

                if (waluta == "")
                {
                    waluta = album.BruttoSymbol;
                }
                else if (waluta != album.BruttoSymbol)
                {
                    waluta = "(Wiele walut)";
                    wartosc = 0;
                }
            }
            int lp = 1;
            foreach(var poz in dict2)
            {
                PozycjaDokumentu pozycja = new PozycjaDokumentu(dok);
                pozycja.AlbumName = poz.Key.Nazwa + " (" + poz.Key.Artysta.Nazwa + ")";
                pozycja.CenaBrutto = poz.Key.BruttoValue;
                pozycja.Ilosc = poz.Value;
                pozycja.Lp = lp;
                pozycja.State = RowState.Added;
                pozycja.Waluta = poz.Key.BruttoSymbol;

                module.AddRow(pozycja);

                var album = poz.Key;
                album.StanIlosc -= pozycja.Ilosc;
                module.Update(album);
                lp++;
            }
            ViewBag.NumerDokumentu = dok.NumerDokumentu;
            return View();
        }

        public ActionResult Orders()
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            var userId = MusicStore.Models.Module.User.GetCurrentId();
            DbModule module = DbModule.GetInstance();
            List<Dokument> doks = null;
            if(MusicStore.Models.Module.User.GetCurrentRole() == MusicStore.Models.Module.User.Klient)
                doks = module.Dokumenty.Where(x => x.NumerDokumentu.Split('/')[1] == userId.ToString()).ToList();
            else if(MusicStore.Models.Module.User.GetCurrentRole() == MusicStore.Models.Module.User.Admin)
            {
                doks = module.Dokumenty.ToList();
            }
            return View(doks);
        }

        public ActionResult PrintOrder(int Id)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View(DbModule.GetInstance().Dokumenty.Where(x => x.Id == Id).First());
        }

        public ActionResult PrintFV(int Id)
        {
            if (!MusicStore.Models.Module.User.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View(DbModule.GetInstance().Dokumenty.Where(x => x.Id == Id).First());
        }
    }
}