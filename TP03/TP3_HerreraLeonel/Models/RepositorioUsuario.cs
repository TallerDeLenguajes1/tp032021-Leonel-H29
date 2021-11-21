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
        List<Usuario> getAll();
        void InsertUsuarios(Usuario user);
        bool IsResgisterUser(string user, string pass);
        Usuario LoginUser(string user);
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
                                Username = DataReader["Username"].ToString(),
                                Password = DataReader["Password"].ToString(),
                            };

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
            bool result = false;
            Usuario usuarioALog = new Usuario();
            string SQLQuery = "SELECT * FROM Usuarios WHERE Username=@_user AND Password=@_pass;";
            //string SQLQuery = "SELECT * FROM Usuarios WHERE Username='"+user+"' AND Password='"+pass+"';";

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
                                Usuario usuario = new Usuario()
                                {
                                    Username = DataReader["Username"].ToString(),
                                    Password = DataReader["Password"].ToString(),
                                };
                                usuarioALog = usuario;
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
                //usuarioALog = new Usuario();
                _logger.Error("EL USUARIO '{0}' SE HA LOGUEADO INCORRECTAMENTE: {1}", user, ex.Message);
            }
            return result;
        }


        public Usuario LoginUser(string user) {
            

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
        /*
        //Obtengo todos los datos de la tabla Cadetes en la DB
        public List<Pedido> getPedidos_delCadete(int id)
        {
            List<Pedido> ListadoDePedidos = new List<Pedido>();
            string SQLQuery = "SELECT * FROM Pedidos INNER JOIN Cadetes " +
            "ON Pedidos.cadeteId=Cadetes.cadeteID" +
            " INNER JOIN Clientes ON Pedidos.clienteId=Clientes.clienteID" +
            " WHERE Cadetes.cadeteID=" + id + "; ";
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
                            Pedido pedido = new Pedido();
                            pedido.Nro = Convert.ToInt32(DataReader["pedidoID"]);
                            pedido.Observacion = DataReader["pedidoObs"].ToString();
                            pedido.Cliente.Id = Convert.ToInt32(DataReader["clienteId"]);
                            pedido.Cliente.Nombre = DataReader["clienteNombre"].ToString();
                            pedido.Cliente.Direccion = DataReader["clienteDireccion"].ToString();
                            pedido.Cliente.Telefono = DataReader["clienteTelefono"].ToString();
                            pedido.Estado = (Pedido.Estados)Enum.Parse(typeof(Pedido.Estados), DataReader["pedidoEstado"].ToString());
                            ListadoDePedidos.Add(pedido);
                        }
                    }

                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DE PEDIDOS LOS CADETES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                ListadoDePedidos = new List<Pedido>();
                _logger.Error("ERROR AL OBTENER LOS DATOS DE LOS CADETES: ", ex.Message);
            }
            return ListadoDePedidos;
        }

        

        //Obtengo todos los datos del  Cadete a modificar en la tabla de la DB
        public Cadete getCadeteAModificar(int id)
        {
            Cadete cadeteAModificar = new Cadete();
            string SQLQuery = "SELECT * FROM Cadetes WHERE cadeteID=" + Convert.ToString(id) + ";";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        if (DataReader.HasRows)
                        {
                            while (DataReader.Read())
                            {
                                Cadete cadete = new Cadete()
                                {
                                    Id = Convert.ToInt32(DataReader["cadeteID"]),
                                    Nombre = DataReader["cadeteNombre"].ToString(),
                                    Direccion = DataReader["cadeteDireccion"].ToString(),
                                    Telefono = DataReader["cadeteTelefono"].ToString()
                                };
                                cadeteAModificar = cadete;
                            }
                        }
                    }
                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DEL CADETE " + id + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                cadeteAModificar = new Cadete();
                _logger.Error("SE OBTUVO LOS DATOS DEL CADETE " + id + " DE FORMA ERRONEA:", ex.Message);

            }
            return cadeteAModificar;
        }

        //Modifico datos a la tabla
        public void UpdateCadetes(Cadete cadete)
        {
            string SQLQuery = "UPDATE Cadetes SET cadeteNombre=@nombre, cadeteDireccion=@direccion," +
                "cadeteTelefono=@telefono WHERE cadeteID=@id_cad";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id_cad", cadete.Id);
                        command.Parameters.AddWithValue("@nombre", cadete.Nombre);
                        command.Parameters.AddWithValue("@direccion", cadete.Direccion);
                        command.Parameters.AddWithValue("@telefono", cadete.Telefono);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE MODIFICARON LOS DATOS DEL CADETE " + cadete.Id + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE MODIFICARON LOS DATOS DEL CADETE " + cadete.Id + " DE FORMA ERRONEA", ex.Message);
            }
        }

        //Modifico datos a la tabla
        public void DeleteCadetes(int id)
        {
            string SQLQuery = "DELETE FROM Cadetes WHERE cadeteID=@id_cad;";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id_cad", id.ToString());
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE ELIMINARON LOS DATOS DEL CADETE " + id + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE ELIMINARON LOS DATOS DEL CADETE " + id + " DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Modifico datos a la tabla
        public void DeleteAllCadetes()
        {
            string SQLQuery = "DELETE FROM Cadetes";

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
               // _logger.Info("SE ELIMINARON LOS DATOS DE LOS CADETES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                //_logger.Error("SE ELIMINARON LOS DATOS DE LOS CADETES DE FORMA ERRONEA", ex.Message);

            }
        }*/
    }
}