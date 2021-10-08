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


        static string rutaArchivo = @"Cadetes.json";
        static string rutaArchivoPedidos = @"Pedidos.json";
        public DBTemporal()
        {
            Cadeteria = new Cadeteria();
        }


        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< CADETES
        public static List<Cadete> leerArchivoCadetes()
        {
            List<Cadete> listaCadetes;
          //  string rutaArchivo = @"Cadetes.json";

            try
            {
                using (StreamReader leerJason = File.OpenText(rutaArchivo))
                {
                    var Json = leerJason.ReadToEnd();
                    if(Json != "")
                        listaCadetes = JsonSerializer.Deserialize<List<Cadete>>(Json);
                    else
                    {
                        listaCadetes = new List<Cadete>();
                    }
                    leerJason.Close();
                    leerJason.Dispose();
                }
            }
            catch (FileNotFoundException)
            {
                listaCadetes = new List<Cadete>();
            }

            return listaCadetes;
        }

        public static Cadete VerCadete(int id)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();
            Cadete cadeteSeleccionado = listaCadetes.Where(cad => cad.Id == id).Single();
            if (cadeteSeleccionado == null)
            {
                Console.WriteLine("CADETE NO ENCONTRADO");
            }
            return cadeteSeleccionado;
        }

        public static void ModificarCadete(Cadete cadete)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();
            
            try
            {
                Cadete cadeteSeleccionado = listaCadetes.Where(cad => cad.Id == cadete.Id).Single();
                if (cadeteSeleccionado != null)
                {
                    cadeteSeleccionado.Nombre = cadete.Nombre;
                    cadeteSeleccionado.Direccion = cadete.Direccion;
                    cadeteSeleccionado.Telefono = cadete.Telefono;
                  
                    FileStream archivoCadete = new FileStream(rutaArchivo, FileMode.Create);
                    StreamWriter escribirCadete = new StreamWriter(archivoCadete);

                    string strJson = JsonSerializer.Serialize(listaCadetes);
                    escribirCadete.WriteLine("{0}", strJson);

                    escribirCadete.Close();
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public static List<Cadete> guardarCadete(Cadete cadete)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            listaCadetes.Add(cadete);

            FileStream archivoCadetes = new FileStream(rutaArchivo, FileMode.Create);
            StreamWriter escribirCadete = new StreamWriter(archivoCadetes);

            string strJson = JsonSerializer.Serialize(listaCadetes);
            escribirCadete.WriteLine("{0}", strJson);

            escribirCadete.Close();

            return listaCadetes;
        }

        public static void BorrarCadete(int id)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            Cadete cadeteAEliminar = listaCadetes.Where(cadete => cadete.Id == id).Single();
            listaCadetes.Remove(cadeteAEliminar);

            FileStream archivoCadetes = new FileStream(rutaArchivo, FileMode.Create);
            StreamWriter escribirCadete = new StreamWriter(archivoCadetes);

            string strJson = JsonSerializer.Serialize(listaCadetes.ToList());
            escribirCadete.Write("{0}", strJson);

            escribirCadete.Close();
            escribirCadete.Dispose();

            //return listaCadetes;
        }

        public static void AsignarPedidoAlCadete(Cadete cadete)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            try
            {
                Cadete cadeteSeleccionado = listaCadetes.Where(cad => cad.Id == cadete.Id).Single();
                if (cadeteSeleccionado != null)
                {
                    cadeteSeleccionado.ListadoPedidos = cadete.ListadoPedidos;
                    FileStream archivoCadete = new FileStream(rutaArchivo, FileMode.Create);
                    StreamWriter escribirCadete = new StreamWriter(archivoCadete);

                    string strJson = JsonSerializer.Serialize(listaCadetes);
                    escribirCadete.WriteLine("{0}", strJson);

                    escribirCadete.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal static void BorrarTodosLosCadetes()
        {

            try
            {
                FileStream archivoCadete = new FileStream(rutaArchivo, FileMode.Create);
                StreamWriter escribirCadete = new StreamWriter(archivoCadete);

                escribirCadete.WriteLine("[]");
                escribirCadete.Close();
                escribirCadete.Dispose();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< PEDIDOS

        public static List<Pedido> leerArchivoPedidos()
        {
            List<Pedido> listaPedidos;
            //string rutaArchivo = @"Pedidos.json";

            try
            {
                using (StreamReader leerJason = File.OpenText(rutaArchivoPedidos))
                {
                    
                    var Json = leerJason.ReadToEnd();
                    if (Json != "")
                    {
                        listaPedidos = JsonSerializer.Deserialize<List<Pedido>>(Json);
                    }
                    else
                    {
                        listaPedidos = new List<Pedido>();
                    }
                    leerJason.Close();
                    leerJason.Dispose();
                }
            }
            catch (FileNotFoundException)
            {
                listaPedidos = new List<Pedido>();
            }

            return listaPedidos;
        }

        public static Pedido VerPedido(int nro)
        {
            List<Pedido> listaPedidos = leerArchivoPedidos();
            Pedido pedidoSeleccionado = listaPedidos.Where(ped => ped.Nro == nro).Single();
            if (pedidoSeleccionado == null)
            {
                Console.WriteLine("PEDIDO NO ENCONTRADO");
            }
            return pedidoSeleccionado;
        }

        public static void ModificarPedido(Pedido pedido)
        {
            List<Pedido> listaPedidos = leerArchivoPedidos();
            //List<Cadete> listaCadetes = leerArchivoCadetes();
            try
            {
                Pedido pedidoSeleccionado = listaPedidos.Where(ped => ped.Nro == pedido.Nro).Single();
                
                if (pedidoSeleccionado != null)
                {
                    pedidoSeleccionado.Cliente.Nombre = pedido.Cliente.Nombre;
                    pedidoSeleccionado.Cliente.Direccion = pedido.Cliente.Direccion;
                    pedidoSeleccionado.Cliente.Telefono = pedido.Cliente.Telefono;
                    pedidoSeleccionado.Observacion = pedido.Observacion;
                    pedidoSeleccionado.Estado = pedido.Estado;

                    ActualizarPedidoAlCadete(pedidoSeleccionado);

                    FileStream archivoPedidos = new FileStream(rutaArchivoPedidos, FileMode.Create);
                    StreamWriter escribirPedido = new StreamWriter(archivoPedidos);

                    string strJsonPedidos = JsonSerializer.Serialize(listaPedidos);
                    escribirPedido.WriteLine("{0}", strJsonPedidos);
                    escribirPedido.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void ActualizarPedidoAlCadete(Pedido pedido)
        {
            List<Cadete> listaCadetes = leerArchivoCadetes();

            try
            {
                foreach (Cadete cadete in listaCadetes)
                {
                    Pedido pedidoAmodificar = cadete.ListadoPedidos.Where(cad => cad.Nro == pedido.Nro).SingleOrDefault();
                    if (pedidoAmodificar != null)
                    {
                        pedidoAmodificar.Cliente.Nombre = pedido.Cliente.Nombre;
                        pedidoAmodificar.Cliente.Direccion = pedido.Cliente.Direccion;
                        pedidoAmodificar.Cliente.Telefono = pedido.Cliente.Telefono;
                        pedidoAmodificar.Observacion = pedido.Observacion;
                        pedidoAmodificar.Estado = pedido.Estado;
                    }
                }

                FileStream archivoCadete = new FileStream(rutaArchivo, FileMode.Create);
                StreamWriter escribirCadete = new StreamWriter(archivoCadete);

                string strJson = JsonSerializer.Serialize(listaCadetes);
                escribirCadete.WriteLine("{0}", strJson);

                escribirCadete.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        internal static void BorrarTodosLosPedidos()
        {
            //Elimino todos los pedidos del archivo los pedidos
            FileStream archivoPedidos = new FileStream(rutaArchivoPedidos, FileMode.Create);
            StreamWriter escribirPedido = new StreamWriter(archivoPedidos);
           
            escribirPedido.WriteLine("[]");
            escribirPedido.Close();
            escribirPedido.Dispose();

            try
            {
                //Elimino todos los listados de pedidos del archivo los cadetes
                List<Cadete> listaCadetes = leerArchivoCadetes();

                if (listaCadetes != null)
                {
                    foreach (Cadete cadete in listaCadetes)
                    {
                        cadete.ListadoPedidos.RemoveRange(0, cadete.ListadoPedidos.Count());
                    }
                    FileStream archivoCadete = new FileStream(rutaArchivo, FileMode.Create);
                    StreamWriter escribirCadete = new StreamWriter(archivoCadete);

                    string strJson = JsonSerializer.Serialize(listaCadetes);
                    escribirCadete.WriteLine("{0}", strJson);

                    escribirCadete.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
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

        public static void BorrarPedido(int nro)
        {
            List<Pedido> listaPedidos = leerArchivoPedidos();
            List<Cadete> listaCadetes = leerArchivoCadetes();

            Pedido pedidoAEliminar = listaPedidos.Where(pedido => pedido.Nro == nro).Single();
            
            
            for(int i=0; i<listaCadetes.Count(); i++)
            {
                BorrarPedidoAlCadete(listaCadetes[i].ListadoPedidos, pedidoAEliminar);
                AsignarPedidoAlCadete(listaCadetes[i]);
            }
            

            listaPedidos.Remove(pedidoAEliminar);

            FileStream archivoPedidos = new FileStream(rutaArchivoPedidos, FileMode.Create);
            StreamWriter escribirPedido = new StreamWriter(archivoPedidos);

            string strJson = JsonSerializer.Serialize(listaPedidos.ToList());
            escribirPedido.Write("{0}", strJson);

            escribirPedido.Close();
            escribirPedido.Dispose();
        }

        public static void BorrarPedidoAlCadete(List<Pedido>ListP, Pedido pedido) {
            Pedido pedidoAEliminar = ListP.Where(ped => ped.Nro == pedido.Nro).SingleOrDefault();
            if(pedidoAEliminar!=null)
                ListP.Remove(pedidoAEliminar);
        }
    }

}
