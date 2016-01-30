using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;

namespace MusicStore.Models.Module
{
    public class DbModule : IDisposable
    {
        IDbConnection Connection;
        private DbModule()
        {
            var path = HttpContext.Current.Request.PhysicalApplicationPath;
            path = Path.Combine(path, @"App_Data\DataBase.mdf");
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + @";Integrated Security=True";
            Connection = new SqlConnection(connString);
        }

        private static DbModule instance = new DbModule();
        public static DbModule GetInstance() => instance;

        public List<User> Users
        {
            get
            {
                List<User> users = new List<User>();

                try
                {
                    Connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Users", (SqlConnection)Connection);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        User user = new User();
                        user.State = RowState.Unchanged;
                        foreach (PropertyInfo prop in typeof(User).GetProperties().Where(x => x.GetCustomAttribute(typeof(DBItemAttribute)) != null))
                        {
                            prop.SetValue(user, reader[prop.Name]);
                        }
                        users.Add(user);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Connection.Close();
                }

                return users;
            }
        }
        public List<Utwor> Utwory
        {
            get
            {
                List<Utwor> utwory = new List<Utwor>();

                try
                {
                    Connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Utwor", (SqlConnection)Connection);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Utwor utwor = new Utwor();
                        utwor.State = RowState.Unchanged;
                        foreach (PropertyInfo prop in typeof(Utwor).GetProperties().Where(x => x.GetCustomAttribute(typeof(DBItemAttribute)) != null))
                        {
                            prop.SetValue(utwor, reader[prop.Name]);
                        }
                        utwory.Add(utwor);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Connection.Close();
                }

                return utwory;
            }
        }

        public List<Artysta> Artysci
        {
            get
            {
                List<Artysta> artists = new List<Artysta>();

                try
                {
                    Connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Artysta", (SqlConnection)Connection);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Artysta art = new Artysta();
                        art.State = RowState.Unchanged;
                        foreach (PropertyInfo prop in typeof(Artysta).GetProperties().Where(x => x.GetCustomAttribute(typeof(DBItemAttribute)) != null))
                        {
                            prop.SetValue(art, reader[prop.Name]);
                        }
                        artists.Add(art);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Connection.Close();
                }

                return artists;
            }
        }

        public List<Album> Albumy
        {
            get
            {
                List<Album> albums = new List<Album>();

                try
                {
                    Connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Album", (SqlConnection)Connection);

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Album album = new Album();
                        album.State = RowState.Unchanged;
                        foreach (PropertyInfo prop in typeof(Album).GetProperties().Where(x => x.GetCustomAttribute(typeof(DBItemAttribute)) != null))
                        {
                            if (reader[prop.Name] != null && reader[prop.Name].GetType() != typeof(DBNull))
                                prop.SetValue(album, reader[prop.Name]);
                        }
                        albums.Add(album);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    Connection.Close();
                }

                return albums;
            }
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }

        public bool AddRow<T>(T obiekt) where T : Row
        {
            bool result = true;
            Type typ = typeof(T);
            var attrib = typ.GetCustomAttributes(typeof(DBTableAttribute), true).Any();
            if (!attrib) throw new InvalidOperationException("For this method you can use only classes with DBTable attribute");

            try
            {
                Connection.Open();
                string query = $"INSERT INTO {typ.Name} ";
                string columns = "(";
                string values = "(";
                var dict = new Dictionary<string, object>();

                foreach(PropertyInfo prop in typ.GetProperties().Where(x => x.GetCustomAttribute(typeof(DBItemAttribute)) != null))
                {
                    if (prop.Name != "Id")
                    {
                        if (prop.GetValue(obiekt) != null)
                        {
                            columns += $"{prop.Name},";
                            values += $"@{prop.Name},";
                            dict.Add(prop.Name, prop.GetValue(obiekt));
                        }
                    }
                }
                columns = columns.Remove(columns.Length - 1, 1) + ") OUTPUT INSERTED.Id ";
                values = values.Remove(values.Length - 1, 1) + ")";

                query = query + columns + " VALUES " + values + ";";

                SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);
                foreach(var obj in dict)
                {
                    command.Parameters.AddWithValue("@" + obj.Key, obj.Value);
                }
                command.Parameters.Add("@ID", SqlDbType.Int, 0).Direction = ParameterDirection.Output;

                if (Connection.State != ConnectionState.Open) Connection.Open();
                obiekt.Id = (int)command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                result = false;
                return result;
            }
            finally
            {
                Connection.Close();
            }

            return result;
        }

        public bool Update<T>(T obiekt) where T : class
        {
            bool result = true;
            Type typ = typeof(T);
            var attrib = typ.GetCustomAttributes(typeof(DBTableAttribute), true).Any();
            if (!attrib) throw new InvalidOperationException("For this method you can use only classes with DBTable attribute");

            try
            {
                Connection.Open();
                string query = $"UPDATE {typ.Name} ";
                string values = "SET ";
                int ID = 0;
                var dict = new Dictionary<string, object>();

                foreach (PropertyInfo prop in typ.GetProperties().Where(x => x.GetCustomAttribute(typeof(DBItemAttribute)) != null))
                {
                    if (prop.Name != "Id")
                    {
                        if (prop.GetValue(obiekt) != null)
                        {
                            values += $"{prop.Name}=@{prop.Name},";
                            dict.Add(prop.Name, prop.GetValue(obiekt));
                        }
                    }
                    else
                    {
                        ID = (int)prop.GetValue(obiekt);
                    }
                }
                values = values.Remove(values.Length - 1, 1) + $" WHERE Id={ID}";

                query = query + values;

                SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);
                foreach (var obj in dict)
                {
                    command.Parameters.AddWithValue("@" + obj.Key, obj.Value);
                }

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                return result;
            }
            finally
            {
                Connection.Close();
            }

            return result;
        }

        public bool Delete<T>(T obiekt) where T : class
        {
            bool result = true;
            Type typ = typeof(T);
            var attrib = typ.GetCustomAttributes(typeof(DBTableAttribute), true).Any();
            if (!attrib) throw new InvalidOperationException("For this method you can use only classes with DBTable attribute");

            try
            {
                Connection.Open();
                string query = $"DELETE FROM {typ.Name} WHERE Id = {typ.GetProperty("Id").GetValue(obiekt)}";
                SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                return result;
            }
            finally
            {
                Connection.Close();
            }

            return result;
        }
    }

}