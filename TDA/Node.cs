public class Nodo<T> //Crear clase de tipo nodo que obtenga un atributo
    {
        public T Dato { get; set; } //metodo get y set para el objeto nodo
        public Nodo<T> Siguiente { get; set; } //metodo get y set para el siguiente nodo

        public Nodo(T dato) //constructor para inicializar nodo
        {
            Dato = dato;
            Siguiente = null;
        }
    }
