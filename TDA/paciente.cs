 public enum ResultadoDiagnostico //resultado diagnostico del paciente
    {
        Pendiente,
        Leve,
        Grave,
        Mortal
    }

    // Representa un paciente con su rejilla de tejido y su diagnóstico.
    public class Paciente
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Periodos { get; set; }        // Máximo de períodos a evaluar
        public Rejilla RejillaActual { get; set; }
        public Rejilla RejillaInicial { get; private set; }  // Copia del estado inicial

        // Diagnóstico
        public ResultadoDiagnostico Resultado { get; set; }
        public int N { get; set; }               // Período en que se repite el patrón inicial
        public int N1 { get; set; }              // Período secundario de repetición
        public int PeriodoActual { get; set; }   // Período en curso

        public Paciente(string nombre, int edad, int periodos, int m)
        {
            Nombre = nombre;
            Edad = edad;
            Periodos = periodos;
            RejillaActual = new Rejilla(m);
            Resultado = ResultadoDiagnostico.Pendiente;
            N = 0;
            N1 = 0;
            PeriodoActual = 0;
        }

        //Guarda una copia del estado inicial una vez cargados los datos
        public void GuardarEstadoInicial()
        {
            RejillaInicial = RejillaActual.Clonar();
        }

        public override string ToString()
        {
            return $"Paciente: {Nombre} | Edad: {Edad} | M: {RejillaActual.M} | Período: {PeriodoActual}";
        }
    }