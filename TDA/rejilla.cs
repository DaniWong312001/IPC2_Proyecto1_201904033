
namespace IPC2_Proyecto1.TDA
{
public class Rejilla
    {
        private ListaN<Celda> _celdas;
        public int M { get; private set; }  // Tamaño de la rejilla (M x M)

        public int CeldasContagiadas //metodo para contar celdas contagiadas
        {
            get
            {
                int count = 0;
                _celdas.ParaCadaUno(c => { if (c.EstaContagiada) count++; });
                return count;
            }
        }

        public int CeldasSanas => (M * M) - CeldasContagiadas; //numero de celdas sanas

        public Rejilla(int m)
        {
            M = m;
            _celdas = new ListaN<Celda>();

            // Inicializar todas las celdas como sanas
            for (int f = 1; f <= m; f++)
                for (int c = 1; c <= m; c++)
                    _celdas.InsertarAlFinal(new Celda(f, c, false));
        }

        //Marca una celda como contagiada
        public void ContagiarCelda(int fila, int columna)
        {
            Celda c = ObtenerCelda(fila, columna);
            if (c != null) c.EstaContagiada = true;
        }

        //Obtiene la celda en la posición indicada
        public Celda ObtenerCelda(int fila, int columna)
        {
            return _celdas.Buscar(c => c.Fila == fila && c.Columna == columna);
        }

        // Cuenta los vecinos contagiados de una celda
        public int ContarVecinosContagiados(int fila, int columna)
        {
            int count = 0;
            for (int df = -1; df <= 1; df++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (df == 0 && dc == 0) continue;
                    int nf = fila + df;
                    int nc = columna + dc;
                    if (nf >= 1 && nf <= M && nc >= 1 && nc <= M)
                    {
                        Celda vecino = ObtenerCelda(nf, nc);
                        if (vecino != null && vecino.EstaContagiada)
                            count++;
                    }
                }
            }
            return count;
        }

        // Aplica un período de simulación usando las reglas del enunciado.
        // Regla 1: Contagiada con 2 o 3 vecinos contagiados → sigue contagiada.
        // Regla 2: Sana con exactamente 3 vecinos contagiados → se contagia.
        public void AplicarPeriodo()
        {
            // Calcular el nuevo estado en un arreglo temporal
            bool[,] nuevoEstado = new bool[M + 1, M + 1];

            for (int f = 1; f <= M; f++)
            {
                for (int c = 1; c <= M; c++)
                {
                    int vecinos = ContarVecinosContagiados(f, c);
                    Celda celda = ObtenerCelda(f, c);
                    bool contagiada = celda.EstaContagiada;

                    if (contagiada)
                        nuevoEstado[f, c] = (vecinos == 2 || vecinos == 3);
                    else
                        nuevoEstado[f, c] = (vecinos == 3);
                }
            }

            // Aplicar nuevo estado
            for (int f = 1; f <= M; f++)
                for (int c = 1; c <= M; c++)
                    ObtenerCelda(f, c).EstaContagiada = nuevoEstado[f, c];
        }

        // Genera una cadena hash que representa el estado actual de la rejilla.
        // Sirve para comparar patrones.
        public string ObtenerHash()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int f = 1; f <= M; f++)
                for (int c = 1; c <= M; c++)
                    sb.Append(ObtenerCelda(f, c).EstaContagiada ? '1' : '0');
            return sb.ToString();
        }

        //Clona la rejilla actual con su estado
        public Rejilla Clonar()
        {
            Rejilla clon = new Rejilla(M);
            for (int f = 1; f <= M; f++)
                for (int c = 1; c <= M; c++)
                    clon.ObtenerCelda(f, c).EstaContagiada = ObtenerCelda(f, c).EstaContagiada;
            return clon;
        }

        //Muestra la rejilla en consola (para grids pequeños)
        public void MostrarEnConsola()
        {
            Console.Write("   ");
            for (int c = 1; c <= M && c <= 20; c++)
                Console.Write($"{c,3}");
            Console.WriteLine();

            for (int f = 1; f <= M && f <= 20; f++)
            {
                Console.Write($"{f,3}");
                for (int c = 1; c <= M && c <= 20; c++)
                {
                    Celda celda = ObtenerCelda(f, c);
                    Console.Write(celda.EstaContagiada ? " [X]" : " [ ]");
                }
                if (M > 20) Console.Write(" ...");
                Console.WriteLine();
            }
            if (M > 20) Console.WriteLine("  ... (rejilla truncada para visualización)");
        }
    }
}