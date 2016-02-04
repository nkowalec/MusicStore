using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class PozycjaDokumentu : Row
    {
        [DBItem]
        public int DokumentId { get; set; } = -1;

        [DBItem]
        public int Lp { get; set; } = 1;
        public Dokument Dokument
        {
            get
            {
                return DbModule.GetInstance().Dokumenty.Where(x => x.Id == DokumentId).First();
            }
        }
        [DBItem]
        public string AlbumName { get; set; } = "";
        [DBItem]
        public decimal Ilosc { get; set; } = 0;
        [DBItem]
        public decimal CenaBrutto { get; set; } = 0;
        [DBItem]
        public string Waluta { get; set; } = "PLN";
        [DBItem]
        public string Jednostka { get; } = "szt";

        public PozycjaDokumentu()
        {
            State = RowState.Unchanged;
            Id = -1;
        }

        public PozycjaDokumentu(Dokument dokument)
        {
            DokumentId = dokument.Id;
            State = RowState.Added;
        }
    }

}