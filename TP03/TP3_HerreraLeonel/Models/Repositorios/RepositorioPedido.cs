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
    public interface IRepositorioPedido
    {
        void DeleteAllPedidos();
        void DeletePedido(int id);
        List<Cliente> getAllClientes();
        List<Pedido> getAllPedidos();
        Pedido getPedidoAModificar(int id);
        Cliente InsertClientes(Cliente cliente);
        void InsertPedidos(Pedido pedido, int id_cadete);
        void UpdateCliente(Cliente cliente);
        void UpdatePedidos(Pedido pedido, int id_cadete);
        public Cadete get_Only_Pedido_delCadete(Pedido pedido);
    }

    public class SQLiteRepositorioPedido : IRepositorioPedido
    {
        private readonly string connectionString;
        private static ILogger _logger;

        public SQLiteRepositorioPedido(string connectionString, ILogger logger)
        {
            this.connectionString = connectionString;
            _logger = logger;
        }


        public List<Cliente> getAllClientes()
        {
            List<Cliente> ListadoDeCliente = new List<Cliente>();
            string SQLQuery = "SELECT * FROM  Clientes;";
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
                            Cliente cliente = new Cliente();

                            cliente.Id = Convert.ToInt32(DataReader["clienteId"]);
                            cliente.Nombre = DataReader["clienteNombre"].ToString();
                            cliente.Direccion = DataReader["clienteDireccion"].ToString();
                            cliente.Telefono = DataReader["clienteTelefono"].ToString();
                            ListadoDeCliente.Add(cliente);
                        }
                    }

                    conexion.Close();
                }
                _logger.Info("SE OBTUVIERON LOS DATOS DE LOS CLIENTES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                ListadoDeCliente = new List<Cliente>();
                _logger.Error("SE OBTUVIERON LOS DATOS DE LOS CLIENTES DE FORMA ERRONEA: ", ex.Message);
            }
            return ListadoDeCliente;
        }

        //Obtengo todos los datos de la tabla Cadetes en la DB
        public List<Pedido> getAllPedidos()
        {
            List<Pedido> ListadoDePedidos = new List<Pedido>();
            string SQLQuery = "SELECT * FROM Pedidos " +
                "INNER JOIN Clientes ON Pedidos.clienteId=Clientes.clienteID;";
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
                _logger.Info("SE OBTUVIERON LOS DATOS DE LOS PEDIDOS DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                ListadoDePedidos = new List<Pedido>();
                _logger.Error("SE OBTUVIERON LOS DATOS DE LOS CLIENTES DE FORMA ERRONEA: ", ex.Message);
            }
            return ListadoDePedidos;
        }

        //Inserto datos a la tabla de los Clientes
        public Cliente InsertClientes(Cliente cliente)
        {
            List<Cliente> ListCliente = getAllClientes();
            try
            {

                if (ListCliente.Find(x => x.Nombre == cliente.Nombre && x.Direccion == cliente.Direccion && x.Telefono == cliente.Telefono) == null)
                {
                    string SQLQuery = "INSERT INTO Clientes (clienteNombre, clienteDireccion, clienteTelefono)" +
                    "VALUES (@nombre, @direccion, @telefono);";

                    using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                    {

                        using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                        {
                            conexion.Open();
                            command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                            command.Parameters.AddWithValue("@direccion", cliente.Direccion);
                            command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                            command.ExecuteNonQuery();
                            conexion.Close();
                        }
                    }
                    _logger.Info("SE INSERTARON LOS DATOS DEL CLIENTE DE FORMA EXITOSA");
                    cliente.Id = GetNewIDCliente(cliente);
                }
                else
                {
                    cliente = ListCliente.Where(x => x.Nombre == cliente.Nombre && x.Direccion == cliente.Direccion && x.Telefono == cliente.Telefono).Single();
                    _logger.Info("EL CLIENTE " + cliente.Id + " YA SE ENCUENTRA REGISTRADO");

                }
            }
            catch (Exception ex)
            {
                cliente = new Cliente();
                _logger.Error("SE INSERTARON LOS DATOS DE LOS CLIENTES DE FORMA ERRONEA: ", ex.Message);
            }
            return cliente;
        }

        //Obtengo el ID del nuevo cliente creado
        public int GetNewIDCliente(Cliente cliente)
        {
            List<Cliente> ListCliente = getAllClientes();
            cliente = ListCliente.Where(x => x.Nombre == cliente.Nombre && x.Direccion == cliente.Direccion && x.Telefono == cliente.Telefono).Single();
            return cliente.Id;
        }

        //Inserto datos a la tabla
        public void InsertPedidos(Pedido pedido, int id_cadete)
        {
            try
            {
                pedido.Cliente = InsertClientes(pedido.Cliente);
                string QueryClientes = "(SELECT clienteID FROM Clientes WHERE clienteID = @id_cliente)";
                string QueryCadetes = "(SELECT cadeteID FROM Cadetes WHERE cadeteID = @id_cadete)";

                string SQLQuery = "INSERT INTO Pedidos (pedidoObs, clienteId, cadeteId, pedidoEstado)" +
                "VALUES (@obs, " + QueryClientes + " , " + QueryCadetes + " , @estado);";

                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id_cliente", pedido.Cliente.Id.ToString());
                        command.Parameters.AddWithValue("@id_cadete", id_cadete.ToString());
                        command.Parameters.AddWithValue("@obs", pedido.Observacion);
                        command.Parameters.AddWithValue("@estado", pedido.Estado);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE INSERTARON LOS DATOS DE PEDIDO DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE INSERTARON LOS DATOS DEL PEDIDO DE FORMA ERRONEA", ex.Message);
            }
        }

        //Obtengo todos los datos del  Cadete a modificar en la tabla de la DB
        public Pedido getPedidoAModificar(int id)
        {
            Pedido pedidoAModificar = new Pedido();
            //string SQLQuery = "SELECT * FROM Pedidos INNER JOIN Clientes ON Clientes.clienteID=Pedidos.clienteId WHERE pedidoID=" + Convert.ToString(id) + ";";
            string SQLQuery = "SELECT * FROM Pedidos INNER JOIN Clientes ON Clientes.clienteID=Pedidos.clienteId WHERE pedidoID=@id_ped;";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    command.Parameters.AddWithValue("@id_ped", id);
                    command.ExecuteNonQuery();
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        if (DataReader.HasRows)
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

                                pedidoAModificar = pedido;
                            }
                        }
                    }
                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DEL  PEDIDO " + id + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                pedidoAModificar = new Pedido();
                _logger.Error("SE OBTUVO LOS DATOS DEL  PEDIDO " + id + " DE FORMA ERRONEA", ex.Message);
            }
            return pedidoAModificar;
        }

        //Modifico datos a la tabla del cliente
        public void UpdateCliente(Cliente cliente)
        {
            string SQLQuery = "UPDATE Clientes SET clienteNombre=@nombre, clienteDireccion=@direccion," +
                "clienteTelefono=@telefono WHERE clienteID = @id_cli";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id_cli", cliente.Id);
                        command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@direccion", cliente.Direccion);
                        command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE MODIFICARON LOS DATOS DEL CLIENTE " + cliente.Id + " DE FORMA EXITOSA");

            }
            catch (Exception ex)
            {
                _logger.Error("SE INSERTARON LOS DATOS DEL CLIENTE " + cliente.Id + " DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Modifico datos a la tabla de pedidos
        public void UpdatePedidos(Pedido pedido, int id_cadete)
        {
            string SQLQuery = "UPDATE Pedidos SET cadeteId=@id_cad, pedidoObs=@obs," +
                "clienteId=@id_cli , pedidoEstado=@estado WHERE pedidoID=@id_ped";

            try
            {
                pedido.Cliente = InsertClientes(pedido.Cliente);
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id_ped", pedido.Nro);
                        command.Parameters.AddWithValue("@id_cad", id_cadete);
                        command.Parameters.AddWithValue("@obs", pedido.Observacion);
                        command.Parameters.AddWithValue("@id_cli", pedido.Cliente.Id);
                        command.Parameters.AddWithValue("@estado", pedido.Estado);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                UpdateCliente(pedido.Cliente);
                _logger.Info("SE MODIFICARON LOS DATOS DEL PEDIDO " + pedido.Nro + " DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                _logger.Error("SE MODIFICARON LOS DATOS DEL PEDIDO " + pedido.Nro + " DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Modifico datos a la tabla
        public void DeletePedido(int id)
        {
            string SQLQuery = "DELETE FROM Pedidos WHERE pedidoID=@id_ped;";

            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {

                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@id_ped", id.ToString());
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
                _logger.Info("SE ELIMINARON LOS DATOS DEL PEDIDO " + id + " DE FORMA EXITOSA");

            }
            catch (Exception ex)
            {
                _logger.Error("SE ELIMINARON LOS DATOS DEL PEDIDO" + id + " DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Modifico datos a la tabla
        public void DeleteAllPedidos()
        {
            string SQLQuery = "DELETE FROM Pedidos";

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
                _logger.Info("SE ELIMINARON LOS DATOS DE LOS PEDIDOS DE FORMA EXITOSA");

            }
            catch (Exception ex)
            {
                _logger.Error("SE ELIMINARON LOS DATOS DE LOS PEDIDOS DE FORMA ERRONEA: ", ex.Message);
            }
        }

        //Obtengo todos los datos de un pedido del cadete en la BD
        public Cadete get_Only_Pedido_delCadete(Pedido pedido)
        {
            Cadete cadeteAdevolver = new Cadete();

            string SQLQuery = "SELECT * FROM Pedidos INNER JOIN Cadetes " +
            "ON Pedidos.cadeteId=Cadetes.cadeteID" +
            " WHERE pedidoID=@id_ped; ";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    command.Parameters.AddWithValue("@id_ped", pedido.Nro);
                    command.ExecuteNonQuery();
                    using (SQLiteDataReader DataReader = command.ExecuteReader())
                    {
                        while (DataReader.Read())
                        {
                            Cadete _cadete = new Cadete()
                            {
                                Id = Convert.ToInt32(DataReader["cadeteID"]),
                                Nombre = DataReader["cadeteNombre"].ToString(),
                                Direccion = DataReader["cadeteDireccion"].ToString(),
                                Telefono = DataReader["cadeteTelefono"].ToString()
                            };
                            cadeteAdevolver = _cadete;
                        }
                        DataReader.Close();
                    }

                    conexion.Close();
                }
                _logger.Info("SE OBTUVO LOS DATOS DE PEDIDOS LOS CADETES DE FORMA EXITOSA");
            }
            catch (Exception ex)
            {
                cadeteAdevolver = new Cadete();
                _logger.Error("ERROR AL OBTENER LOS DATOS DEL CADETE: ", ex.Message);
            }
            return cadeteAdevolver;
        }
    }

    public class JSONRepositorioPedido : IRepositorioPedido
    {
        private static ILogger _logger;
        //static string rutaArchivoCadetes = @"Cadetes.json";
        static string rutaArchivoPedidos = @"Pedidos.json";

        public JSONRepositorioPedido(ILogger logger) {
            _logger = logger;
        }
        public void DeleteAllPedidos()
        {
            try
            {
                //Elimino todos los pedidos del archivo los pedidos
                using (FileStream archivoPedidos = new FileStream(rutaArchivoPedidos, FileMode.Create))
                {
                    using (StreamWriter escribirPedido = new StreamWriter(archivoPedidos))
                    {
                        escribirPedido.WriteLine("[]");
                        escribirPedido.Close();
                        escribirPedido.Dispose();
                    }
                }
                string mensaje = "TODOS LOS DATOS DE LOS PEDIDOS SE ELIMINARON CORRECTAMENTE DEL ARCHIVO";
                _logger.Info(mensaje);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                string mensaje = "TODOS LOS DATOS DE LOS PEDIDOS SE ELIMINARON CORRECTAMENTE DEL ARCHIVO: "+ex.Message ;
                _logger.Error(mensaje);
            }
        }

        public void DeletePedido(int id)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> getAllClientes()
        {
            throw new NotImplementedException();
        }

        public List<Pedido> getAllPedidos()
        {
            throw new NotImplementedException();
        }

        public Pedido getPedidoAModificar(int id)
        {
            throw new NotImplementedException();
        }

        public Cliente InsertClientes(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void InsertPedidos(Pedido pedido, int id_cadete)
        {
            throw new NotImplementedException();
        }

        public void UpdateCliente(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void UpdatePedidos(Pedido pedido, int id_cadete)
        {
            throw new NotImplementedException();
        }
        public Cadete get_Only_Pedido_delCadete(Pedido pedido) {
            throw new NotImplementedException();
        }
    }
}