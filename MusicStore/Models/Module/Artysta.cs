using System.Collections.Generic;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Artysta
    {
        [DBItem]
        public int Id { get; set; }
        public RowState State { get; set; } = RowState.Unchanged;
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