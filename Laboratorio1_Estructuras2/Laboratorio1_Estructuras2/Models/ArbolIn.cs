namespace Laboratorio1_Estructuras2.Models
{
    public class ArbolIn
    {
        public NodoIn raiz;

        // Métodos de inserción, eliminación y búsqueda

        public void Insertar(Aspirante dato)
        {
            raiz = Insertar(dato, raiz);
        }

        private NodoIn Insertar(Aspirante dato, NodoIn nodo)
        {
            if (nodo == null)
                return new NodoIn(dato);

            if (dato.CompareTo(nodo.dato) < 0)
                nodo.izquierda = Insertar(dato, nodo.izquierda);
            else
                nodo.derecha = Insertar(dato, nodo.derecha);

            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));

            int balance = FactorBalance(nodo);

            if (balance > 1 && dato.CompareTo(nodo.izquierda.dato) < 0)
                return RotacionDerecha(nodo);

            if (balance < -1 && dato.CompareTo(nodo.derecha.dato) > 0)
                return RotacionIzquierda(nodo);

            if (balance > 1 && dato.CompareTo(nodo.izquierda.dato) > 0)
            {
                nodo.izquierda = RotacionIzquierda(nodo.izquierda);
                return RotacionDerecha(nodo);
            }

            if (balance < -1 && dato.CompareTo(nodo.derecha.dato) < 0)
            {
                nodo.derecha = RotacionDerecha(nodo.derecha);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }
        private NodoIn Eliminar(Aspirante dato, NodoIn nodo)
        {
            if (nodo == null)
                return nodo;

            if (dato.CompareTo(nodo.dato) < 0)
                nodo.izquierda = Eliminar(dato, nodo.izquierda);
            else if (dato.CompareTo(nodo.dato) > 0)
                nodo.derecha = Eliminar(dato, nodo.derecha);
            else
            {
                if (nodo.izquierda == null || nodo.derecha == null)
                {
                    NodoIn aux = null;
                    if (nodo.izquierda == null)
                        aux = nodo.derecha;
                    else
                        aux = nodo.izquierda;

                    if (aux == null)
                    {
                        nodo = null;
                    }
                    else
                    {
                        nodo = aux;
                    }
                }
                else
                {
                    NodoIn aux = Minimo(nodo.derecha);
                    nodo.dato = aux.dato;
                    nodo.derecha = Eliminar(aux.dato, nodo.derecha);
                }
            }

            if (nodo == null)
                return nodo;

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
        public Aspirante Buscar(string valor)
        {
            return Buscar(valor, raiz);
        }

        private Aspirante Buscar(string valor, NodoIn nodo)
        {
            if (nodo == null)
            {
                return null;
            }
            if (valor.CompareTo(nodo.dato.infoPriv[0]) < 0)
            {
                return Buscar(valor, nodo.izquierda);
            }
            else if (valor.CompareTo(nodo.dato.infoPriv[0]) > 0)
            {
                return Buscar(valor, nodo.derecha);
            }
            else
            {
                return nodo.dato;
            }
        }
        private NodoIn RotacionDerecha(NodoIn nodo)
        {
            NodoIn nodoizquierda = nodo.izquierda;
            NodoIn nododerecha = nodoizquierda.derecha;

            nodoizquierda.derecha = nodo;
            nodo.izquierda = nododerecha;

            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));
            nodoizquierda.altura = 1 + Math.Max(Altura(nodoizquierda.izquierda), Altura(nodoizquierda.derecha));

            return nodoizquierda;
        }

        private NodoIn RotacionIzquierda(NodoIn nodo)
        {
            NodoIn nododerecha = nodo.derecha;
            NodoIn nodoizquierda = nododerecha.izquierda;

            nododerecha.izquierda = nodo;
            nodo.derecha = nodoizquierda;

            nodo.altura = 1 + Math.Max(Altura(nodo.izquierda), Altura(nodo.derecha));
            nododerecha.altura = 1 + Math.Max(Altura(nododerecha.izquierda), Altura(nododerecha.derecha));

            return nododerecha;
        }


        private int Altura(NodoIn nodo)
        {
            if (nodo == null) { return 0; }


            return nodo.altura;
        }

        private int FactorBalance(NodoIn nodo)
        {
            if (nodo == null)
                return 0;

            return Altura(nodo.izquierda) - Altura(nodo.derecha);
        }

        private NodoIn Minimo(NodoIn nodo)
        {
            NodoIn actual = nodo;
            while (actual.izquierda != null)
            {
                actual = actual.izquierda;
            }
            return actual;
        }
        private NodoIn EliminarMinimo(NodoIn nodo)
        {
            if (nodo.izquierda == null)
                return nodo.derecha;

            nodo.izquierda = EliminarMinimo(nodo.izquierda);

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

        public void Eliminar(Aspirante dato)
        {
            raiz = Eliminar(dato, raiz);
        }

        public List<Aspirante> listaOrdenada()
        {
            return InOrderAVL(raiz);
        }
        private List<Aspirante> InOrderAVL(NodoIn nodoActual)
        {
            List<Aspirante> nodosInOrder = new List<Aspirante>();

            if (nodoActual != null)
            {
                nodosInOrder.AddRange(InOrderAVL(nodoActual.izquierda));
                nodosInOrder.Add(nodoActual.dato);
                nodosInOrder.AddRange(InOrderAVL(nodoActual.derecha));
            }

            return nodosInOrder;
        }
        //actualizar
        public void actual(Aspirante aspirante)
        {
            actualizar(aspirante, raiz);
        }
        private void actualizar(Aspirante aspirante, NodoIn nodo)
        {
            if (nodo != null)
            {
                if (nodo.dato.infoPriv[0] == aspirante.infoPriv[0])
                {
                    nodo.dato = aspirante;
                }
                else if (aspirante.CompareTo(nodo.dato) < 0)
                {
                    actualizar(aspirante, nodo.izquierda);
                }
                else if (aspirante.CompareTo(nodo.dato) > 0)
                {
                    actualizar(aspirante, nodo.derecha);
                }
            }
        }
    }
}

