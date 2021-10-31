using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TP3_HerreraLeonel.Entities;


namespace TP3_HerreraLeonel.Models
{
    public class RepositorioCadete
    {
        private readonly string connectionString;
        //private readonly SQLiteConnection conexion;

        public RepositorioCadete(string connectionString){
            this.connectionString = connectionString;
            //this.conexion = new SQLiteConnection(connectionString);
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
                    using(SQLiteDataReader DataReader = command.ExecuteReader())
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
                            ListadoDeCadetes.Add(cadete);
                        }
                    }
                    
                    conexion.Close();
                }
                Console.WriteLine(ListadoDeCadetes);
            }
            catch(Exception ex)
            {
                ListadoDeCadetes = new List<Cadete>();
                Console.WriteLine(ex.Message);

            }
            return ListadoDeCadetes;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        //Obtengo todos los datos del  Cadete a modificar en la tabla de la DB
        public Cadete getCadeteAModificar(int id)
        {
            Cadete cadeteAModificar = new Cadete();
            string SQLQuery = "SELECT * FROM Cadetes WHERE cadeteID="+Convert.ToString(id)+";";
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
               
            }
            catch (Exception ex)
            {
                cadeteAModificar = new Cadete();
                Console.WriteLine(ex.Message);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        //Modifico datos a la tabla
        public void DeleteCadetes(int id)
        {
            string SQLQuery = "DELETE FROM Cadetes WHERE cadeteID=" + Convert.ToString(id) + ";"; 

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}