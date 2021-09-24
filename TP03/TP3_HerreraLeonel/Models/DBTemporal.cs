using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace TP3_HerreraLeonel.Entities
{
    public class DBTemporal
    {
        public Cadeteria Cadeteria { get; set; }

        public DBTemporal()
        {
            Cadeteria = new Cadeteria();
        }

        public static List<Cadete> leerArchivoCadetes()
        {
            List<Cadete> listaCadetes;
            string rutaArchivo = @"Cadetes.json";

            try
            {
                using (StreamReader leerJason = File.OpenText(rutaArchivo))
                {
                    var Json = leerJason.ReadToEnd();
                    listaCadetes = JsonSerializer.Deserialize<List<Cadete>>(Json);
                }
            }
            catch (FileNotFoundException)
            {
                listaCadetes = new List<Cadete>();
            }

            return listaCadetes;
        }

        public static List<Cadete> guardarCadete(Cadete cadete)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            listaCadetes.Add(cadete);

            FileStream archiboCadetes = new FileStream("Cadetes.json", FileMode.Create);
            StreamWriter escribirCadete = new StreamWriter(archiboCadetes);

            string strJson = JsonSerializer.Serialize(listaCadetes);
            escribirCadete.WriteLine("{0}", strJson);

            escribirCadete.Close();

            return listaCadetes;
        }

        public static List<Cadete> borrarCadete(int id)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            listaCadetes.RemoveAt(id);

            FileStream archiboCadetes = new FileStream("Cadetes.json", FileMode.Create);
            StreamWriter escribirCadete = new StreamWriter(archiboCadetes);

            string strJson = JsonSerializer.Serialize(listaCadetes);
            escribirCadete.WriteLine("{0}", strJson);

            escribirCadete.Close();

            return listaCadetes;
        }

    }

}
