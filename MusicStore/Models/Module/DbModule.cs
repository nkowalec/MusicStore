using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    public class DbModule
    {
        private DbModule()
        {

        }

        private static DbModule instance = new DbModule();
        public static DbModule GetInstance() => instance;

        private DataTable utwory = new DataTable();

        public DataTable Utwory
        {
            get { return utwory; }
        }

    }

}