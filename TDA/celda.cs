
namespace IPC2_Proyecto1.TDA
{
public class Celda //clase para cada celula
    {
        public int Fila { get; set; } //variable para almacenar valor de fila
        public int Columna { get; set; } //variable para almacenar valor de columna
        public bool EstaContagiada { get; set; } //variable para el valor de cada celula si esta contagiada o no

        public Celda(int fila, int columna, bool estaContagiada = false) //metodo constructor
        {
            Fila = fila;
            Columna = columna;
            EstaContagiada = estaContagiada;
        }

        public Celda Clonar() //metodo para clonar valor de celda
        {
            return new Celda(Fila, Columna, EstaContagiada);
        }

        public override string ToString() //metodo para obetener datos de la celda a cadena
        {
            return $"({Fila},{Columna})={( EstaContagiada ? "1" : "0")}";
        }
    }
}