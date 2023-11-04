namespace Laboratorio1_Estructuras2.Models
{
    public class Encriptacion
    {
        public List<int> permutacion(string cadena)
        {
            List<int> numeros = new List<int>();
            for (int i = 0; i < cadena.Length; i++)
            {
                numeros.Add(i);
            }
            int semilla = cadena.Length * 123;
            Random random = new Random(semilla);
            for (int i = numeros.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = numeros[i];
                numeros[i] = numeros[j];
                numeros[j] = temp;
            }
            return numeros;
        }
        public string permutar(string cadena, List<int> perm)
        {
            List<string> lista = new List<string>();
            int cantidad = perm.Count();
            for (int i = 0; i < cadena.Length; i += cantidad)
            {
                if (i + cantidad <= cadena.Length)
                {
                    string sub = cadena.Substring(i, cantidad);
                    char[] permutado = new char[cantidad];
                    for (int j = 0; j < cantidad; j++)
                    {
                        int newIndex = perm.IndexOf(j);
                        permutado[newIndex] = sub[j];
                    }
                    lista.Add(new string(permutado));
                }
                else
                {
                    lista.Add(cadena.Substring(i));
                    break;
                }
            }
            string desordenado = string.Join("", lista);
            return desordenado;
        }
        public string desPermutar(string cadena, List<int> perm)
        {
            int cantidad = perm.Count;
            int longitud = cadena.Length;
            char[] resultado = new char[longitud];

            for (int i = 0; i < longitud; i += cantidad)
            {
                int tamaño = Math.Min(cantidad, longitud - i);
                string segmento = cadena.Substring(i, tamaño);

                if (tamaño == cantidad)
                {
                    char[] reordered = new char[tamaño];

                    for (int j = 0; j < tamaño; j++)
                    {
                        int originalIndex = perm[j];
                        reordered[originalIndex] = segmento[j];
                    }

                    for (int j = 0; j < tamaño; j++)
                    {
                        resultado[i + j] = reordered[j];
                    }
                }
                else
                {
                    for (int j = 0; j < tamaño; j++)
                    {
                        resultado[i + j] = segmento[j];
                    }
                }
            }
            return new string(resultado);
        }
        public string CifrarCesar(string mensaje)
        {
            char[] caracteres = mensaje.ToCharArray();

            for (int i = 0; i < caracteres.Length; i++)
            {
                char caracter = caracteres[i];

                if (char.IsLetter(caracter) && caracter != 'á' && caracter != 'é' && caracter != 'í' && caracter != 'ó' && caracter != 'ú' && caracter != 'Á' && caracter != 'É' && caracter != 'Í' && caracter != 'Ó' && caracter != 'Ú')
                {
                    char limite = char.IsUpper(caracter) ? 'A' : 'a';
                    caracteres[i] = (char)(limite + (caracter - limite + 8) % 26);
                }
            }

            return new string(caracteres);
        }
        public string DescifrarCesar(string mensajeCifrado)
        {
            char[] caracteres = mensajeCifrado.ToCharArray();

            for (int i = 0; i < caracteres.Length; i++)
            {
                char caracter = caracteres[i];

                if (char.IsLetter(caracter) && caracter != 'á' && caracter != 'é' && caracter != 'í' && caracter != 'ó' && caracter != 'ú' && caracter != 'Á' && caracter != 'É' && caracter != 'Í' && caracter != 'Ó' && caracter != 'Ú')
                {
                    char limite = char.IsUpper(caracter) ? 'A' : 'a';
                    int offset = (caracter - limite - 8 + 26) % 26;
                    caracteres[i] = (char)(limite + offset);
                }
            }

            return new string(caracteres);
        }
    }
}
