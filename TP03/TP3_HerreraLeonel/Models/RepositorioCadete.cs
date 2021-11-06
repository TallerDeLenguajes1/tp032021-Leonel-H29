using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Entities;
using NLog;

namespace TP3_HerreraLeonel.Models
{
    public interface IRepositorioCadete
    {
        void DeleteAllCadetes();
        void DeleteCadetes(int id);
        List<Cadete> getAll();
        Cadete getCadeteAModificar(int id);
        List<Pedido> getPedidos_delCadete(int id);
        void InsertCadetes(Cadete cadete);
        void UpdateCadetes(Cadete cadete);
    }

    public class SQLiteRepositorioCadete : IRepositorioCadete
    {
        private readonly string connectionString;
        private static ILogger _logger;
        //private readonly SQLiteConnection conexion;

        public SQLiteRepositorioCadete(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }

        //Obtengo todos los datos de la tabla Cadetes en la DB
        public List<Cadete> getAll()
        {
            List<Cadete> ListadoDeCadetes = new List<Cadete>();
            string SQLQuery = "SELECT * FROM Cadetes;";
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
                            Cadete cadete = new Cadete()
                            {
                                Id = Convert.ToInt32(DataReader["cadeteID"]),
                                Nombre = DataReader["cadeteNombre"].ToString(),
                                Direccion = DataReader["cadeteDireccion"].ToString(),
                                Telefono = DataReader["cadeteTelefono"].ToString()
                            };
                            cadete.ListadoPedidos = getPedidos_delCadete(cadete.Id);
                            ListadoDeCadetes.Add(cadete);
                        }
                    }

                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DE LOS CADETES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                ListadoDeCadetes = new List<Cadete>();
                _logger.Error("ERROR AL OBTENER LOS DATOS DE LOS CADETES: ", ex.Message);
            }
            return ListadoDeCadetes;
        }

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

        //Inserto datos a la tabla
        public void InsertCadetes(Cadete cadete)
        {
            string SQLQuery = "INSERT INTO Cadetes (cadeteNombre, cadeteDireccion, cadeteTelefono)" +
                "VALUES (@nombre, @direccion, @telefono);";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@nombre", cadete.Nombre);
                        command.Parameters.AddWithValue("@direccion", cadete.Direccion);
                        command.Parameters.AddWithValue("@telefono", cadete.Telefono);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE INSERTARON LOS DATOS DE LOS CADETES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE INSERTARON LOS DATOS DE LOS CADETES DE FORMA ERRONEA: ", ex.Message);
            }
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
                _logger.Info("SE ELIMINARON LOS DATOS DE LOS CADETES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE ELIMINARON LOS DATOS DE LOS CADETES DE FORMA ERRONEA", ex.Message);

            }
        }
    }
}