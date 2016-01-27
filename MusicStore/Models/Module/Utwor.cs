using System.Linq;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Utwor : Row
    {
        [DBItem]
        public int AlbumId { get; set; } = -1;
        private Album album = null;
        public Album Album {
            get
            {
                if (album == null) 
                album = DbModule.GetInstance().Albumy.Where(x => x.Id == AlbumId).First();
                return album;
            }
        }
        [DBItem]
        public string Tytul { get; set; } = "";
        [DBItem]
        public string Wykonawca { get; set; } = "";
        [DBItem]
        public string Info { get; set; } = "";
        [DBItem]
        public string Tekst { get; set; } = "";
        [DBItem]
        public string YtLink { get; set; } = "";

        public Utwor(Album album)
        {
            this.album = album;
            AlbumId = album.Id;
            State = RowState.Added;
            Id = 0;
        }
        public Utwor() { }

    }
}