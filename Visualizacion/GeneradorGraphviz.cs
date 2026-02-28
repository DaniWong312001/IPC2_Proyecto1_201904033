using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using IPC2_Proyecto1.TDA;

namespace IPC2_Proyecto1.Visualizacion
{

    // Genera archivos .dot y los renderiza con Graphviz para visualizar los TDAs.

    public class GeneradorGraphviz
    {
        private readonly string _carpetaSalida;

        public GeneradorGraphviz(string carpetaSalida = "graphviz_output")
        {
            _carpetaSalida = carpetaSalida;
            Directory.CreateDirectory(carpetaSalida);
        }

        // Genera visualización de la lista enlazada de pacientes.

        public void VisualizarListaPacientes(ListaN<Paciente> lista)
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph ListaPacientes {");
            dot.AppendLine("  rankdir=LR;");
            dot.AppendLine("  node [shape=record, style=filled, fillcolor=lightblue];");
            dot.AppendLine("  edge [arrowhead=vee];");

            // Nodo NULL al inicio
            dot.AppendLine("  NULL_START [label=\"NULL\", shape=plaintext, fillcolor=white];");

            string anterior = "NULL_START";

            for (int i = 0; i < lista.Tamano; i++)
            {
                Paciente p = lista.ObtenerEn(i);
                string nodeId = $"N{i}";
                string resultado = p.Resultado == ResultadoDiagnostico.Pendiente
                    ? "Pendiente" : p.Resultado.ToString();

                dot.AppendLine($"  {nodeId} [label=\"{{<dato> {EscaparDot(p.Nombre)}\\nEdad: {p.Edad}\\nM: {p.RejillaActual.M}|<sig> →}}\"];");
                dot.AppendLine($"  {anterior} -> {nodeId};");
                anterior = $"{nodeId}:sig";
            }

            // Nodo NULL al final
            dot.AppendLine("  NULL_END [label=\"NULL\", shape=plaintext, fillcolor=white];");
            dot.AppendLine($"  {anterior} -> NULL_END;");
            dot.AppendLine("}");

            string archivoDot = Path.Combine(_carpetaSalida, "lista_pacientes.dot");
            string archivoPng = Path.Combine(_carpetaSalida, "lista_pacientes.png");

            File.WriteAllText(archivoDot, dot.ToString());
            RenderizarDot(archivoDot, archivoPng);
        }

        // Genera visualización de la rejilla de un paciente como grafo de nodos.

        public void VisualizarRejilla(Paciente paciente)
        {
            int m = paciente.RejillaActual.M;
            if (m > 20)
            {
                Console.WriteLine("[Graphviz] Rejilla demasiado grande para visualizar (M > 20).");
                return;
            }

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph Rejilla {");
            dot.AppendLine("  rankdir=TB;");
            dot.AppendLine("  node [shape=square, width=0.4, fixedsize=true, fontsize=10];");
            dot.AppendLine("  edge [style=invis];");  // Aristas invisibles solo para layout

            for (int f = 1; f <= m; f++)
            {
                dot.Append($"  {{ rank=same; ");
                for (int c = 1; c <= m; c++)
                {
                    var celda = paciente.RejillaActual.ObtenerCelda(f, c);
                    string color = celda.EstaContagiada ? "cornflowerblue" : "white";
                    string label = celda.EstaContagiada ? "X" : " ";
                    dot.AppendLine($"  C_{f}_{c} [label=\"{label}\", fillcolor={color}, style=filled];");
                }
                dot.AppendLine("  }");

                // Conectar horizontalmente con aristas invisibles
                for (int c = 1; c < m; c++)
                    dot.AppendLine($"  C_{f}_{c} -> C_{f}_{c + 1};");
            }

            // Conectar verticalmente con aristas invisibles
            for (int f = 1; f < m; f++)
                for (int c = 1; c <= m; c++)
                    dot.AppendLine($"  C_{f}_{c} -> C_{f + 1}_{c};");

            dot.AppendLine("}");

            string nombre = EscaparNombreArchivo(paciente.Nombre);
            string archivoDot = Path.Combine(_carpetaSalida, $"rejilla_{nombre}_p{paciente.PeriodoActual}.dot");
            string archivoPng = Path.Combine(_carpetaSalida, $"rejilla_{nombre}_p{paciente.PeriodoActual}.png");

            File.WriteAllText(archivoDot, dot.ToString());
            RenderizarDot(archivoDot, archivoPng);
        }

        // Ejecuta el comando dot de Graphviz para renderizar un .dot a .png
        private void RenderizarDot(string archivoDot, string archivoPng)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-Tpng \"{archivoDot}\" -o \"{archivoPng}\"",
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                using Process proceso = Process.Start(psi);
                proceso.WaitForExit();

                if (proceso.ExitCode == 0)
                    Console.WriteLine($"[Graphviz] Imagen generada: {archivoPng}");
                else
                {
                    string error = proceso.StandardError.ReadToEnd();
                    Console.WriteLine($"[Graphviz] Error al renderizar: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Graphviz] No se pudo ejecutar Graphviz: {ex.Message}");
                Console.WriteLine($"[Graphviz] Archivo .dot guardado en: {archivoDot}");
            }
        }

        private string EscaparDot(string texto) => texto.Replace("\"", "\\\"").Replace("<", "\\<").Replace(">", "\\>");
        private string EscaparNombreArchivo(string nombre) => nombre.Replace(" ", "_").Replace("/", "-");
    }
}
