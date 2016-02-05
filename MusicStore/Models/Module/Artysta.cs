using System.Collections.Generic;
using System.Linq;

namespace MusicStore.Models.Module
{
    [DBTable]
    public class Artysta : Row
    {
        [DBItem]
        public string Nazwa { get; set; } = "";
        [DBItem]
        public string Opis { get; set; } = "";

        private List<Album> albumy = null;
        public List<Album> Albumy
        {
            get
            {
                if (Id != 0)
                {
                    if (albumy == null) albumy = DbModule.GetInstance().Albumy.Where(x => x.ArtystaId == Id).ToList();
                }
                else
                {
                    albumy = new List<Album>();
                }
                return albumy;
            }
        }
        public Artysta()
        {
            State = RowState.Unchanged;
            Id = 0;
        }

        public override string ToString()
        {
            return this.Nazwa;
        }
    }
}