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


        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< CADETES
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

        public static List<Cadete> ModificarCadete(int i, Cadete cadete)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            //listaPedidos.Add(pedido);
            listaCadetes[i] = cadete;

            FileStream archivoPedidos = new FileStream("Cadetes.json", FileMode.Create);
            StreamWriter escribirPedido = new StreamWriter(archivoPedidos);

            string strJson = JsonSerializer.Serialize(listaCadetes);
            escribirPedido.WriteLine("{0}", strJson);

            escribirPedido.Close();

            return listaCadetes;
        }

        public static List<Cadete> guardarCadete(Cadete cadete)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            listaCadetes.Add(cadete);

            FileStream archivoCadetes = new FileStream("Cadetes.json", FileMode.Create);
            StreamWriter escribirCadete = new StreamWriter(archivoCadetes);

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

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< PEDIDOS

        public static List<Pedido> leerArchivoPedidos()
        {
            List<Pedido> listaPedidos;
            string rutaArchivo = @"Pedidos.json";

            try
            {
                using (StreamReader leerJason = File.OpenText(rutaArchivo))
                {
                    var Json = leerJason.ReadToEnd();
                    listaPedidos = JsonSerializer.Deserialize<List<Pedido>>(Json);
                }
            }
            catch (FileNotFoundException)
            {
                listaPedidos = new List<Pedido>();
            }

            return listaPedidos;
        }

        public static List<Pedido> ModificarPedido(int i, Pedido pedido)
        {
            List<Pedido> listaPedidos = leerArchivoPedidos();

            //listaPedidos.Add(pedido);
            listaPedidos[i] = pedido;

            FileStream archivoPedidos = new FileStream("Pedidos.json", FileMode.Create);
            StreamWriter escribirPedido = new StreamWriter(archivoPedidos);

            string strJson = JsonSerializer.Serialize(listaPedidos);
            escribirPedido.WriteLine("{0}", strJson);

            escribirPedido.Close();

            return listaPedidos;
        }

        public static List<Pedido> guardarPedido(Pedido pedido)
        {
            List<Pedido> listaPedidos = leerArchivoPedidos();

            listaPedidos.Add(pedido);

            FileStream archivoPedidos = new FileStream("Pedidos.json", FileMode.Create);
            StreamWriter escribirPedido = new StreamWriter(archivoPedidos);

            string strJson = JsonSerializer.Serialize(listaPedidos);
            escribirPedido.WriteLine("{0}", strJson);

            escribirPedido.Close();

            return listaPedidos;
        }

        public static List<Pedido> borrarPedido(int id)
        {
            List<Pedido> listaPedidos = leerArchivoPedidos();

            listaPedidos.RemoveAt(id);

            FileStream archivoPedidos = new FileStream("Pedidos.json", FileMode.Create);
            StreamWriter escribirPedido = new StreamWriter(archivoPedidos);

            string strJson = JsonSerializer.Serialize(listaPedidos);
            escribirPedido.WriteLine("{0}", strJson);

            escribirPedido.Close();

            return listaPedidos;
        }


    }

}
