using System;
using System.Collections.Generic;
using System.Linq;
namespace Laboratorio1_Estructuras2.Models
{
    public class Cola<T> where T : IComparable<T>
    {
        private List<T> obj;

        public int Count { get { return obj.Count; } }

        public Cola(IEnumerable<T> coleccion)
        {
            obj = new List<T>(coleccion);
            obj.Sort();
        }

        public T sacar()
        {
            if (Count == 0)
                throw new InvalidOperationException("Ahhhh, está vacío.mp4");
            T item = obj[0];
            obj.RemoveAt(0);
            return item;
        }

        public void ingresar(T item)
        {
            obj.Add(item);
            obj.Sort();
        }
    }
}
