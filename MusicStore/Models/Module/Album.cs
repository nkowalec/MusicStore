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
        public string Nazwa { get; set; } = "";
        [DBItem]
        public int ArtystaId { get; set; } = -1;
        [DBItem]
        public decimal StanIlosc { get; set; } = 0;
        [DBItem]
        public decimal BruttoValue { get; set; } = 0;
        [DBItem]
        public string BruttoSymbol { get; set; } = "";
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
            set
            {
                artysta = value;
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
            Id = -1;
            State = RowState.Added;
            Blokada = false;
            this.artysta = artist;
            ArtystaId = artist.Id;
        }

        public Album() { }

        public override string ToString()
        {
            return this.Nazwa;
        }
    }
}