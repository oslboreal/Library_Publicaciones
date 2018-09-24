using System;
using System.IO;

namespace Publicaciones
{
    public class Publicacion
    {
        /// <summary>
        /// Propiedad empleada para indicar la ruta donde se almacenaran las diferentes publicaciones.
        /// </summary>
        public static string RutaPublicaciones { get; } = "publicaciones.txt";
        /// <summary>
        /// Propiedad empleada para indicar la ruta donde se especificará el id de la publicación actual.
        /// </summary>
        public static string RutaPublicacionActual { get; set; } = "actual.txt";
        /// <summary>
        /// Propiedad empleada para indicar el ID de la publicación instanciada.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Propiedad empleada para indicar el cuerpo de la publicación instanciada.
        /// </summary>
        public string Cuerpo { get; set; }

        static Publicacion()
        {
            if (!File.Exists(RutaPublicacionActual))
                File.Create(RutaPublicacionActual);

            if (!File.Exists(RutaPublicaciones))
                File.Create(RutaPublicaciones);
        }

        /// <summary>
        /// Método encargado de retornar la próxima publicación.
        /// </summary>
        /// <returns></returns>
        public static int ProximaPublicacion()
        {
            try
            {
                using (StreamReader lector = new StreamReader(RutaPublicacionActual))
                {
                    if (string.IsNullOrEmpty(lector.ReadLine()) || string.IsNullOrWhiteSpace(lector.ReadLine()))
                    {
                        throw new ApplicationException("Error a la hora de obtener una publicación, no hay publicaciones.");
                    }
                    else
                    {
                        var valor = lector.ReadLine();
                        int proxima = (int.Parse(valor) + 1); // Agarro el valor de la publicación actual y lo incremento en uno.
                        return proxima;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error a la hora de obtener una publicación.", ex);
            }
        }

        public static void ActualizarActual(int valor)
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(RutaPublicacionActual))
                {
                    escritor.Write(valor.ToString());
                    escritor.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Método encargado de agregar una publicación al archivo.
        /// </summary>
        /// <returns></returns>
        public static bool AgregarPublicación(string cuerpo)
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(RutaPublicaciones))
                {
                    var ultimoID = ObtenerUltimoID();
                    int nuevoID = ultimoID + 1;
                    escritor.WriteLine($"{cuerpo};{nuevoID}");
                    escritor.Close();

                    if (ObtenerPublicacion(nuevoID) != null)
                    {
                        ActualizarActual(nuevoID);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Error a la hora de agregar una publicación.");
            }
        }

        public static int ObtenerUltimoID()
        {
            try
            {
                using (StreamReader lector = new StreamReader(RutaPublicaciones))
                {
                    if (string.IsNullOrEmpty(lector.ReadLine()) || string.IsNullOrWhiteSpace(lector.ReadLine()))
                    {
                        return 1; // No hay registros, retornamos el primer ID.
                    }
                    else
                    {
                        int maximo = 0;
                        while (!lector.EndOfStream)
                        {
                            string lineaActual = lector.ReadLine();
                            maximo = int.Parse(lineaActual.Split(';')[1]);
                        }
                        return maximo;
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Error a la hora de obtener ultima publicacion.");
            }
        }

        /// <summary>
        /// Método encargado de indicar si la publicación especificada existe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ExistePublicacion(int id)
        {
            try
            {
                using (StreamReader lector = new StreamReader(RutaPublicaciones))
                {
                    if (string.IsNullOrEmpty(lector.ReadLine()) || string.IsNullOrWhiteSpace(lector.ReadLine()))
                    {
                        return false; // No hay registros, retornamos false.
                    }
                    else
                    {
                        bool coincidencia = false;
                        while (!lector.EndOfStream)
                        {
                            string lineaActual = lector.ReadLine();
                            int valor = int.Parse(lineaActual.Split(';')[1]);

                            if (valor == id)
                                coincidencia = true;
                        }
                        return coincidencia;
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Error a la hora de corroborar si Existe publicación.");
            }
        }

        /// <summary>
        /// Método encargado de recorrer el archivo de publicaciones y deserializar la publicación indicada si es que existe.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Publicación si existe; null si no existe.</returns>
        public static Publicacion ObtenerPublicacion(int id)
        {
            try
            {
                using (StreamReader lector = new StreamReader(RutaPublicaciones))
                {
                    Publicacion retorno = null;
                    if (string.IsNullOrEmpty(lector.ReadLine()) || string.IsNullOrWhiteSpace(lector.ReadLine()))
                    {
                        return retorno; // No hay registros, retornamos null.
                    }
                    else
                    {
                        if (ExistePublicacion(id))
                        {
                            while (!lector.EndOfStream)
                            {
                                string lineaActual = lector.ReadLine();
                                int valor = int.Parse(lineaActual.Split(';')[1]);

                                if (valor == id)
                                {
                                    retorno = new Publicacion();
                                    retorno.ID = id;
                                    retorno.Cuerpo = lineaActual.Split(';')[0];
                                }
                            }
                        }
                        return retorno;
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Error a la hora de obtener la publicación.");
            }
        }
    }
}
