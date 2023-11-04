using System;
using System.Collections.Generic;
using System.Linq;
using Laboratorio1_Estructuras2.Models;
namespace Laboratorio1_Estructuras2.Models
{
    public class ArbolHuffman
    {
        public NodoHuffman raiz { get; set; }

        public ArbolHuffman arbol(string contexto)
        {
            Dictionary<char, int> frecuencias = contexto
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            Cola<NodoHuffman> cola = new Cola<NodoHuffman>(
                frecuencias.Select(kv => new NodoHuffman { letra = kv.Key, frecuencia = kv.Value })
            );

            while (cola.Count > 1)
            {
                NodoHuffman izquierda = cola.sacar();
                NodoHuffman derecha = cola.sacar();
                NodoHuffman padre = new NodoHuffman { frecuencia = izquierda.frecuencia + derecha.frecuencia, izquierda = izquierda, derecha = derecha };
                cola.ingresar(padre);
            }

            return new ArbolHuffman { raiz = cola.sacar() };
        }

        public Dictionary<char, string> Tabla()
        {
            Dictionary<char, string> Tabla = new Dictionary<char, string>();
            ConstructorTabla(raiz, "", Tabla);
            return Tabla;
        }

        private void ConstructorTabla(NodoHuffman node, string actual, Dictionary<char, string> Tabla)
        {
            if (node == null)
                return;

            if (node.letra != '\0')
            {
                Tabla[node.letra] = actual;
            }

            ConstructorTabla(node.izquierda, actual + "0", Tabla);
            ConstructorTabla(node.derecha, actual + "1", Tabla);
        }
        public string Codificar(string palabra, ArbolHuffman arbol)
        {
            Dictionary<char, string> tabla = arbol.Tabla();
            string codigo = "";

            foreach (char c in palabra)
            {
                if (tabla.ContainsKey(c))
                {
                    codigo += tabla[c];
                }
                else
                {
                    throw new Exception($"icono '{c}' no se encuentra.");
                }
            }

            return codigo;
        }
        public string Decodificar(string codigo, ArbolHuffman arbol)
        {
            NodoHuffman actual = arbol.raiz;
            string textoDecifrado = "";

            foreach (char bit in codigo)
            {
                if (bit == '0')
                {
                    actual = actual.izquierda;
                }
                else if (bit == '1')
                {
                    actual = actual.derecha;
                }

                if (actual.letra != '\0')
                {
                    textoDecifrado += actual.letra;
                    actual = arbol.raiz; 
                }
            }

            return textoDecifrado;
        }
    }
}
