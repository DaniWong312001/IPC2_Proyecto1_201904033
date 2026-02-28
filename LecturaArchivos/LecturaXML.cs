using System;
using System.Xml;
using IPC2_Proyecto1.TDA;

namespace IPC2_Proyecto1.LecturaArchivos
{

    // Lee el archivo XML de entrada y carga los pacientes en una lista enlazada.
    public class LectorXML
    {
        // Carga los pacientes desde un archivo XML y los retorna en una lista enlazada.

        public ListaN<Paciente> CargarPacientes(string rutaArchivo)
        {
            ListaN<Paciente> listaPacientes = new ListaN<Paciente>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(rutaArchivo);

                XmlNodeList nodosPaciente = doc.SelectNodes("/pacientes/paciente");
                if (nodosPaciente == null)
                {
                    Console.WriteLine("[ERROR] No se encontraron pacientes en el XML.");
                    return listaPacientes;
                }

                foreach (XmlNode nodoPaciente in nodosPaciente)
                {
                    try
                    {
                        string nombre = nodoPaciente.SelectSingleNode("datospersonales/nombre")?.InnerText ?? "Desconocido";
                        int edad     = int.Parse(nodoPaciente.SelectSingleNode("datospersonales/edad")?.InnerText ?? "0");
                        int periodos = int.Parse(nodoPaciente.SelectSingleNode("periodos")?.InnerText ?? "0");
                        int m        = int.Parse(nodoPaciente.SelectSingleNode("m")?.InnerText ?? "10");

                        Paciente paciente = new Paciente(nombre, edad, periodos, m);

                        // Cargar celdas contagiadas
                        XmlNodeList celdas = nodoPaciente.SelectNodes("rejilla/celda");
                        if (celdas != null)
                        {
                            foreach (XmlNode nodoC in celdas)
                            {
                                int f = int.Parse(nodoC.Attributes["f"]?.Value ?? "0");
                                int c = int.Parse(nodoC.Attributes["c"]?.Value ?? "0");
                                paciente.RejillaActual.ContagiarCelda(f, c);
                            }
                        }

                        paciente.GuardarEstadoInicial();
                        listaPacientes.InsertarAlFinal(paciente);

                        Console.WriteLine($"[OK] Paciente cargado: {nombre}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Al leer un paciente: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Al abrir el archivo XML: {ex.Message}");
            }

            return listaPacientes;
        }
    }
}