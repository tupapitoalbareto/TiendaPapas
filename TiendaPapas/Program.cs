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

            Empleado nuevo = null;
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

            static (Sede sede, Empleado empleado) BuscarEmpleadoGlobalPorID(int id)
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
                productos.Add(new Producto(cant, nombre, marca));
                Console.WriteLine("Producto creado.");
            }

            static void ListarProductos()
            {
                if (!productos.Any()) { Console.WriteLine("No hay productos."); return; }
                for (int i = 0; i < productos.Count; i++)
                {
                    // Las propiedades de Producto no son públicas en el archivo; se muestra el tipo y el índice.
                    Console.WriteLine($"{i}: {productos[i].GetType().Name}");
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
                    if (Console.ReadLine()?.ToLower() == "s") { break; }
                    else if (Console.ReadLine()?.ToLower() == "n") {
                        Console.WriteLine("ingredientes añadidos correctamente"); 
                    }
                    else {
                        Console.WriteLine("escriba un caracter valido");
                    }
                    
                   
                }

                var receta = new Recetas(nombre, Ingredientes);
                receta.ObtenerReceta(productos); // Obtener ingredientes de la bodega
                carta.AgregarPlatillo(receta);

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
    }
            
            #endregion
}
    
           


