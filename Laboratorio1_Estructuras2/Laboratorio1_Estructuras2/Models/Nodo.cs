namespace Laboratorio1_Estructuras2.Models
{
    public class Nodo
    {
        public Nodo(Aspirante aspirante)
        {
            dato = aspirante;
            izquierda = null;
            derecha = null;
            padre = null;
            lista = new List<Aspirante> { aspirante };
            altura = 0;
        }
        public Aspirante dato { get; set; }
        public Nodo izquierda { get; set; }
        public Nodo derecha { get; set; }
        public Nodo padre { get; set; }
        public List<Aspirante> lista { get; set; }
        public int altura { get; set; }
        public void borrar(Aspirante aspirante)
        {
            lista.RemoveAll(Aspirante => Aspirante.infoPriv[0] == aspirante.infoPriv[0]);
        }
    }
}
