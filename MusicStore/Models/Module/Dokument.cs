using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Dokument : Row
    {
        public Dokument()
        {
            this.State = RowState.Unchanged;
            this.Id = -1;
        }

        #region Kontrahent
        [DBItem]
        public string KontrahentNazwa { get; set; } = "";
        [DBItem]
        public string Miasto { get; set; } = "";
        [DBItem]
        public string UlicaNumer { get; set; } = "";
        [DBItem]
        public string KodPocztowy { get; set; } = "";
        [DBItem]
        public string Telefon { get; set; } = "";
        [DBItem]
        public string EMAIL { get; set; } = "";
        #endregion

        #region Dokument
        private StanDokumentu stan;
        private int stanInt;
        [DBItem]
        public int stanDokumentuInt
        {
            get
            {
                return stanInt;
            }
            set
            {
                stanInt = value;
                stan = (StanDokumentu)value;
            }
        }
        public StanDokumentu StanDokumentu
        {
            get
            {
                return stan;
            }
            set
            {
                stan = value;
                stanDokumentuInt = (int)value;
            }
        }
        [DBItem]
        public DateTime DataZamowienia { get; set; } = DateTime.Now;
        [DBItem]
        public string NumerDokumentu { get; set; } = "";

        public PozycjaDokumentu[] PozycjeDokumentu
        {
            get
            {
                return DbModule.GetInstance().PozycjeDokumentow.Where(x => x.DokumentId == this.Id).ToArray();
            }
        }

        public decimal Wartosc
        {
            get
            {
                decimal wart = 0;
                foreach(var poz in PozycjeDokumentu)
                {
                    wart += poz.CenaBrutto * poz.Ilosc;
                }
                return wart;
            }
        }

        public string Waluta
        {
            get
            {
                string waluta = "";
                foreach (var poz in PozycjeDokumentu)
                {
                    if (waluta == "")
                    {
                        waluta = poz.Waluta;
                    }
                    else if(waluta != poz.Waluta)
                    {
                        waluta = "(Wiele walut)";
                    }
                }
                return waluta;
            }
        }

        internal static string GetLastNumber()
        {
            var Context = HttpContext.Current;
            string Numer = "ZO/";
            Numer += Context.Session["LoginId"].ToString() + "/";
            Numer += DateTime.Now.Year.ToString() + "/";

            var doksy = DbModule.GetInstance().Dokumenty.Where(x => x.NumerDokumentu.Split('/')[1] == Context.Session["LoginId"].ToString()).OrderBy(x => x.NumerDokumentu);
            if(doksy.Count() > 0)
            {
                int index = int.Parse(doksy.Last().NumerDokumentu.Split('/')[3]);
                ++index;
                Numer += index.ToString();
            }
            else
            {
                Numer += "1";
            }

            return Numer;
        }
        #endregion
    }

    public enum StanDokumentu
    {
        Bufor,
        Zamowiony,
        Zatwierdzony,
        Anulowany
    }

}