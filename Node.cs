public class Node //Creacion de la Clase Nodo

{
    private object dato; //declarar variable tipo objeto para almacenar los valores
    private Node puntero_next; // declarar variable tipo nodo para crear el puntero al siguiente nodo

    public Node (object values, Node next_node) //Crear el metodo constructor para los atributos de la clase

    {
        dato = values;
        puntero_next = next_node;

    }

    public Node(object values): this(values, null)
    {
        
    }
    public Node Siguiente //creacion de los metodos getter y setter para el puntero
    {
        get{ return puntero_next;}
        set{ puntero_next = value;}
    }

    public Object Dato //creacion de los metodos getter y setter para la variable dato
    {
        get{ return dato;}
    }
}