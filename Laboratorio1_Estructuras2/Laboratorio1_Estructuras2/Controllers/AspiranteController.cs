using Microsoft.AspNetCore.Mvc;
using Laboratorio1_Estructuras2.Models;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Laboratorio1_Estructuras2.Controllers
{
    public class AspiranteController : Controller
    {
        public static AVL arbol = new AVL();
        public static List<Aspirante> listaAspi = new List<Aspirante>();
        public static ArbolHuffman huffman = new ArbolHuffman();
        public static ArbolIn confidencial = new ArbolIn();
        public static LZW codificador = new LZW();
        public static List<int> permu= new List<int>();
        public static List<int> permu2 = new List<int>();
        public static Encriptacion cod = new Encriptacion();
        public static String clave = "Adelante caballero, puede pasar";
        public static RSAA Asimetrico;
        public IActionResult Index()
        {
            huffman = huffman.arbol("234987 81324QWERTYUIOP,YUEQIUOQKJADSF-JNVCCHU IEW9413278901-234567890 1 2345ASD,FGHJKLÑHK ADSFHIOEWHE,WFHDSFJBKV-JVCUHA678908,793789347897 8954648798ZXCVBNMJAWFJK'ADSFJIODA  SKNLVADN,J79869");
            listaAspi.Clear();
            permu2 = cod.permutacion("hola");
            permu = cod.permutacion("lahau");
            return View("Index");
        }
        [Route("carga")]
        public IActionResult SubirDatos(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                ViewBag.Error = "Seleccione un archivo CSV válido.";
                return View("SubirDatos", listaAspi);
            }
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                if (listaAspi.Count() > 0)
                {
                    listaAspi.Clear();
                }
                if (arbol.raiz != null)
                {
                    arbol.raiz = null;
                }
                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();
                    var partes = linea.Split(';');
                    if (partes.Length != 2)
                    {
                        continue;
                    }
                    var instruccion = partes[0].Trim();
                    var json = partes[1].Trim();
                    try
                    {
                        JObject jsonData = JObject.Parse(json);
                        var idents = new string[3];
                        if (jsonData.TryGetValue("companies", out JToken companiesToken) && companiesToken.Type == JTokenType.Array)
                        {
                            idents = companiesToken.ToObject<string[]>();
                        }
                        string empres;
                        empres = convertidorVec(idents);
                        Aspirante aspirante = new Aspirante
                        {
                            nombre = (string)jsonData["name"],
                            infoPriv = new List<string> { (string)jsonData["dpi"] },
                            nacimiento = (string)jsonData["datebirth"],
                            direccion = (string)jsonData["address"],
                        };
                        aspirante.infoPriv.Add(empres);
                        switch (instruccion.ToUpper())
                        {
                            case "INSERT":
                                arbol.Insertar(aspirante);
                                break;

                            case "DELETE":
                                arbol.BuscaElimina(aspirante);
                                break;

                            case "PATCH":
                                arbol.actual(aspirante);
                                break;

                            default:
                                return View("Error");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        return View("Error");
                    }
                }
                listaAspi = arbol.listaOrdenada();
            }
            return RedirectToAction("SubirDatos");
        }
        [Route("carga2")]
        public IActionResult SubirDatos2(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                ViewBag.Error = "Seleccione un archivo CSV válido.";
                return View("SubirDatos2", listaAspi);
            }
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                if (listaAspi.Count() > 0)
                {
                    listaAspi.Clear();
                }
                if (confidencial.raiz != null)
                {
                    confidencial.raiz = null;
                }
                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();
                    var partes = linea.Split(';');
                    if (partes.Length != 2)
                    {
                        continue;
                    }
                    var instruccion = partes[0].Trim();
                    var json = partes[1].Trim();
                    try
                    {
                        JObject jsonData = JObject.Parse(json);
                        var idents = new string[3];
                        if (jsonData.TryGetValue("companies", out JToken companiesToken) && companiesToken.Type == JTokenType.Array)
                        {
                            idents = companiesToken.ToObject<string[]>();
                        }
                        string empres;
                        empres = convertidorVec(idents);
                        Aspirante aspirante = new Aspirante
                        {
                            nombre = (string)jsonData["name"],
                            infoPriv = new List<string> { (string)jsonData["dpi"] },
                            nacimiento = (string)jsonData["datebirth"],
                            direccion = (string)jsonData["address"],
                        };
                        aspirante.infoPriv.Add(empres);
                        switch (instruccion.ToUpper())
                        {
                            case "INSERT":
                                aspirante.infoPriv[0] = huffman.Codificar(aspirante.infoPriv[0], huffman);
                                aspirante.infoPriv[1] = huffman.Codificar(aspirante.infoPriv[1].ToUpper(), huffman);  
                                confidencial.Insertar(aspirante);
                                break;

                            case "DELETE":
                                aspirante.infoPriv[0] = huffman.Codificar(aspirante.infoPriv[0], huffman);
                                aspirante.infoPriv[1] = huffman.Codificar(aspirante.infoPriv[1].ToUpper(), huffman);
                                confidencial.Eliminar(aspirante);
                                break;

                            case "PATCH":
                                aspirante.infoPriv[0] = huffman.Codificar(aspirante.infoPriv[0], huffman);
                                aspirante.infoPriv[1] = huffman.Codificar(aspirante.infoPriv[1].ToUpper(), huffman);
                                confidencial.actual(aspirante);
                                break;

                            default:
                                return View("Error");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        return View("Error");
                    }
                }
                listaAspi = confidencial.listaOrdenada();
            }
            return RedirectToAction("SubirDatos2");
        }
        [HttpPost]
        [Route("buscarNom")]
        public IActionResult encontrado(string nombre)
        {
            if (nombre == null)
            {
                return View("ErrorBuscar");
            }
            List<Aspirante> aspirantesEncontrados = new List<Aspirante>();
            aspirantesEncontrados = arbol.busqueda(nombre);
            if (aspirantesEncontrados == null)
            {
                return View("ErrorBuscar");
            }
            return View("Encontrado", aspirantesEncontrados);
        }
        [HttpPost]
        [Route("buscarDpi")]
        public IActionResult encontradoDpi(string nombre)
        {
            if (nombre == null)
            {
                return View("ErrorBuscarDpi");
            }
            string codigo = huffman.Codificar(nombre, huffman);
            Aspirante aspirante = new Aspirante();
            aspirante = confidencial.Buscar(codigo);
            if (aspirante == null)
            {
                return View("ErrorBuscarDpi");
            }
            aspirante.infoPriv[0] = huffman.Decodificar(aspirante.infoPriv[0], huffman);
            aspirante.infoPriv[1] = huffman.Decodificar(aspirante.infoPriv[1], huffman);
            List<Aspirante> ListaAspirante = new List<Aspirante>();
            ListaAspirante.Add(aspirante);
            return View("EncontradoDPI", ListaAspirante);
        }
        public string convertidorVec(string[] vector)
        {
            string vectorAString = "";
            if (vector.Count() == 0)
            {
                return vectorAString;
            }
            for (int i = 0; i < vector.Length; i++)
            {
                if (vectorAString == "")
                {
                    vectorAString = vectorAString + vector[i];
                }
                else
                {
                    vectorAString = vectorAString + " " + vector[i];
                }
            }
            return vectorAString;
        }
        [Route("carga3")]
        public IActionResult SubirDatos3(IFormFile archivo, string ruta)
        {
            if (archivo == null || archivo.Length == 0 || ruta == null)
            {
                ViewBag.Error = "Seleccione un archivo CSV válido. y una ruta de carpeta";
                return View("SubirDatos3", listaAspi);
            }
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                if (listaAspi.Count() > 0)
                {
                    listaAspi.Clear();
                }
                if (confidencial.raiz != null)
                {
                    confidencial.raiz = null;
                }
                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();
                    var partes = linea.Split(';');
                    if (partes.Length != 2)
                    {
                        continue;
                    }
                    var instruccion = partes[0].Trim();
                    var json = partes[1].Trim();
                    try
                    {
                        JObject jsonData = JObject.Parse(json);
                        var idents = new string[3];
                        if (jsonData.TryGetValue("companies", out JToken companiesToken) && companiesToken.Type == JTokenType.Array)
                        {
                            idents = companiesToken.ToObject<string[]>();
                        }
                        string empres;
                        empres = convertidorVec(idents);
                        Aspirante aspirante = new Aspirante
                        {
                            nombre = (string)jsonData["name"],
                            infoPriv = new List<string> { (string)jsonData["dpi"] },
                            nacimiento = (string)jsonData["datebirth"],
                            direccion = (string)jsonData["address"],
                            carta = new List<string>(),
                            diccionario = new List<Dictionary<string, int>>(),
                        };
                        aspirante.infoPriv.Add(empres);
                        switch (instruccion.ToUpper())
                        {
                            case "INSERT":
                                confidencial.Insertar(aspirante);
                                break;

                            case "DELETE":
                                confidencial.Eliminar(aspirante);
                                break;

                            case "PATCH":
                                confidencial.actual(aspirante);
                                break;

                            default:
                                return View("Error");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        return View("Error");
                    }
                }

                try
                {
                    string[] archivos = Directory.GetFiles(ruta, "*.txt");
                    foreach (string archivoEncontrado in archivos)
                    {
                        string contenido = System.IO.File.ReadAllText(archivoEncontrado);
                        string nombreArchivo = Path.GetFileNameWithoutExtension(archivoEncontrado);
                        string[] partesNombre = nombreArchivo.Split('-');
                        var aspirante = confidencial.Buscar(partesNombre[1]);
                        var tupla = codificador.comprimir(contenido);
                        string codigo = tupla.Item1;
                        Dictionary<string, int> diccionario = tupla.Item2;
                        aspirante.carta.Add(codigo);
                        aspirante.diccionario.Add(diccionario);
                    }
                }
                catch (Exception e)
                {
                    return View("ErrorArchivo");
                }

                listaAspi = confidencial.listaOrdenada();
            }
            return RedirectToAction("SubirDatos3");
        }
        [HttpPost]
        [Route("buscarCartas")]
        public IActionResult descom(string Dpi)
        {
            if(Dpi == null || confidencial.raiz == null)
            {
                return View("ErrorArchivo");
            }
            var aspiranteTemp = confidencial.Buscar(Dpi);
            var aspirante = new Aspirante();
            aspirante.nombre = aspiranteTemp.nombre;
            aspirante.infoPriv = aspiranteTemp.infoPriv;
            aspirante.direccion = aspiranteTemp.direccion;
            aspirante.nacimiento = aspiranteTemp.nacimiento;
            aspirante.carta = aspiranteTemp.carta;
            aspirante.diccionario = aspiranteTemp.diccionario;
            if (aspirante == null)
            {
                return View("ErrorArchivo");
            }
            for(int i = 0; i < aspirante.carta.Count(); i++)
            {
                aspirante.carta[i] = codificador.descomprimir(aspirante.carta[i], aspirante.diccionario[i]);
            }
            List<Aspirante> ListaAspirante = new List<Aspirante>();
            ListaAspirante.Add(aspirante);
            return View("Cartas", ListaAspirante);
        }
        public IActionResult SubirDatos4(IFormFile archivo, string ruta)
        {
            if (archivo == null || archivo.Length == 0 || ruta == null)
            {
                ViewBag.Error = "Seleccione un archivo CSV válido. y una ruta de carpeta";
                return View("SubirDatos4", listaAspi);
            }
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                if (listaAspi.Count() > 0)
                {
                    listaAspi.Clear();
                }
                if (confidencial.raiz != null)
                {
                    confidencial.raiz = null;
                }
                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();
                    var partes = linea.Split(';');
                    if (partes.Length != 2)
                    {
                        continue;
                    }
                    var instruccion = partes[0].Trim();
                    var json = partes[1].Trim();
                    try
                    {
                        JObject jsonData = JObject.Parse(json);
                        var idents = new string[3];
                        if (jsonData.TryGetValue("companies", out JToken companiesToken) && companiesToken.Type == JTokenType.Array)
                        {
                            idents = companiesToken.ToObject<string[]>();
                        }
                        string empres;
                        empres = convertidorVec(idents);
                        Aspirante aspirante = new Aspirante
                        {
                            nombre = (string)jsonData["name"],
                            infoPriv = new List<string> { (string)jsonData["dpi"] },
                            nacimiento = (string)jsonData["datebirth"],
                            direccion = (string)jsonData["address"],
                            carta = new List<string>(),
                            Convs = new List<string>(),
                            diccionario = new List<Dictionary<string, int>>(),
                        };
                        aspirante.infoPriv.Add(empres);
                        switch (instruccion.ToUpper())
                        {
                            case "INSERT":
                                confidencial.Insertar(aspirante);
                                break;

                            case "DELETE":
                                confidencial.Eliminar(aspirante);
                                break;

                            case "PATCH":
                                confidencial.actual(aspirante);
                                break;

                            default:
                                return View("ErrorSub");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        return View("ErrorSub");
                    }
                }

                try
                {
                    string[] archivos = Directory.GetFiles(ruta, "*.txt");
                    foreach (string archivoEncontrado in archivos)
                    {
                        string contenido = System.IO.File.ReadAllText(archivoEncontrado);
                        string nombreArchivo = Path.GetFileNameWithoutExtension(archivoEncontrado);
                        string[] partesNombre = nombreArchivo.Split('-');
                        if (partesNombre[0] == "REC")
                        {
                            var aspirante = confidencial.Buscar(partesNombre[1]);
                            var tupla = codificador.comprimir(contenido);
                            string codigo = tupla.Item1;
                            Dictionary<string, int> diccionario = tupla.Item2;
                            aspirante.carta.Add(codigo);
                            aspirante.diccionario.Add(diccionario);
                        }
                        else if(partesNombre[0] == "CONV")
                        {
                            var aspirante = confidencial.Buscar(partesNombre[1]);
                            string codigo = cod.permutar(contenido, permu);
                            codigo = cod.CifrarCesar(codigo);
                            aspirante.Convs.Add(codigo);
                        }
                    }
                }
                catch (Exception e)
                {
                    return View("ErrorSub");
                }

                listaAspi = confidencial.listaOrdenada();
            }
            return RedirectToAction("SubirDatos4");
        }
        [HttpPost]
        [Route("buscarConversaciones")]
        public IActionResult desi(string Dpi)
        {
            if (Dpi == null || confidencial.raiz == null)
            {
                return View("ErrorSub");
            }
            var aspiranteTemp = confidencial.Buscar(Dpi);
            var aspirante = new Aspirante();
            aspirante.nombre = aspiranteTemp.nombre;
            aspirante.infoPriv = aspiranteTemp.infoPriv;
            aspirante.direccion = aspiranteTemp.direccion;
            aspirante.nacimiento = aspiranteTemp.nacimiento;
            aspirante.carta = aspiranteTemp.carta;
            aspirante.diccionario = aspiranteTemp.diccionario;
            aspirante.Convs = aspiranteTemp.Convs;
            if (aspirante == null)
            {
                return View("ErrorSub");
            }
            for (int i = 0; i < aspirante.Convs.Count(); i++)
            {
                aspirante.Convs[i] = cod.DescifrarCesar(aspirante.Convs[i]);
                aspirante.Convs[i] = cod.desPermutar(aspirante.Convs[i], permu);
            }
            List<Aspirante> ListaAspirante = new List<Aspirante>();
            ListaAspirante.Add(aspirante);
            return View("Convs", ListaAspirante);
        }
        public IActionResult SubirDatos5(IFormFile archivo, string ruta)
        {
            if (archivo == null || archivo.Length == 0 || ruta == null)
            {
                ViewBag.Error = "Seleccione un archivo CSV válido. y una ruta de carpeta";
                return View("SubirDatos5", listaAspi);
            }
            using (var reader = new StreamReader(archivo.OpenReadStream()))
            {
                if (listaAspi.Count() > 0)
                {
                    listaAspi.Clear();
                }
                if (confidencial.raiz != null)
                {
                    confidencial.raiz = null;
                }
                while (!reader.EndOfStream)
                {
                    var linea = reader.ReadLine();
                    var partes = linea.Split(';');
                    if (partes.Length != 2)
                    {
                        continue;
                    }
                    var instruccion = partes[0].Trim();
                    var json = partes[1].Trim();
                    try
                    {
                        JObject jsonData = JObject.Parse(json);
                        var idents = new string[3];
                        if (jsonData.TryGetValue("companies", out JToken companiesToken) && companiesToken.Type == JTokenType.Array)
                        {
                            idents = companiesToken.ToObject<string[]>();
                        }
                        string empres;
                        empres = convertidorVec(idents);
                        Aspirante aspirante = new Aspirante
                        {
                            nombre = (string)jsonData["name"],
                            infoPriv = new List<string> { (string)jsonData["dpi"] },
                            nacimiento = (string)jsonData["datebirth"],
                            direccion = (string)jsonData["address"],
                            carta = new List<string>(),
                            Convs = new List<string>(),
                            diccionario = new List<Dictionary<string, int>>(),
                            reclutadores = (string)jsonData["recluiter"]
                        };
                        aspirante.infoPriv.Add(empres);
                        switch (instruccion.ToUpper())
                        {
                            case "INSERT":
                                confidencial.Insertar(aspirante);
                                break;

                            case "DELETE":
                                confidencial.Eliminar(aspirante);
                                break;

                            case "PATCH":
                                confidencial.actual(aspirante);
                                break;

                            default:
                                return View("ErrorSub5");
                        }
                    }
                    catch (JsonReaderException)
                    {
                        return View("ErrorSub5");
                    }
                }

                try
                {
                    string[] archivos = Directory.GetFiles(ruta, "*.txt");
                    foreach (string archivoEncontrado in archivos)
                    {
                        string contenido = System.IO.File.ReadAllText(archivoEncontrado);
                        string nombreArchivo = Path.GetFileNameWithoutExtension(archivoEncontrado);
                        string[] partesNombre = nombreArchivo.Split('-');
                        if (partesNombre[0] == "REC")
                        {
                            var aspirante = confidencial.Buscar(partesNombre[1]);
                            var tupla = codificador.comprimir(contenido);
                            string codigo = tupla.Item1;
                            Dictionary<string, int> diccionario = tupla.Item2;
                            aspirante.carta.Add(codigo);
                            aspirante.diccionario.Add(diccionario);
                        }
                        else if (partesNombre[0] == "CONV")
                        {
                            var aspirante = confidencial.Buscar(partesNombre[1]);
                            string codigo = cod.permutar(contenido, permu);
                            codigo = cod.CifrarCesar(codigo);
                            aspirante.Convs.Add(codigo);
                        }
                    }
                }
                catch (Exception e)
                {
                    return View("ErrorSub5");
                }

                listaAspi = confidencial.listaOrdenada();
            }
            return RedirectToAction("SubirDatos5");
        }
        static bool EsPrimo(int numero)
        {
            if (numero <= 1)
                return false;
            if (numero <= 3)
                return true;

            if (numero % 2 == 0 || numero % 3 == 0)
                return false;

            for (int i = 5; i * i <= numero; i += 6)
            {
                if (numero % i == 0 || numero % (i + 2) == 0)
                    return false;
            }

            return true;
        }

        static int GenerarNumeroPrimoAleatorio(int min, int max)
        {
            Random random = new Random();
            List<int> primos = new List<int>();

            for (int i = min; i <= max; i++)
            {
                if (EsPrimo(i))
                    primos.Add(i);
            }

            if (primos.Count == 0)
                return -1; // No se encontraron números primos en el rango

            int indiceAleatorio = random.Next(primos.Count);
            return primos[indiceAleatorio];
        }
        [HttpPost]
        [Route("GenerarClave")]
        public IActionResult generar(string Dpi, string reclu)
        {
            if (Dpi == null || confidencial.raiz == null)
            {
                return View("ErrorSub5");
            }
            var aspirante = confidencial.Buscar(Dpi);
            if(aspirante.reclutadores != reclu)
            {
                return View("ErrorSub5");
            }
            Asimetrico = new RSAA(GenerarNumeroPrimoAleatorio(1, 100), GenerarNumeroPrimoAleatorio(1, 100));
            aspirante.encriptado = Asimetrico.Crypt(clave, Asimetrico.public1, Asimetrico.common);
            return View("clave", ("Identificador: " + Convert.ToString(Asimetrico.private1) + ", Contraseña: " + Convert.ToString(Asimetrico.common)));
        }
        [HttpPost]
        [Route("mostrarInfoClave")]
        public IActionResult buscarCla(string Dpi1, string reclu1, string clave1, string ident)
        {
            if (Dpi1 == null || confidencial.raiz == null)
            {
                return View("ErrorSub5");
            }
            var aspirante = confidencial.Buscar(Dpi1);
            if (aspirante.reclutadores != reclu1 || aspirante.encriptado == null)
            {
                return View("ErrorSub5");
            }
            if(Asimetrico.Decrypt(aspirante.encriptado, Convert.ToInt64(clave1), Convert.ToInt64(ident)) == clave)
            {
                List<Aspirante> lista = new List<Aspirante>();
                lista.Add(aspirante);
                return View("EncontradoClave", lista);
            }
            return View("ErrorSub5");
        }
    }
}
