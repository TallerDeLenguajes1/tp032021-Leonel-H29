using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Entities;
using System.IO;
using System.Text.Json;
using NLog;

namespace TP3_HerreraLeonel.Models
{
    public interface ISQLiteRepositorioUsuario
    {
        void DeleteAllUsers();
        void DeleteUsuarios(int id);
        List<Usuario> getAll();
        Usuario getUsuarioAModificar(int id);
        void InsertUsuarios(Usuario user);
        bool IsResgisterUser(string user, string pass);
        Usuario LoginUser(string user);
        void UpdateUsuarios(Usuario usuario);
    }

    public class SQLiteRepositorioUsuario : ISQLiteRepositorioUsuario
    {
        private readonly string connectionString;
        private static ILogger _logger;
        //private readonly SQLiteConnection conexion;

        public SQLiteRepositorioUsuario(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        //Obtengo todos los datos de la tabla Usuarios en la DB
        public List<Usuario> getAll()
        {
            List<Usuario> ListadoDeUsuarios = new List<Usuario>();
            string SQLQuery = "SELECT * FROM Usuarios;";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        while (DataReader.Read())
                        {
                            Usuario usuario = new Usuario
                            {
                                Id_usuario = Convert.ToInt32(DataReader["IdUsuario"].ToString()),
                                Username = DataReader["Username"].ToString(),
                                Password = DataReader["Password"].ToString(),
                            };
                            ListadoDeUsuarios.Add(usuario);
                        }
                    }
                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DE LOS USUARIOS DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                ListadoDeUsuarios = new List<Usuario>();
                _logger.Error("ERROR AL OBTENER LOS DATOS DE LOS USUARIOS: ", ex.Message);
            }
            return ListadoDeUsuarios;
        }

        //Controlo si el usuario ya se encuentra registrado
        public bool IsResgisterUser(string user, string pass)
        {
            Usuario usuario = null;
            bool result = false;
            string SQLQuery = "SELECT * FROM Usuarios WHERE Username=@_user AND Password=@_pass;";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    command.Parameters.AddWithValue("@_user", user);
                    command.Parameters.AddWithValue("@_pass", pass);

                    command.ExecuteNonQuery();
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                usuario = new Usuario()
                                {
                                    Id_usuario = Convert.ToInt32(DataReader["IdUsuario"].ToString()),
                                    Username = DataReader["Username"].ToString(),
                                    Password = DataReader["Password"].ToString(),
                                };
                                result = true;
                            }
                        }
                    }
                    conexion.Close();
                }
                _logger.Info("EL USUARIO '{0}' SE HA LOGUEADO CORRECTAMENTE", user);
            }
            catch (Exception ex)
            {
                usuario = null;
                _logger.Error("EL USUARIO '{0}' SE HA LOGUEADO INCORRECTAMENTE: {1}", user, ex.Message);
            }
            return result;
        }


        public Usuario LoginUser(string user)
        {


            Usuario usuarioALog = new Usuario();
            string SQLQuery = "SELECT * FROM Usuarios WHERE Username=@_user;";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    command.Parameters.AddWithValue("@_user", user);
                    //command.Parameters.AddWithValue("@_pass", pass);

                    command.ExecuteNonQuery();
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                Usuario usuario = new Usuario()
                                {
                                    Id_usuario = Convert.ToInt32(DataReader["IdUsuario"].ToString()),
                                    Username = DataReader["Username"].ToString(),
                                    Password = DataReader["Password"].ToString(),
                                };
                                usuarioALog = usuario;
                            }
                        }
                    }
                    conexion.Close();
                }
                _logger.Info("EL USUARIO '{0}' SE HA LOGUEADO CORRECTAMENTE", user);
            }
            catch (Exception ex)
            {
                //usuarioALog = new Usuario();
                _logger.Error("EL USUARIO '{0}' SE HA LOGUEADO INCORRECTAMENTE: {1}", user, ex.Message);
            }
            return usuarioALog;
        }


        //Inserto datos a la tabla
        public void InsertUsuarios(Usuario user)
        {
            string SQLQuery = "INSERT INTO Usuarios (Username, Password)" +
                "VALUES (@user, @pass);";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@user", user.Username);
                        command.Parameters.AddWithValue("@pass", user.Password);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE INSERTARON LOS DATOS DE LOS USUARIOS DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE INSERTARON LOS DATOS DE LOS USUARIOS DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Obtengo todos los datos del Usuario a modificar en la tabla de la DB
        public Usuario getUsuarioAModificar(int id)
        {
            Usuario usuario = null;
            string SQLQuery = "SELECT * FROM Usuarios WHERE IdUsuario=@_id;";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    command.Parameters.AddWithValue("@_id", id);
                    command.ExecuteNonQuery();
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                usuario = new Usuario()
                                {
                                    Id_usuario = Convert.ToInt32(DataReader["IdUsuario"].ToString()),
                                    Username = DataReader["Username"].ToString(),
                                    Password = DataReader["Password"].ToString(),
                                };

                            }
                        }
                    }
                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DEL USUARIO " + id + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                usuario = new Usuario();
                _logger.Error("SE OBTUVO LOS DATOS DEL USUARIO " + id + " DE FORMA ERRONEA:", ex.Message);

            }
            return usuario;
        }

        //Modifico datos a la tabla
        public void UpdateUsuarios(Usuario usuario)
        {
            string SQLQuery = "UPDATE Usuarios SET Username=@nombre, Password=@pass" +
                " WHERE idUsuario=@id;";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id", usuario.Id_usuario);
                        command.Parameters.AddWithValue("@nombre", usuario.Username);
                        command.Parameters.AddWithValue("@pass", usuario.Password);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE MODIFICARON LOS DATOS DEL USUARIO " + usuario.Username + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE MODIFICARON LOS DATOS DEL USUARIO " + usuario.Username + " DE FORMA ERRONEA", ex.Message);
            }
        }

        //Modifico datos a la tabla
        public void DeleteUsuarios(int id)
        {
            string SQLQuery = "DELETE FROM Usuarios WHERE IdUsuario=@_id AND Username NOT LIKE 'admin';";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@_id", id);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE ELIMINARON LOS DATOS DEL USUARIO " + id + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE ELIMINARON LOS DATOS DEL USUARIO " + id + " DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Modifico datos a la tabla
        public void DeleteAllUsers()
        {
            string SQLQuery = "DELETE FROM Usuarios WHERE Username NOT LIKE 'admin'";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE ELIMINARON LOS DATOS DE LOS USUARIOS DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE ELIMINARON LOS DATOS DE LOS USUARIOS DE FORMA ERRONEA", ex.Message);
            }
        }
    }

    public class JSONRepositorioUsuario : ISQLiteRepositorioUsuario
    {
        public void DeleteAllUsers()
        {
            throw new NotImplementedException();
        }

        public void DeleteUsuarios(int id)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> getAll()
        {
            throw new NotImplementedException();
        }

        public Usuario getUsuarioAModificar(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertUsuarios(Usuario user)
        {
            throw new NotImplementedException();
        }

        public bool IsResgisterUser(string user, string pass)
        {
            throw new NotImplementedException();
        }

        public Usuario LoginUser(string user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUsuarios(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}