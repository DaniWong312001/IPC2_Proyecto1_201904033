using System;
using IPC2_Proyecto1.TDA;

namespace IPC2_Proyecto1.Simulador
{
    // Motor de simulación que aplica las reglas de contagio y detecta patrones repetidos.

    public class SimuladorEnfermedad
    {
        private readonly int MAX_PERIODOS = 10000;

        // Avanza UN período en la simulación del paciente.
        // Retorna true si se detectó un diagnóstico definitivo.
        public bool EjecutarUnPeriodo(Paciente paciente, ListaN<string> historicoHashes)
        {
            if (paciente.Resultado != ResultadoDiagnostico.Pendiente)
                return true; // Ya tiene diagnóstico

            string hashAntes = paciente.RejillaActual.ObtenerHash();
            string hashInicial = paciente.RejillaInicial.ObtenerHash();

            // Aplicar reglas de contagio
            paciente.RejillaActual.AplicarPeriodo();
            paciente.PeriodoActual++;

            string hashDespues = paciente.RejillaActual.ObtenerHash();

            // --- Verificar si volvemos al patrón inicial ---
            if (hashDespues == hashInicial)
            {
                paciente.N = paciente.PeriodoActual;
                paciente.N1 = 0;
                if (paciente.N == 1)
                    paciente.Resultado = ResultadoDiagnostico.Mortal;
                else
                    paciente.Resultado = ResultadoDiagnostico.Grave;
                return true;
            }

            // --- Verificar si el nuevo patrón ya apareció antes (ciclo secundario) ---
            int periodoRepeticion = BuscarHashEnHistorico(historicoHashes, hashDespues);
            if (periodoRepeticion >= 0)
            {
                // El patrón actual ya apareció en el período 'periodoRepeticion'
                int n  = periodoRepeticion;       // período en que apareció por primera vez
                int n1 = paciente.PeriodoActual - periodoRepeticion; // longitud del ciclo
                paciente.N  = n;
                paciente.N1 = n1;
                if (n1 == 1)
                    paciente.Resultado = ResultadoDiagnostico.Mortal;
                else
                    paciente.Resultado = ResultadoDiagnostico.Grave;
                return true;
            }

            // Guardar hash del estado actual en el histórico
            historicoHashes.InsertarAlFinal(hashDespues);

            // Límite de períodos
            if (paciente.PeriodoActual >= Math.Min(paciente.Periodos, MAX_PERIODOS))
            {
                paciente.Resultado = ResultadoDiagnostico.Leve;
                return true;
            }

            return false;
        }

        // Ejecuta automáticamente todos los períodos necesarios hasta obtener diagnóstico.
        public void EjecutarAutomatico(Paciente paciente)
        {
            ListaN<string> historico = new ListaN<string>();
            // Guardar hash del estado inicial (período 0)
            historico.InsertarAlFinal(paciente.RejillaInicial.ObtenerHash());

            bool terminado = false;
            while (!terminado)
            {
                terminado = EjecutarUnPeriodo(paciente, historico);
            }
        }

        // Busca un hash en la lista histórica y retorna el índice (= período) donde apareció.
        // Retorna -1 si no se encontró.

        private int BuscarHashEnHistorico(ListaN<string> historico, string hash)
        {
            for (int i = 0; i < historico.Tamano; i++)
            {
                if (historico.ObtenerEn(i) == hash)
                    return i; // el índice equivale al período
            }
            return -1;
        }
    }
}