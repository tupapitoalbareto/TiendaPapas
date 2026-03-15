using CsvHelper;
using System.Globalization;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TiendaPapas
{
    internal static class Program
    {
        static List<Sede> sedes = new List<Sede>();
        static List<Producto> productos = new List<Producto>();
        static Carta carta = new Carta(new List<Recetas>());

        static void Main()
        {
            // Carga los datos apenas abre el programa
            CargarProductos();
            CargarSedes();
            CargarCarta();


            while (true)
            {
                Console.WriteLine("\n--- Administrador TiendaPapas ---");
                Console.WriteLine("1. Gestionar Sedes");
                Console.WriteLine("2. Gestionar Empleados");
                Console.WriteLine("3. Gestionar Productos");
                Console.WriteLine("4. Gestionar Carta");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");
                var opt = Console.ReadLine();

                switch (opt)
                {
                    case "1": MenuSedes(); break;
                    case "2": MenuEmpleados(); break;
                    case "3": MenuProductos(); break;
                    case "4": MenuCarta(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        #region Sedes
        static void MenuSedes()
        {
            while (true)
            {
                Console.WriteLine("\n--- Sedes ---");
                Console.WriteLine("1. Crear Sede");
                Console.WriteLine("2. Listar Sedes");
                Console.WriteLine("3. Actualizar Sede");
                Console.WriteLine("4. Buscar Sede");
                Console.WriteLine("5. Eliminar Sede");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var opt = Console.ReadLine();
                switch (opt)
                {
                    case "1": CrearSede(); break;
                    case "2": ListarSedes(); break;
                    case "3": ActualizarSede(); break;
                    case "4": BuscarSede(); break;
                    case "5": EliminarSede(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        static void CrearSede()
        {
            Console.Write("Nombre de la sede: ");
            var nombre = Console.ReadLine();
            Console.Write("Ubicación: ");
            var ubic = Console.ReadLine();
            sedes.Add(new Sede(nombre, ubic));
            Console.WriteLine("Sede creada.");
            GuardarSedes();
        }

        static void ListarSedes()
        {
            if (!sedes.Any()) { Console.WriteLine("No hay sedes."); return; }
            for (int i = 0; i < sedes.Count; i++)
            {
                Console.WriteLine($"{i}: {sedes[i].Nombre} - {sedes[i].Ubicacion} (Empleados: {sedes[i].Empleados.Count})");
            }
        }

        static Sede SeleccionarSede()
        {
            ListarSedes();
            Console.Write("Índice de sede: ");
            if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < sedes.Count) return sedes[idx];
            Console.WriteLine("Índice inválido.");
            return null;
        }

        static void ActualizarSede()
        {
            var s = SeleccionarSede();
            if (s == null) return;
            Console.Write("Nuevo nombre (enter para mantener): ");
            var n = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(n)) s.Nombre = n;
            Console.Write("Nueva ubicación (enter para mantener): ");
            var u = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(u)) s.Ubicacion = u;
            Console.WriteLine("Sede actualizada.");
        }

        static void BuscarSede()
        {
            Console.Write("Nombre a buscar: ");
            var q = Console.ReadLine();
            var found = sedes.Where(x => x.Nombre?.Equals(q, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (!found.Any()) { Console.WriteLine("No encontrada."); return; }
            foreach (var s in found) Console.WriteLine($"{s.Nombre} - {s.Ubicacion}");
        }

        static void EliminarSede()
        {
            var s = SeleccionarSede();
            if (s == null) return;
            sedes.Remove(s);
            Console.WriteLine("Sede eliminada.");
        }
        #endregion

        #region Empleados
        static void MenuEmpleados()
        {
            while (true)
            {
                Console.WriteLine("\n--- Empleados ---");
                Console.WriteLine("1. Crear Empleado (Mesero/Cocinero) en Sede");
                Console.WriteLine("2. Listar Empleados por Sede");
                Console.WriteLine("3. Actualizar Empleado");
                Console.WriteLine("4. Buscar Empleado por ID");
                Console.WriteLine("5. Eliminar Empleado");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var opt = Console.ReadLine();
                switch (opt)
                {
                    case "1": CrearEmpleado(); break;
                    case "2": ListarEmpleadosPorSede(); break;
                    case "3": ActualizarEmpleado(); break;
                    case "4": BuscarEmpleadoPorID(); break;
                    case "5": EliminarEmpleado(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        static void CrearEmpleado()
        {
            var s = SeleccionarSede();
            if (s == null) return;
            Console.Write("Tipo (1=Mesero, 2=Cocinero): ");
            var t = Console.ReadLine();
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine();
            Console.Write("ID (numérico): ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }

            Empleados nuevo = null;
            if (t == "1")
            {
                Console.Write("Mesa asignada (numérica): ");
                if (!int.TryParse(Console.ReadLine(), out int mesa)) mesa = 0;
                nuevo = new Mesero(nombre, id, mesa);
            }
            else
            {
                nuevo = new Cocinero(nombre, id);
            }

            s.AgregarEmpleado(nuevo);
            Console.WriteLine("Empleado agregado.");
            GuardarSedes();
        }

        static void ListarEmpleadosPorSede()
        {
            var s = SeleccionarSede();
            if (s == null) return;
            if (!s.Empleados.Any()) { Console.WriteLine("No hay empleados en esta sede."); return; }
            foreach (var e in s.Empleados)
            {
                Console.WriteLine($"Nombre: {e.Nombre}, ID: {e.ID}, Tipo: {e.GetType().Name}");
            }
        }

        static (Sede sede, Empleados empleado) BuscarEmpleadoGlobalPorID(int id)
        {
            foreach (var s in sedes)
            {
                var e = s.Empleados.FirstOrDefault(x => x.ID == id);
                if (e != null) return (s, e);
            }
            return (null, null);
        }

        static void BuscarEmpleadoPorID()
        {
            Console.Write("ID a buscar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            var (s, e) = BuscarEmpleadoGlobalPorID(id);
            if (e == null) { Console.WriteLine("No encontrado."); return; }
            Console.WriteLine($"Encontrado en {s.Nombre}: {e.Nombre} ({e.GetType().Name})");
        }

        static void ActualizarEmpleado()
        {
            Console.Write("ID de empleado a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            var (s, e) = BuscarEmpleadoGlobalPorID(id);
            if (e == null) { Console.WriteLine("Empleado no encontrado."); return; }
            Console.Write($"Nuevo nombre (actual: {e.Nombre}): ");
            var nn = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nn)) e.Nombre = nn;
            Console.Write($"Nuevo ID (enter para mantener {e.ID}): ");
            var nidStr = Console.ReadLine();
            if (int.TryParse(nidStr, out int nid)) e.ID = nid;

            // Si es Mesero, permitir actualizar mesa (propiedad propia)
            if (e is Mesero m)
            {
                Console.Write($"Mesa asignada (actual: {m.MesaAsignada}): ");
                if (int.TryParse(Console.ReadLine(), out int nm)) m.MesaAsignada = nm;
            }

            Console.WriteLine("Empleado actualizado.");
            GuardarSedes();

        }

        static void EliminarEmpleado()
        {
            Console.Write("ID de empleado a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            var (s, e) = BuscarEmpleadoGlobalPorID(id);
            if (e == null) { Console.WriteLine("Empleado no encontrado."); return; }
            s.EliminarEmpleado(e);
            Console.WriteLine("Empleado eliminado.");
            GuardarSedes();

        }
        #endregion

        #region Productos
        static void MenuProductos()
        {
            while (true)
            {
                Console.WriteLine("\n--- Productos ---");
                Console.WriteLine("1. Crear Producto");
                Console.WriteLine("2. Listar Productos");
                Console.WriteLine("3. Actualizar Producto");
                Console.WriteLine("4. Eliminar Producto");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var opt = Console.ReadLine();
                switch (opt)
                {
                    case "1": CrearProducto(); break;
                    case "2": ListarProductos(); break;
                    case "3": ActualizarProducto(); break;
                    case "4": EliminarProducto(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        static void CrearProducto()
        {
            Console.Write("Cantidad (numérica): ");
            if (!int.TryParse(Console.ReadLine(), out int cant)) cant = 0;
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine();
            Console.Write("Marca: ");
            var marca = Console.ReadLine();
            productos.Add(new Producto(cant, nombre, marca));

            GuardarProductos();
            Console.WriteLine("Producto guardado exitosamente.");
        }

        static void ListarProductos()
        {
            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos en la lista.");
                return;
            }

            for (int i = 0; i < productos.Count; i++)
            {
                // Esto usa el override ToString que pusiste en Producto.cs
                Console.WriteLine($"{i}: {productos[i]}");
            }
        }

        static void ActualizarProducto()
        {
            ListarProductos();
            Console.Write("Índice del producto a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 0 || idx >= productos.Count) { Console.WriteLine("Índice inválido."); return; }
            Console.WriteLine("Las propiedades de Producto no son públicas en la clase actual; para modificar re-crea el producto.");
            Console.Write("Desea reemplazar por uno nuevo? (s/n): ");
            if (Console.ReadLine()?.ToLower() == "s")
            {
                Console.Write("Cantidad (numérica): ");
                if (!int.TryParse(Console.ReadLine(), out int cant)) cant = 0;
                Console.Write("Nombre: ");
                var nombre = Console.ReadLine();
                Console.Write("Marca: ");
                var marca = Console.ReadLine();
                productos[idx] = new Producto(cant, nombre, marca);
                Console.WriteLine("Producto reemplazado.");
            }
        }

        static void EliminarProducto()
        {
            ListarProductos();
            Console.Write("Índice a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 0 || idx >= productos.Count)

            {
                Console.WriteLine("Índice inválido.");
                return;

            }

            productos.RemoveAt(idx);
            GuardarProductos();
            Console.WriteLine("Producto eliminado.");
        }
        #endregion

        #region Carta
        static void MenuCarta()
        {
            while (true)
            {
                Console.WriteLine("\n--- Carta ---");
                Console.WriteLine("1. Mostrar Carta");
                Console.WriteLine("2. Agregar Platillo (por nombre)");
                Console.WriteLine("3. Eliminar Platillo por índice");
                Console.WriteLine("0. Volver");
                Console.Write("Opción: ");
                var opt = Console.ReadLine();
                switch (opt)
                {
                    case "1": carta.MostrarCarta(); break;
                    case "2": AgregarPlatillo(); break;
                    case "3": EliminarPlatillo(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        static void AgregarPlatillo()
        {
            Console.Write("Nombre del platillo: ");
            var nombre = Console.ReadLine();

            // Intentar crear instancia de Recetas por reflexión (constructor no público en el archivo original).
            try
            {
                var tipoReceta = typeof(Recetas);
                var ctor = tipoReceta.GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public, null, new Type[] { typeof(string) }, null);
                if (ctor == null)
                {
                    Console.WriteLine("No es posible crear Recetas porque el constructor no es accesible. Cambia el constructor de Recetas a public o internal con parámetro string.");
                    return;
                }
                var receta = (Recetas)ctor.Invoke(new object[] { nombre });
                carta.AgregarPlatillo(receta);
                Console.WriteLine("Platillo agregado a la carta.");
                GuardarCarta();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear platillo: {ex.Message}");
            }
        }

        static void EliminarPlatillo()
        {
            try
            {
                carta.MostrarCarta();
                Console.Write("Índice a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int idx)) { Console.WriteLine("Índice inválido."); return; }
                // Carta.Plantillos es privada en el archivo? En tu clase es pública `Platillos`, así que la usamos.
                if (idx < 0 || idx >= carta.Platillos.Count) { Console.WriteLine("Índice fuera de rango."); return; }
                carta.EliminarPlatillo(carta.Platillos[idx]);
                Console.WriteLine("Platillo eliminado.");
                GuardarCarta();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        #endregion

        // Método para GUARDAR (Escribir en el disco)
        static void GuardarProductos()
        {
            try
            {
                using (var writer = new StreamWriter("productos.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(productos);
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error al guardar productos: {ex.Message}"); }
        }

        static void CargarProductos()
        {
            if (!File.Exists("productos.csv")) { productos = new List<Producto>(); return; }

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HeaderValidated = null, // ESTO EVITA LA PANTALLA ROJA
                MissingFieldFound = null // ESTO TAMBIÉN
            };

            try
            {
                using (var reader = new StreamReader("productos.csv"))
                using (var csv = new CsvReader(reader, config))
                {
                    productos = csv.GetRecords<Producto>().ToList();
                }
            }
            catch
            {
                Console.WriteLine("Archivo de productos dañado. Iniciando lista vacía.");
                productos = new List<Producto>();
            }
        }



        static void GuardarCarta()
        {
            using (var writer = new StreamWriter("carta.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(carta.Platillos);
            }
        }

        static void CargarCarta()
        {
            if (File.Exists("carta.csv"))
            {
                var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                };
                using (var reader = new StreamReader("carta.csv"))
                using (var csv = new CsvReader(reader, config))
                {
                    var platillosCargados = csv.GetRecords<Recetas>().ToList();
                    foreach (var p in platillosCargados) carta.AgregarPlatillo(p);
                }
            }
        }

        // --- SEDES Y EMPLEADOS ---
        static void GuardarSedes()
        {
            try
            {
                // 1. Guardar Sedes
                using (var writer = new StreamWriter("sedes.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(sedes);
                }

                // 2. Guardar Empleados con su Sede vinculada
                var listaEmpleados = new List<object>();
                foreach (var s in sedes)
                {
                    foreach (var e in s.Empleados)
                    {
                        listaEmpleados.Add(new
                        {
                            SedeNombre = s.Nombre,
                            e.Nombre,
                            e.ID,
                            Tipo = e.GetType().Name,
                            Mesa = (e is Mesero m) ? m.MesaAsignada : 0
                        });
                    }
                }

                using (var writer = new StreamWriter("empleados.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(listaEmpleados);
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error al guardar sedes: {ex.Message}"); }
        }

        static void CargarSedes()
        {
            if (!File.Exists("sedes.csv")) { sedes = new List<Sede>(); return; }

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HeaderValidated = null,
                MissingFieldFound = null
            };

            try
            {
                // 1. Cargar Sedes
                using (var reader = new StreamReader("sedes.csv"))
                using (var csv = new CsvReader(reader, config))
                {
                    sedes = csv.GetRecords<Sede>().ToList();
                }

                // 2. Cargar Empleados
                if (File.Exists("empleados.csv"))
                {
                    using (var reader = new StreamReader("empleados.csv"))
                    using (var csv = new CsvReader(reader, config))
                    {
                        var registros = csv.GetRecords<dynamic>().ToList();
                        foreach (var r in registros)
                        {
                            var d = (IDictionary<string, object>)r;
                            string tipo = d["Tipo"].ToString();
                            string sNombre = d["SedeNombre"].ToString();

                            Empleados nuevo;
                            if (tipo == "Mesero")
                                nuevo = new Mesero(d["Nombre"].ToString(), int.Parse(d["ID"].ToString()), int.Parse(d["Mesa"].ToString()));
                            else
                                nuevo = new Cocinero(d["Nombre"].ToString(), int.Parse(d["ID"].ToString()));

                            sedes.FirstOrDefault(x => x.Nombre == sNombre)?.AgregarEmpleado(nuevo);
                        }
                    }
                }
            }
            catch { sedes = new List<Sede>(); }
        }
    }
    }

    //ajuste