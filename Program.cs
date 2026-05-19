using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TiendaPapas
{
    internal static class Program
    {
        static List<Sede> sedes = new List<Sede>();
        static List<Producto> productos = new List<Producto>();
        static Carta carta = new Carta(new List<Recetas>());

        static void Main()
        {
            //autenticacion (POA)
            var container = new WindsorContainer();

            container.Register(
                Component.For<IInterceptor>().ImplementedBy<AutenticatorInterceptor>(),
                Component.For<PersistenciaInterceptor>(),

                Component.For<IAutentic>().ImplementedBy<Administrador>().Interceptors<AutenticatorInterceptor>(),
                Component.For<IPersistencia>().ImplementedBy<PersistenciaService>().Interceptors<PersistenciaInterceptor>()
            );

            var admin = container.Resolve<IAutentic>(); //obtener servicio autenticado
            var persistencia = container.Resolve<IPersistencia>(); //obtener servicio de persistencia

            Console.WriteLine("=== LOGIN ADMINISTRADOR ===");

            Console.Write("Usuario: ");
            string usuario = Console.ReadLine();

            Console.Write("Contraseña: ");
            string password = Console.ReadLine();

            if (!admin.Autenticar(usuario, password))
            {
                Console.WriteLine("Acceso denegado.");
                return;
            }

            Console.WriteLine("Acceso concedido.");

            // Cargar datos al iniciar la aplicación
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
                    case "0":
                        // Guardar todo antes de salir
                        persistencia.GuardarTodos();
                        return;
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
            var nuevaSede = new Sede(nombre, ubic);
            sedes.Add(nuevaSede);
            nuevaSede.SedeAgregada += MostrarMensajeSede;
            nuevaSede.AgregarSede();
            Console.WriteLine("Sede creada.");
        }

        static void MostrarMensajeSede(object sender,SedeAgregadaEventArgs e)
        {
            Console.WriteLine(
                $"EVENTO: Se agregó {e.NombreSede} con ubicación {e.Ubicacion}"
            );
        }

        static void ListarSedes()
        {
            if (!sedes.Any()) { Console.WriteLine("No hay sedes."); return; }
            for (int i = 0; i < sedes.Count; i++)
            {
                Console.WriteLine($"{i}: {sedes[i].Nombre} - {sedes[i].Ubicacion} ");
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

            Mesero meseroNuevo;
            Cocinero cocineroNuevo;

            if (t == "1")
            {
                Console.Write("Mesa asignada (numérica): ");
                if (!int.TryParse(Console.ReadLine(), out int mesa)) mesa = 0;
                meseroNuevo = new Mesero(nombre, id, mesa);
                s.AgregarEmpleado(meseroNuevo);

            }
            else if (t == "2")
            {
                cocineroNuevo = new Cocinero(nombre, id);
                s.AgregarEmpleado(cocineroNuevo);
            }
            else return;
            Console.WriteLine("Empleado agregado.");
        }

        static void ListarEmpleadosPorSede()
        {
            var s = SeleccionarSede();
            if (s == null) return;
            if (!s.Empleados.Any()) { Console.WriteLine("No hay empleados en esta sede."); return; }
            foreach (var e in s.Meseros)
            {
                Console.WriteLine($"Nombre: {e.Nombre}, ID: {e.ID}, Tipo: {e.GetType().Name}");
            }
            foreach (var e in s.Cocineros)
            {
                Console.WriteLine($"Nombre: {e.Nombre}, ID: {e.ID}, Tipo: {e.GetType().Name}");
            }
        }

        static (Sede sede, Empleado empleado) BuscarEmpleadoGlobalPorID(int id)
        {
            foreach (var s in sedes)
            {
                var e = s.Empleados.FirstOrDefault(x => x.ID == id);
                if (e == null) return (null , null);
                if (e is Mesero m) return (s, m);
                if (e is Cocinero c) return (s, c);
            }
            return (null, null);
        }

        static void BuscarEmpleadoPorID()
        {
            Console.Write("ID a buscar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            var (s, e) = BuscarEmpleadoGlobalPorID(id);
            if (e == null) { Console.WriteLine("No encontrado."); return; }
            Console.WriteLine($"Encontrado en {s.Nombre}: {e.Nombre} ({e.GetType().Name}) - ID: {e.ID}");
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
                var nMesero = s.Meseros.Find(e => e.ID == id);
                nMesero.Nombre = nn;
                nMesero.ID = nid;
                nMesero.MesaAsignada = nm;

            }
            else if (e is Cocinero)
            {
                var nCocinero = s.Cocineros.Find(e => e.ID == id);
                nCocinero.Nombre = nn;
                nCocinero.ID = nid;
            }

            Console.WriteLine("Empleado actualizado.");
        }

        static void EliminarEmpleado()
        {
            Console.Write("ID de empleado a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("ID inválido."); return; }
            var (s, e) = BuscarEmpleadoGlobalPorID(id);
            if (e == null) { Console.WriteLine("Empleado no encontrado."); return; }
            s.EliminarEmpleado(e);
            Console.WriteLine("Empleado eliminado.");
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
            var productoNuevo = new Producto(cant, nombre, marca);
            productoNuevo.ProductoAgregado += MostrarMensajeProducto;
            productoNuevo.AgregarProducto(); // Esto disparará el evento|
            productos.Add(productoNuevo);
            Console.WriteLine("Producto creado.");
        }

        static void MostrarMensajeProducto(object sender,ProductoAgregadoEventArgs e)
        {
            Console.WriteLine($"EVENTO: Se agregó {e.NombreProducto} con cantidad {e.Cantidad} y marca {e.Marca}");
        }


        static void ListarProductos()
        {
            if (!productos.Any()) { Console.WriteLine("No hay productos."); return; }
            for (int i = 0; i < productos.Count; i++)
            {
                Console.WriteLine($"{i}: {productos[i].Nombre} - marca: {productos[i].Marca} - cantidad: {productos[i].Cantidad}");
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
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 0 || idx >= productos.Count) { Console.WriteLine("Índice inválido."); return; }
            productos.RemoveAt(idx);
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
                    case "1": carta.MostrarCarta(carta.Platillos); break;
                    case "2": AgregarPlatillo(); break;
                    case "3": EliminarPlatillo(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        static void AgregarPlatillo()
        {
            try
            {
                Console.WriteLine("Nombre del platillo: ");
                var nombre = Console.ReadLine();
                var Ingredientes = new List<Producto>();

                while (true)
                {
                    Console.WriteLine("indique el nombre del producto");
                    string nomb = Console.ReadLine();
                    Console.WriteLine("indique la marca del producto");
                    string marca = Console.ReadLine();
                    Console.WriteLine("indique la cantidad del producto");
                    int cant = int.Parse(Console.ReadLine());
                    Ingredientes.Add(new Producto(cant, nomb, marca));

                    Console.WriteLine("son todos los ingredientes? (s/n)");
                    string respuesta = Console.ReadLine()?.ToLower();
                    if (respuesta == "s") { break; }
                    else if (respuesta == "n")
                    {
                        Console.WriteLine("Continúe añadiendo ingredientes.");
                    }
                    else
                    {
                        Console.WriteLine("escriba un caracter valido");
                    }


                }

                var receta = new Recetas(nombre, Ingredientes);
                receta.ObtenerReceta(productos); // Obtener ingredientes de la bodega
                carta = carta.AgregarPlatillo(receta); //hubo error por ser inmutable, se re-asigna el resultado a carta

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
                carta.MostrarCarta(carta.Platillos);
                Console.Write("Índice a eliminar: ");
                if (!int.TryParse(Console.ReadLine(), out int idx)) { Console.WriteLine("Índice inválido."); return; }
                if (idx < 0 || idx >= carta.Platillos.Count) { Console.WriteLine("Índice fuera de rango."); return; }
                carta = carta.EliminarPlatillo(carta.Platillos[idx]);
                Console.WriteLine("Platillo eliminado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        #endregion

        // --- Persistencia ---

        public static void GuardarProductos()
        {
            try
            {
                using (var writer = new StreamWriter("Producto.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(productos);
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error al guardar productos: {ex.Message}"); }
        }

        public static void CargarProductos()
        {
            if (!File.Exists("Producto.csv")) { productos = new List<Producto>(); return; }

            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HeaderValidated = null, // ESTO EVITA LA PANTALLA ROJA
                MissingFieldFound = null // ESTO TAMBIÉN
            };

            try
            {
                using (var reader = new StreamReader("Producto.csv"))
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

        public static void GuardarSedes()
        {
            try
            {
                // 1. Guardar Sedes
                using (var writer = new StreamWriter("Sede.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(sedes);
                }

                // 2. Guardar Meseros — igual que Cocineros, sobreescribir directo
                using var writer2 = new StreamWriter("Meseros.csv");  // <-- sin Append, sin doble apertura
                using var csv2 = new CsvWriter(writer2, CultureInfo.InvariantCulture);
                var datosGuardados = sedes.SelectMany(s => s.Meseros.Select(m => new
                {
                    Sede = s.Nombre,
                    m.Nombre,
                    m.ID,
                    m.MesaAsignada
                })).ToList();
                csv2.WriteRecords(datosGuardados);

                // 3. Guardar Cocineros
                using var writer3 = new StreamWriter("Cocineros.csv");
                using var csv3 = new CsvWriter(writer3, CultureInfo.InvariantCulture);
                var datosCocineros = sedes.SelectMany(s => s.Cocineros.Select(c => new
                {
                    Sede = s.Nombre,
                    c.Nombre,
                    c.ID
                }));
                csv3.WriteRecords(datosCocineros);
            }
            catch (Exception ex) { Console.WriteLine($"Error al guardar: {ex.Message}"); }
        }

        public static void CargarSedes()
        {
            if (!File.Exists("Sede.csv")) { sedes = new List<Sede>(); return; }

            try
            {
                var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader("Sede.csv"))
                using (var csv = new CsvReader(reader, config))
                {
                    sedes = csv.GetRecords<Sede>().ToList();
                    Console.WriteLine($"SEDES CARGADAS: {sedes.Count}");
                }

                foreach (var s in sedes)
                {
                    s.Empleados = new List<Empleado>();
                    s.Meseros = new List<Mesero>();
                    s.Cocineros = new List<Cocinero>();
                }

                // CARGAR MESEROS  <-- adentro del try
                if (File.Exists("Meseros.csv"))
                {
                    using var readerMeseros = new StreamReader("Meseros.csv");
                    using var csvMeseros = new CsvReader(readerMeseros, config);
                    var registrosMeseros = csvMeseros.GetRecords<MeseroDto>().ToList();
                    foreach (var r in registrosMeseros)
                    {
                        var mesero = new Mesero(r.Nombre, r.ID, r.MesaAsignada);
                        var sede = sedes.FirstOrDefault(s => s.Nombre == r.Sede);
                        if (sede != null) { sede.Meseros.Add(mesero); sede.Empleados.Add(mesero); }
                    }
                    Console.WriteLine($"MESEROS EN TOTAL: {sedes.Sum(s => s.Meseros.Count)}");
                }

                // CARGAR COCINEROS  <-- adentro del try
                if (File.Exists("Cocineros.csv"))
                {
                    using var readerCocineros = new StreamReader("Cocineros.csv");
                    using var csvCocineros = new CsvReader(readerCocineros, config);
                    var registrosCocineros = csvCocineros.GetRecords<CocineroDto>().ToList();
                    foreach (var r in registrosCocineros)
                    {
                        var cocinero = new Cocinero(r.Nombre, r.ID);
                        var sede = sedes.FirstOrDefault(s => s.Nombre == r.Sede);
                        if (sede != null) { sede.Cocineros.Add(cocinero); sede.Empleados.Add(cocinero); }
                    }
                    Console.WriteLine($"COCINEROS EN TOTAL: {sedes.Sum(s => s.Cocineros.Count)}");
                }

            }  // <-- aquí cierra el try, DESPUÉS de los dos if
            catch (Exception ex)
            {
                Console.WriteLine($"Error en carga: {ex}");
            }
        
           
            //if (!File.Exists("Sede.csv")) { sedes = new List<Sede>(); return; }

            //var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    PrepareHeaderForMatch = args => args.Header.ToLower(),
            //    HeaderValidated = null,
            //    MissingFieldFound = null
            //};

            //try
            //{
            //    // 1. Cargar Sedes
            //    using (var reader = new StreamReader("Sede.csv"))
            //    using (var csv = new CsvReader(reader, config))
            //    {
            //        sedes = csv.GetRecords<Sede>().ToList();
            //        Console.WriteLine($"SEDES CARGADAS: {sedes.Count}");
            //    }

            //    foreach (var s in sedes)
            //    {
            //        s.Empleados = new List<Empleado>();
            //        s.Meseros = new List<Mesero>();
            //        s.Cocineros = new List<Cocinero>();
            //    }

            //    // CARGAR MESEROS
            //    if (File.Exists("Meseros.csv"))
            //    {
            //        using var readerMeseros = new StreamReader("Meseros.csv");
            //        using var csvMeseros = new CsvReader(readerMeseros, config);

            //        var registrosMeseros = csvMeseros.GetRecords<dynamic>().ToList();

            //        foreach (var r in registrosMeseros)
            //        {
            //            var d = (IDictionary<string, object>)r;

            //            var mesero = new Mesero(
            //                d["Nombre"].ToString(),
            //                int.Parse(d["ID"].ToString()),
            //                int.Parse(d["MesaAsignada"].ToString())
            //            );

            //            var sede = sedes.FirstOrDefault(
            //                s => s.Nombre == d["Sede"].ToString()
            //            );

            //            if (sede != null)
            //            {
            //                sede.Meseros.Add(mesero);
            //                sede.Empleados.Add(mesero);
            //            }
            //        }
            //        Console.WriteLine($"MESEROS EN TOTAL: {sedes.Sum(s => s.Meseros.Count)}");
            //    }

            //    // CARGAR COCINEROS
            //    if (File.Exists("Cocineros.csv"))
            //    {
            //        using var readerCocineros = new StreamReader("Cocineros.csv");
            //        using var csvCocineros = new CsvReader(readerCocineros, config);

            //        var registrosCocineros = csvCocineros.GetRecords<dynamic>().ToList();

            //        foreach (var r in registrosCocineros)
            //        {
            //            var d = (IDictionary<string, object>)r;

            //            var cocinero = new Cocinero(
            //                d["Nombre"].ToString(),
            //                int.Parse(d["ID"].ToString())
            //            );

            //            var sede = sedes.FirstOrDefault(
            //                s => s.Nombre == d["Sede"].ToString()
            //            );

            //            if (sede != null)
            //            {
            //                sede.Cocineros.Add(cocinero);
            //                sede.Empleados.Add(cocinero);
            //            }
            //        }
            //        Console.WriteLine($"COCINEROS EN TOTAL: {sedes.Sum(s => s.Cocineros.Count)}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error en carga: {ex}");
            //}
        }

        // Persistencia de Carta usando CsvHelper (campo Ingredients serializado)
        public static void GuardarCarta()
        {
            try
            {
                var dtoList = carta.Platillos.Select(r => new RecetaCsvDto
                {
                    Nombre = r.Nombre,
                    Ingredientes = SerializeIngredientes(r.Ingredientes)
                }).ToList();

                using (var writer = new StreamWriter("Carta.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(dtoList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar carta: {ex.Message}");
            }
        }

        public static void CargarCarta()
        {
            try
            {
                if (!File.Exists("Carta.csv")) { carta = new Carta(new List<Recetas>()); return; }

                var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader("Carta.csv"))
                using (var csv = new CsvReader(reader, config))
                {
                    var dtoList = csv.GetRecords<RecetaCsvDto>().ToList();
                    carta = new Carta(new List<Recetas>());
                    foreach (var dto in dtoList)
                    {
                        var ingredientes = DeserializeIngredientes(dto.Ingredientes);
                        var receta = new Recetas(dto.Nombre, ingredientes);
                        carta = carta.AgregarPlatillo(receta);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar carta: {ex.Message}");
                carta = new Carta(new List<Recetas>());
            }
        }

        public static void GuardarTodos()
        {
            GuardarProductos();
            GuardarSedes();
            GuardarCarta();
        }

        // Helpers para serializar ingredientes (uso Base64 para campos de texto)
        static string SerializeIngredientes(List<Producto> ingredientes)
        {
            if (ingredientes == null || ingredientes.Count == 0) return string.Empty;
            // Formato por item: base64(nombre);base64(marca);cantidad   y se separan items por "||"
            return string.Join("||", ingredientes.Select(p => $"{ToBase64(p.Nombre)};{ToBase64(p.Marca)};{p.Cantidad}"));
        }

        static List<Producto> DeserializeIngredientes(string data)
        {
            var list = new List<Producto>();
            if (string.IsNullOrWhiteSpace(data)) return list;
            var items = data.Split(new[] { "||" }, StringSplitOptions.None);
            foreach (var it in items)
            {
                var parts = it.Split(';');
                if (parts.Length < 3) continue;
                if (!int.TryParse(parts[2], out int cant)) continue;
                var nombre = FromBase64(parts[0]);
                var marca = FromBase64(parts[1]);
                list.Add(new Producto(cant, nombre, marca));
            }
            return list;
        }

        static string ToBase64(string s) => Convert.ToBase64String(Encoding.UTF8.GetBytes(s ?? string.Empty));
        static string FromBase64(string b)
        {
            try { return Encoding.UTF8.GetString(Convert.FromBase64String(b ?? "")); }
            catch { return string.Empty; }
        }

        // DTO interno para CsvHelper
        private class RecetaCsvDto
        {
            public string Nombre { get; set; }
            public string Ingredientes { get; set; }
        }
        private class MeseroDto
        {
            public string Sede { get; set; }
            public string Nombre { get; set; }
            public int ID { get; set; }
            public int MesaAsignada { get; set; }
        }

        private class CocineroDto
        {
            public string Sede { get; set; }
            public string Nombre { get; set; }
            public int ID { get; set; }
        }
    }
}


