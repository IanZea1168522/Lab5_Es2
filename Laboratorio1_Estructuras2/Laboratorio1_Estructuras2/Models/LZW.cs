using System;
using System.Collections.Generic;
using System.Text;
namespace Laboratorio1_Estructuras2.Models
{
    public class LZW
    {
        public (string Cadena, Dictionary<string, int> Diccionario) comprimir(string carta)
        {
            Dictionary<string, int> diccionario = new Dictionary<string, int>();
            for (int i = 0; i < carta.Length; i++)
            {
                string letra = Convert.ToString(carta[i]);
                if (!diccionario.ContainsKey(letra))
                {
                    diccionario.Add(letra, diccionario.Count);
                }
            }
            string actual = null;
            string siguiente = "";
            string union = "";
            string codigo = "";
            for (int i = 0; i < carta.Length; i++)
            {
                siguiente = Convert.ToString(carta[i]);
                if (actual == null)
                {
                    union = siguiente;
                }
                else
                {
                    union = actual + siguiente;
                }
                if (diccionario.ContainsKey(union))
                {
                    actual = union;
                }
                else
                {
                    codigo += diccionario[actual] + ",";
                    diccionario.Add(union, diccionario.Count);
                    actual = siguiente;
                }
            }
            codigo += diccionario[actual] + ",";
            return (codigo,diccionario);
        }

        public string descomprimir(string codigo, Dictionary<string, int> diccionario)
        {
            string mensaje = "";
            int cantidad = codigo.Split(',').Count() - 1;
            int nums;
            for (int i = 0; i < cantidad; i++)
            {
                nums = int.Parse(codigo.Split(',')[i]);
                mensaje += diccionario.FirstOrDefault(x => x.Value == nums).Key;
            }
            return mensaje;
        }
    }
}
