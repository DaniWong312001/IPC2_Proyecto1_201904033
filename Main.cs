using System;
using System.Xml;

namespace IPC2_Proyecto1_201904033

{
    class Program

    {

        public static void LeerXML()
        {
            // Ruta del archivo XML
            string path = "C:/Users/danie/OneDrive/Desktop/Proyectos IPC2/IPC2_Proyecto1_201904033/ejemplo.xml";
            // Crear instancia de XmlDocument
            XmlDocument xmlDoc = new XmlDocument();


            try
            {
                // Cargar el archivo XML
                xmlDoc.Load(path);
                // Obtener el nodo raíz 
                XmlElement raiz = xmlDoc.DocumentElement;

                Console.WriteLine("Contenido del archivo XML:");

                // Recorrer todos los nodos hijos
                foreach (XmlNode nodo in raiz.ChildNodes) 
                { 
                    Console.WriteLine($"Nodo: {nodo.Name}");
                    // Si el nodo tiene atributos 
                    if (nodo.Attributes != null) 
                    { 
                        foreach (XmlAttribute atributo in nodo.Attributes) 
                        { 
                            Console.WriteLine($" Atributo: {atributo.Name} = {atributo.Value}"); 
                        } 
                    } 
                    // Si el nodo tiene texto interno 
                    if (!string.IsNullOrWhiteSpace(nodo.InnerText)) 
                    { 
                        Console.WriteLine($" Valor: {nodo.InnerText}"); 
                    } 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
            }

        }

        static void Main(string[] args)

        {

            char entrada;

            Console.Write("Elija una Opción\n" +
            "1. Cargar archivo de un Paciente\n" +
            "2. Generar Archivo de Paciente\n" +
            "3. Limpiar información Cargada\n" +
            "4. Salir del Programa\n");

            entrada = Convert.ToChar(Console.ReadLine());

            switch (entrada)
            {
                case '1':
                    Console.WriteLine("Cargar archivo de un Paciente");
                    LeerXML();
                    break;
                case '2':
                    Console.WriteLine("Generar Archivo de Paciente");
                    break;
                case '3':
                    Console.WriteLine("Limpiar información Cargada");
                    break;
                case '4':
                    Console.WriteLine("Fin del Programa, Gracias");
                    break;
                default:
                    Console.WriteLine("Opcion no Válida");
                    break;
            }

        }
    }

}