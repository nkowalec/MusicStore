namespace MusicStore.Models.Module
{
    [DBTable]
    public class Utwor : Row
    {
        [DBItem]
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        [DBItem]
        public string Tytul { get; set; }
        [DBItem]
        public string Wykonawca { get; set; }
        [DBItem]
        public string Info { get; set; }
        [DBItem]
        public string Tekst { get; set; }
        [DBItem]
        public string YtLink { get; set; }

        public Utwor(Album album)
        {
            Album = album;
            AlbumId = album.Id;
            State = RowState.Added;
            Id = 0;
        }
        private Utwor() { }

        public static Utwor Empty = new Utwor();
    }
}