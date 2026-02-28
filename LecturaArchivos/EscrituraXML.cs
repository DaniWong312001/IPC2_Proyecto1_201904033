using System;
using System.Xml;
using IPC2_Proyecto1.TDA;

namespace IPC2_Proyecto1.LecturaArchivos
{
    // Genera el archivo XML de salida con los resultados del diagnóstico de cada paciente.
    public class EscritorXML
    {

        // Escribe el archivo XML de resultados a partir de la lista de pacientes.

        public void GenerarSalida(ListaN<Paciente> listaPacientes, string rutaSalida)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration declaracion = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(declaracion);

                XmlElement raiz = doc.CreateElement("pacientes");
                doc.AppendChild(raiz);

                listaPacientes.ParaCadaUno(paciente =>
                {
                    XmlElement elemPaciente = doc.CreateElement("paciente");

                    // datos personales
                    XmlElement datos = doc.CreateElement("datospersonales");
                    AgregarHijo(doc, datos, "nombre", paciente.Nombre);
                    AgregarHijo(doc, datos, "edad", paciente.Edad.ToString());
                    elemPaciente.AppendChild(datos);

                    AgregarHijo(doc, elemPaciente, "periodos", paciente.Periodos.ToString());
                    AgregarHijo(doc, elemPaciente, "m", paciente.RejillaActual.M.ToString());

                    // resultado
                    string resultado = paciente.Resultado switch
                    {
                        ResultadoDiagnostico.Mortal => "mortal",
                        ResultadoDiagnostico.Grave  => "grave",
                        ResultadoDiagnostico.Leve   => "leve",
                        _                           => "pendiente"
                    };
                    AgregarHijo(doc, elemPaciente, "resultado", resultado);

                    // N y N1 si aplica
                    if (paciente.N > 0)
                        AgregarHijo(doc, elemPaciente, "n", paciente.N.ToString());
                    if (paciente.N1 > 0)
                        AgregarHijo(doc, elemPaciente, "n1", paciente.N1.ToString());

                    raiz.AppendChild(elemPaciente);
                });

                doc.Save(rutaSalida);
                Console.WriteLine($"[OK] Archivo de salida generado: {rutaSalida}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Al generar el XML de salida: {ex.Message}");
            }
        }

        private void AgregarHijo(XmlDocument doc, XmlElement padre, string nombre, string valor)
        {
            XmlElement elem = doc.CreateElement(nombre);
            elem.InnerText = valor;
            padre.AppendChild(elem);
        }
    }
}