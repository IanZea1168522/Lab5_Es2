namespace Laboratorio1_Estructuras2.Models
{
    public class NodoIn
    {
        public NodoIn(Aspirante d)
        {
            dato = d;
            izquierda = null;
            derecha = null;
            padre = null;
            peso = 0;
        }

        public Aspirante dato { get; set; }
        public NodoIn izquierda { get; set; }
        public NodoIn derecha { get; set; }
        public NodoIn padre { get; set; }
        public double peso { get; set; }

        public int altura { get; set; }

    }
}
