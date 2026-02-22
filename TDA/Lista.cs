using System;

    public class ListaN<T> //crear clase tipo lista enlazada
    {
        private Nodo<T> _cabeza; //crear variable nodo principal o cabecera
        private int _tamano; //crear variable del tamaño de la lista

        public int Tamano => _tamano; //inicializar variable tamaño
        public bool EstaVacia => _cabeza == null; //incializar variable cabeza a nulo

        public ListaN() //metodo lista enlazada
        {
            _cabeza = null;
            _tamano = 0;
        }

        //Inserta un elemento al final de la lista
        public void InsertarAlFinal(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);
            if (_cabeza == null)
            {
                _cabeza = nuevo;
            }
            else
            {
                Nodo<T> actual = _cabeza;
                while (actual.Siguiente != null)
                    actual = actual.Siguiente;
                actual.Siguiente = nuevo;
            }
            _tamano++;
        }

        //Inserta un elemento al inicio de la lista
        public void InsertarAlInicio(T dato)
        {
            Nodo<T> nuevo = new Nodo<T>(dato);
            nuevo.Siguiente = _cabeza;
            _cabeza = nuevo;
            _tamano++;
        }

        //Obtiene el elemento en la posición indicada
        public T ObtenerEn(int indice)
        {
            if (indice < 0 || indice >= _tamano)
                throw new IndexOutOfRangeException($"Índice {indice} fuera de rango.");

            Nodo<T> actual = _cabeza;
            for (int i = 0; i < indice; i++)
                actual = actual.Siguiente;

            return actual.Dato;
        }

        //Elimina el primer elemento que cumpla el predicado
        public bool Eliminar(Func<T, bool> predicado)
        {
            if (_cabeza == null) return false;

            if (predicado(_cabeza.Dato))
            {
                _cabeza = _cabeza.Siguiente;
                _tamano--;
                return true;
            }

            Nodo<T> actual = _cabeza;
            while (actual.Siguiente != null)
            {
                if (predicado(actual.Siguiente.Dato))
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    _tamano--;
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        //Busca el primer elemento que cumpla el predicado
        public T Buscar(Func<T, bool> predicado)
        {
            Nodo<T> actual = _cabeza;
            while (actual != null)
            {
                if (predicado(actual.Dato))
                    return actual.Dato;
                actual = actual.Siguiente;
            }
            return default;
        }

        //Limpia todos los elementos de la lista
        public void Limpiar()
        {
            _cabeza = null;
            _tamano = 0;
        }

        //Ejecuta una acción sobre cada elemento
        public void ParaCadaUno(Action<T> accion)
        {
            Nodo<T> actual = _cabeza;
            while (actual != null)
            {
                accion(actual.Dato);
                actual = actual.Siguiente;
            }
        }

        //Convierte la lista a un arreglo (para comparaciones de estado)
        public T[] AArreglo()
        {
            T[] arreglo = new T[_tamano];
            Nodo<T> actual = _cabeza;
            for (int i = 0; i < _tamano; i++)
            {
                arreglo[i] = actual.Dato;
                actual = actual.Siguiente;
            }
            return arreglo;
        }
    }
