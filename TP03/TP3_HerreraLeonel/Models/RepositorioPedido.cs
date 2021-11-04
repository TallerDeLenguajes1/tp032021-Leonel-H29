using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Entities;


namespace TP3_HerreraLeonel.Models
{
    public class RepositorioPedido
    {
        private readonly string connectionString;
        //private readonly SQLiteConnection conexion;

        public RepositorioPedido(string connectionString){
            this.connectionString = connectionString;
            //this.conexion = new SQLiteConnection(connectionString);
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

            }
            catch (Exception ex)
            {
                ListadoDeCliente = new List<Cliente>();
                Console.WriteLine(ex.Message);

            }
            return ListadoDeCliente;
        }

        //Obtengo todos los datos de la tabla Cadetes en la DB
        public List<Pedido> getAllPedidos()
        {
            List<Pedido> ListadoDePedidos = new List<Pedido>();
            string SQLQuery = "SELECT * FROM Pedidos " +
                "INNER JOIN Clientes WHERE Pedidos.clienteId=Clientes.clienteID;";
            try
            {
                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    conexion.Open();
                    SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion);
                    using(SQLiteDataReader DataReader = command.ExecuteReader())
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

                            ListadoDePedidos.Add(pedido);
                        }
                    }
                    
                    conexion.Close();
                }
                
            }
            catch(Exception ex)
            {
                ListadoDePedidos = new List<Pedido>();
                Console.WriteLine(ex.Message);

            }
            return ListadoDePedidos;
        }

        //Inserto datos a la tabla de los Clientes
        public void InsertClientes(Cliente cliente)
        {
            string SQLQuery = "IF NOT EXISTS (SELECT * FROM Clientes WHERE clienteID="+cliente.Id+" OR clienteNombre="+cliente.Nombre+")" +
                "THEN INSERT INTO Clientes (clienteNombre, clienteDireccion, clienteTelefono)" +
                "VALUES (@nombre, @direccion, @telefono);";
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        //Inserto datos a la tabla
        public void InsertPedidos(Pedido pedido, int id_cadete)
        {
            try
            {
                InsertClientes(pedido.Cliente);
                string QueryClientes = "(SELECT clienteID FROM Clientes WHERE clienteID = @id_cli)";
                string QueryCadetes = "(SELECT cadeteID FROM Cadetes WHERE cadeteID = " + id_cadete + ")";

                string SQLQuery = "INSERT INTO Pedidos (pedidoObs, clienteId, cadeteId)" +
                "VALUES (@obs, " + QueryClientes + " , " + QueryCadetes +" );";

                using (SQLiteConnection conexion = new SQLiteConnection(connectionString))
                {
                    
                    using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conexion))
                    {
                        conexion.Open();
                        command.Parameters.AddWithValue("@obs", pedido.Observacion);
                        command.Parameters.AddWithValue("@id_cli", pedido.Cliente.Id);
                        command.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        
        //Obtengo todos los datos del  Cadete a modificar en la tabla de la DB
        public Pedido getPedidoAModificar(int id)
        {
            Pedido pedidoAModificar = new Pedido();
            string SQLQuery = "SELECT * FROM Pedidos INNER JOIN Clientes WHERE pedidoID=" + Convert.ToString(id)+";";
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
               
            }
            catch (Exception ex)
            {
                pedidoAModificar= new Pedido();
                Console.WriteLine(ex.Message);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        //Modifico datos a la tabla de pedidos
        public void UpdatePedidos(Pedido pedido , int id_cadete)
        {
            string SQLQuery = "UPDATE Pedidos SET cadeteId=@id_cad, pedidoObs=@obs," +
                "clienteId=@id_cli , pedidoEstado=@estado WHERE pedidoID=@id_ped";

            try
            {
                InsertClientes(pedido.Cliente);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        
        //Modifico datos a la tabla
        public void DeletePedido(int id)
        {
            string SQLQuery = "DELETE FROM Pedidos WHERE pedidoID=" + Convert.ToString(id) + ";"; 

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}