using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Album : Row
    {
        [DBItem]
        public int ArtystaId { get; set; }
        [DBItem]
        public decimal StanIlosc { get; set; }
        [DBItem]
        public Currency Brutto { get; set; }
        [DBItem]
        public byte[] Image { get; set; }
        private Artysta artysta = null;
        public Artysta Artysta
        {
            get
            {
                if (artysta == null) artysta = DbModule.GetInstance().Artysci.Where(x => x.Id == ArtystaId).First();
                return artysta;
            }
        }
        [DBItem]
        public bool Blokada { get; set; }
        private List<Utwor> utwory = null;
        public List<Utwor> Utwory
        {
            get
            {
                if(Id != 0)
                {
                    if (utwory == null) utwory = DbModule.GetInstance().Utwory.Where(x => x.AlbumId == Id).ToList();
                }
                else
                {
                    utwory = new List<Utwor>();
                }
                return utwory;
            }
        }

        public Album(Artysta artist)
        {
            Id = 0;
            State = RowState.Added;
            Blokada = false;
            this.artysta = artist;
            ArtystaId = artist.Id;
        }

        private Album() { }
        public static Album Empty = new Album();
    }
}