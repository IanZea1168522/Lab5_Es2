namespace Laboratorio1_Estructuras2.Models
{
    public class AVL
    {
        //raíz
        public Nodo raiz;
        //incersión
        public void Insertar(Aspirante dato)
        {
            raiz = Insertar(dato, raiz);
        }

        private Nodo Insertar(Aspirante dato, Nodo nodo)
        {
            if (nodo == null)
            {
                nodo = new Nodo(dato);
                return nodo;
            }
            if (nodo.dato.nombre == dato.nombre)
            {
                nodo.lista.Add(dato);
                return nodo;
            }

            if (dato.nombre.CompareTo(nodo.dato.nombre) < 0)
                nodo.izquierda = Insertar(dato, nodo.izquierda);
            else
                nodo.derecha = Insertar(dato, nodo.derecha);

            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));

            int balance = FactorBalance(nodo);

            if (balance > 1 && dato.nombre.CompareTo(nodo.izquierda.dato.nombre) < 0)
                return RotacionDerecha(nodo);

            if (balance < -1 && dato.nombre.CompareTo(nodo.derecha.dato.nombre) > 0)
                return RotacionIzquierda(nodo);

            if (balance > 1 && dato.nombre.CompareTo(nodo.izquierda.dato.nombre) > 0)
            {
                nodo.izquierda = RotacionIzquierda(nodo.izquierda);
                return RotacionDerecha(nodo);
            }

            if (balance < -1 && dato.nombre.CompareTo(nodo.derecha.dato.nombre) < 0)
            {
                nodo.derecha = RotacionDerecha(nodo.derecha);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }
        //obtener la altura
        private int Altura(Nodo nodo)
        {
            if (nodo == null) { return 0; }


            return nodo.altura;
        }
        //obtener el balance
        private int FactorBalance(Nodo nodo)
        {
            if (nodo == null)
                return 0;

            return Altura(nodo.izquierda) - Altura(nodo.derecha);
        }
        //rotaciones 
        private Nodo RotacionIzquierda(Nodo nodo)
        {
            Nodo nododerecha = nodo.derecha;
            Nodo nodoizquierda = nododerecha.izquierda;

            nododerecha.izquierda = nodo;
            nodo.derecha = nodoizquierda;

            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));
            nododerecha.altura = 1 + Math.Max(Altura(nododerecha.izquierda), Altura(nododerecha.derecha));

            return nododerecha;
        }
        private Nodo RotacionDerecha(Nodo nodo)
        {
            Nodo nodoizquierda = nodo.izquierda;
            Nodo nododerecha = nodoizquierda.derecha;

            nodoizquierda.derecha = nodo;
            nodo.izquierda = nododerecha;

            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));
            nodoizquierda.altura = 1 + Math.Max(Altura(nodoizquierda.izquierda), Altura(nodoizquierda.derecha));

            return nodoizquierda;
        }
        //eliminaciones
        public void BuscaElimina(Aspirante dato)
        {
            raiz = Elimina(raiz, dato);
        }

        private Nodo Elimina(Nodo nodo, Aspirante dato)
        {
            if (nodo == null)
            {
                return nodo;
            }

            // Realiza la eliminación recursiva como en un árbol binario de búsqueda
            if (dato.nombre.CompareTo(nodo.dato.nombre) < 0)
            {
                nodo.izquierda = Elimina(nodo.izquierda, dato);
            }
            else if (dato.nombre.CompareTo(nodo.dato.nombre) > 0)
            {
                nodo.derecha = Elimina(nodo.derecha, dato);
            }
            else
            {
                // Nodo encontrado, manejar casos de eliminación
                if (nodo.lista.Count == 0)
                {
                    // Si la lista está vacía, elimina este nodo
                    return null;
                }
                else
                {
                    // Si la lista no está vacía, simplemente elimina el elemento de la lista
                    nodo.borrar(dato);
                }
            }

            // Actualizar altura y realizar rotaciones si es necesario
            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));
            int balance = FactorBalance(nodo);

            if (balance > 1 && FactorBalance(nodo.izquierda) >= 0)
                return RotacionDerecha(nodo);

            if (balance > 1 && FactorBalance(nodo.izquierda) < 0)
            {
                nodo.izquierda = RotacionIzquierda(nodo.izquierda);
                return RotacionDerecha(nodo);
            }

            if (balance < -1 && FactorBalance(nodo.derecha) <= 0)
                return RotacionIzquierda(nodo);

            if (balance < -1 && FactorBalance(nodo.derecha) > 0)
            {
                nodo.derecha = RotacionDerecha(nodo.derecha);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }
        public void actual(Aspirante aspirante)
        {
            actualizar(aspirante, raiz);
        }
        private void actualizar(Aspirante aspirante, Nodo nodo)
        {
            if (nodo != null)
            {
                if (nodo.dato.nombre == aspirante.nombre)
                {
                    for (int i = 0; i < nodo.lista.Count(); i++)
                    {
                        if (nodo.lista[i].infoPriv[0] == aspirante.infoPriv[0])
                        {
                            nodo.lista[i] = aspirante;
                            break;
                        }
                    }
                }
                else if (aspirante.nombre.CompareTo(nodo.dato.nombre) < 0)
                {
                    actualizar(aspirante, nodo.izquierda);
                }
                else if (aspirante.nombre.CompareTo(nodo.dato.nombre) > 0)
                {
                    actualizar(aspirante, nodo.derecha);
                }
            }
        }
        //buscar
        public List<Aspirante> busqueda(string nombre)
        {
            return buscar(nombre, raiz);
        }
        private List<Aspirante> buscar(string nombre, Nodo nodo)
        {
            if (nodo == null)
            {
                return null;
            }
            if (nodo.dato.nombre == nombre)
            {
                return nodo.lista;
            }
            else if (nombre.CompareTo(nodo.dato.nombre) < 0)
            {
                return buscar(nombre, nodo.izquierda);
            }
            else if (nombre.CompareTo(nodo.dato.nombre) > 0)
            {
                return buscar(nombre, nodo.derecha);
            }
            return null;
        }
        public List<Aspirante> listaOrdenada()
        {
            return InOrderAVL(raiz);
        }
        private List<Aspirante> InOrderAVL(Nodo nodoActual)
        {
            List<Aspirante> nodosInOrder = new List<Aspirante>();

            if (nodoActual != null)
            {
                nodosInOrder.AddRange(InOrderAVL(nodoActual.izquierda));
                nodosInOrder.AddRange(nodoActual.lista);
                nodosInOrder.AddRange(InOrderAVL(nodoActual.derecha));
            }
            return nodosInOrder;
        }
    }
}
