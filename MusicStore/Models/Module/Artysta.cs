using System.Collections.Generic;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Artysta : Row
    {
        [DBItem]
        public string Nazwa { get; set; }
        [DBItem]
        public string Opis { get; set; }
        public List<Album> Albumy { get; set; }

        public Artysta()
        {
            State = RowState.Added;
            Id = 0;
            Albumy = new List<Album>();
        }
    }
}