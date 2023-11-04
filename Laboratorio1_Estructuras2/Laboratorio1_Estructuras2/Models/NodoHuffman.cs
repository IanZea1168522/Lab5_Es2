using System;
using System.Collections.Generic;
using System.Linq;
namespace Laboratorio1_Estructuras2.Models
{
    public class NodoHuffman : IComparable<NodoHuffman>
    {
        public char letra { get; set; }
        public int frecuencia { get; set; }
        public NodoHuffman izquierda { get; set; }
        public NodoHuffman derecha { get; set; }
        public int CompareTo(NodoHuffman other)
        {
            return frecuencia.CompareTo(other.frecuencia);
        }
    }
}
