using System;
using IPC2_Proyecto1.LecturaArchivos;
using IPC2_Proyecto1.Simulador;
using IPC2_Proyecto1.TDA;
using IPC2_Proyecto1.Visualizacion;

namespace IPC2_Proyecto1
{
    // Menú principal de la aplicación de diagnóstico epidemiológico.

    public class MenuPrincipal
    {
        private ListaN<Paciente> _listaPacientes;
        private Paciente _pacienteSeleccionado;

        // Histórico de hashes para el paciente en análisis paso a paso
        private ListaN<string> _historicoHashes;

        private readonly LectorXML _lector;
        private readonly EscritorXML _escritor;
        private readonly SimuladorEnfermedad _simulador;
        private readonly GeneradorGraphviz _graphviz;

        public MenuPrincipal()
        {
            _listaPacientes = new ListaN<Paciente>();
            _lector  = new LectorXML();
            _escritor = new EscritorXML();
            _simulador = new SimuladorEnfermedad();
            _graphviz = new GeneradorGraphviz();
        }

        public void Ejecutar()
        {
            Console.Clear();
            MostrarBienvenida();

            bool salir = false;
            while (!salir)
            {
                MostrarMenuPrincipal();
                string opcion = Console.ReadLine()?.Trim() ?? "";

                switch (opcion)
                {
                    case "1": CargarArchivoXML(); break;
                    case "2": SeleccionarPaciente(); break;
                    case "3": EjecutarUnPeriodo(); break;
                    case "4": EjecutarAutomatico(); break;
                    case "5": GenerarSalidaXML(); break;
                    case "6": GenerarGraphviz(); break;
                    case "7": LimpiarMemoria(); break;
                    case "8": salir = true; break;
                    default:
                        Console.WriteLine("\n[!] Opción no válida. Presione Enter para continuar...");
                        Console.ReadLine();
                        break;
                }
            }

            Console.WriteLine("\nCerrando aplicación. ¡Hasta luego!\n");
        }

        // ─────────────────────────────────────────────────────
        //  MENÚ VISUAL
        // ─────────────────────────────────────────────────────

        private void MostrarBienvenida()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║   Laboratorio de Investigación Epidemiológica GT     ║");
            Console.WriteLine("║   IPC2 – Proyecto 1  |  USAC Ingeniería              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void MostrarMenuPrincipal()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("══════════════════ MENÚ PRINCIPAL ══════════════════");
            Console.ResetColor();

            string nombrePaciente = _pacienteSeleccionado != null
                ? $"{_pacienteSeleccionado.Nombre} | Período: {_pacienteSeleccionado.PeriodoActual} | " +
                  $"Celulas Sanas: {_pacienteSeleccionado.RejillaActual.CeldasSanas} | " +
                  $"Celulas Contagiadas: {_pacienteSeleccionado.RejillaActual.CeldasContagiadas}"
                : "Ninguno";

            Console.WriteLine($"  Paciente activo : {nombrePaciente}");
            Console.WriteLine($"  Pacientes cargados: {_listaPacientes.Tamano}");
            Console.WriteLine();
            Console.WriteLine("  [1] Cargar archivo XML de entrada");
            Console.WriteLine("  [2] Seleccionar paciente para análisis");
            Console.WriteLine("  [3] Ejecutar un período (paso a paso)");
            Console.WriteLine("  [4] Ejecutar automáticamente hasta diagnóstico");
            Console.WriteLine("  [5] Generar archivo XML de salida");
            Console.WriteLine("  [6] Visualizar con Graphviz");
            Console.WriteLine("  [7] Limpiar memoria");
            Console.WriteLine("  [8] Salir");
            Console.WriteLine();
            Console.Write("  Seleccione una opción: ");
        }

        // ─────────────────────────────────────────────────────
        //  OPERACIONES
        // ─────────────────────────────────────────────────────

        private void CargarArchivoXML()
        {
            Console.WriteLine();
            Console.Write("  Ingrese la ruta del archivo XML de entrada: ");
            string ruta = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(ruta))
            {
                Console.WriteLine("[!] Ruta vacía.");
                Pausar();
                return;
            }

            ListaN<Paciente> nuevos = _lector.CargarPacientes(ruta);
            nuevos.ParaCadaUno(p => _listaPacientes.InsertarAlFinal(p));
            Console.WriteLine($"\n[OK] {nuevos.Tamano} paciente(s) cargado(s). Total: {_listaPacientes.Tamano}");
            Pausar();
        }

