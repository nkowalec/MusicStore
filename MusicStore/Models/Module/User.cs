using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Module
{
    [DBTable("Users")]
    public class User : Row
    {
        public static string Klient = "Klient";
        public static string Admin = "Admin";

        [DBItem]
        public string Login { get; set; } = "";
        [DBItem]
        public string Password { get; set; } = "";
        [DBItem]
        public string Rola { get; set; } = "Klient";

        public static string TryLogin(string _user, string _pass)
        {
            var user = DbModule.GetInstance().Users.Where(x => x.Login == _user).ToList();
            if (user.Count > 0)
            {
                if (user.First().Password == _pass)
                {
                    return user.First().Rola;
                }
            }
            return "";
        }

        public static bool IsAuthenticated
        {
            get
            {
                var session = HttpContext.Current.Session;
                if (session["LoginRole"] != null && session["LoginRole"].ToString() != "")
                    return true;
                return false;
            }
        }

        public static string GetCurrentLogin()
        {
            var session = HttpContext.Current.Session;
            return session["Login"]?.ToString();
        }

        public static string GetCurrentRole()
        {
            var session = HttpContext.Current.Session;
            return session["LoginRole"]?.ToString();
        }

        public static int GetCurrentId()
        {
            var session = HttpContext.Current.Session;
            return (int)session["LoginId"];
        }
    }
}