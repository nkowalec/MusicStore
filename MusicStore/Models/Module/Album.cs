using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Album
    {
        [DBItem]
        public int Id { get; set; }
        public RowState State { get; private set; } = RowState.Unchanged;
        [DBItem]
        public int ArtystaId { get; set; }
        [DBItem]
        public decimal StanIlosc { get; set; }
        [DBItem]
        public Currency Brutto { get; set; }
        [DBItem]
        public byte[] Image { get; set; }
        public Artysta Artysta { get; set; }
        [DBItem]
        public bool Blokada { get; set; }
        public List<Utwor> Utwory { get; set; }

        public Album(Artysta artist)
        {
            Id = 0;
            State = RowState.Added;
            Blokada = false;
            Utwory = new List<Utwor>();
            Artysta = artist;
            ArtystaId = artist.Id;
        }
    }
}