        private void SeleccionarPaciente()
        {
            if (_listaPacientes.EstaVacia)
            {
                Console.WriteLine("\n[!] No hay pacientes cargados. Cargue primero un archivo XML.");
                Pausar();
                return;
            }

            Console.WriteLine("\n  Pacientes disponibles:");
            for (int i = 0; i < _listaPacientes.Tamano; i++)
            {
                Paciente p = _listaPacientes.ObtenerEn(i);
                string diag = p.Resultado != ResultadoDiagnostico.Pendiente
                    ? $" [{p.Resultado}]" : "";
                Console.WriteLine($"  [{i + 1}] {p.Nombre} – Edad: {p.Edad} – M: {p.RejillaActual.M}{diag}");
            }

            Console.Write("\n  Seleccione el número de paciente: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= _listaPacientes.Tamano)
            {
                _pacienteSeleccionado = _listaPacientes.ObtenerEn(idx - 1);
                _historicoHashes = new ListaN<string>();
                _historicoHashes.InsertarAlFinal(_pacienteSeleccionado.RejillaInicial.ObtenerHash());
                Console.WriteLine($"\n[OK] Paciente seleccionado: {_pacienteSeleccionado.Nombre}");
            }
            else
            {
                Console.WriteLine("[!] Selección inválida.");
            }
            Pausar();
        }

        private void EjecutarUnPeriodo()
        {
            if (_pacienteSeleccionado == null)
            {
                Console.WriteLine("\n[!] Seleccione un paciente primero (opción 2).");
                Pausar();
                return;
            }

            if (_pacienteSeleccionado.Resultado != ResultadoDiagnostico.Pendiente)
            {
                Console.WriteLine($"\n[!] El diagnóstico ya está determinado: {_pacienteSeleccionado.Resultado}");
                Pausar();
                return;
            }

            bool terminado = _simulador.EjecutarUnPeriodo(_pacienteSeleccionado, _historicoHashes);

            Console.WriteLine($"\n  Período: {_pacienteSeleccionado.PeriodoActual}");
            Console.WriteLine($"  Células sanas: {_pacienteSeleccionado.RejillaActual.CeldasSanas}");
            Console.WriteLine($"  Células contagiadas: {_pacienteSeleccionado.RejillaActual.CeldasContagiadas}");

            if (_pacienteSeleccionado.RejillaActual.M <= 20)
                _pacienteSeleccionado.RejillaActual.MostrarEnConsola();

            if (terminado)
                MostrarDiagnostico(_pacienteSeleccionado);

            Pausar();
        }

        private void EjecutarAutomatico()
        {
            if (_pacienteSeleccionado == null)
            {
                Console.WriteLine("\n[!] Seleccione un paciente primero (opción 2).");
                Pausar();
                return;
            }

            if (_pacienteSeleccionado.Resultado != ResultadoDiagnostico.Pendiente)
            {
                Console.WriteLine($"\n[!] El diagnóstico ya está determinado: {_pacienteSeleccionado.Resultado}");
                Pausar();
                return;
            }

            Console.WriteLine("\n  Ejecutando simulación automática...");
            _simulador.EjecutarAutomatico(_pacienteSeleccionado);
            MostrarDiagnostico(_pacienteSeleccionado);
            Pausar();
        }

        private void GenerarSalidaXML()
        {
            if (_listaPacientes.EstaVacia)
            {
                Console.WriteLine("\n[!] No hay pacientes cargados.");
                Pausar();
                return;
            }

            Console.Write("\n  Ingrese la ruta del archivo de salida (ej: salida.xml): ");
            string ruta = Console.ReadLine()?.Trim() ?? "salida.xml";
            _escritor.GenerarSalida(_listaPacientes, ruta);
            Pausar();
        }

        private void GenerarGraphviz()
        {
            Console.WriteLine("\n  [1] Visualizar lista de pacientes");
            Console.WriteLine("  [2] Visualizar rejilla del paciente activo");
            Console.Write("\n  Seleccione: ");
            string op = Console.ReadLine()?.Trim() ?? "";

            if (op == "1")
                _graphviz.VisualizarListaPacientes(_listaPacientes);
            else if (op == "2" && _pacienteSeleccionado != null)
                _graphviz.VisualizarRejilla(_pacienteSeleccionado);
            else
                Console.WriteLine("[!] Opción inválida o no hay paciente seleccionado.");

            Pausar();
        }

        private void LimpiarMemoria()
        {
            Console.Write("\n  ¿Está seguro que desea limpiar todos los pacientes? (s/n): ");
            string resp = Console.ReadLine()?.Trim().ToLower() ?? "n";
            if (resp == "s")
            {
                _listaPacientes.Limpiar();
                _pacienteSeleccionado = null;
                _historicoHashes = null;
                Console.WriteLine("[OK] Memoria limpiada.");
            }
            Pausar();
        }

        private void MostrarDiagnostico(Paciente p)
        {
            Console.ForegroundColor = p.Resultado switch
            {
                ResultadoDiagnostico.Mortal => ConsoleColor.Red,
                ResultadoDiagnostico.Grave  => ConsoleColor.DarkYellow,
                ResultadoDiagnostico.Leve   => ConsoleColor.Green,
                _                           => ConsoleColor.White
            };
            Console.WriteLine($"\n  ══ DIAGNÓSTICO: {p.Resultado.ToString().ToUpper()} ══");
            if (p.N  > 0) Console.WriteLine($"  N  = {p.N}");
            if (p.N1 > 0) Console.WriteLine($"  N1 = {p.N1}");
            Console.ResetColor();
        }

        private void Pausar()
        {
            Console.WriteLine("\n  Presione Enter para continuar...");
            Console.ReadLine();
            Console.Clear();
            MostrarBienvenida();
        }
    }
}