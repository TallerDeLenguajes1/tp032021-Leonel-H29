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
    }
}