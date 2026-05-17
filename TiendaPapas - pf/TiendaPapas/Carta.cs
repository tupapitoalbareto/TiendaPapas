using System;
using System.Collections.Generic;
using System.Linq;

namespace TiendaPapas
{
    public record Carta(IReadOnlyList<Recetas> Platillos)
    {
        public void MostrarCarta(IReadOnlyList<Recetas> platillos) => platillos.ToList().ForEach(platillo => Console.WriteLine($"Nombre: {platillo.Nombre}"));

        public int TotalPlatillos() => Platillos.Count;
        
        public Carta AgregarPlatillo(Recetas nuevoPlatillo) => this with { Platillos = Platillos.Append(nuevoPlatillo).ToList() };
        public Carta EliminarPlatillo(Recetas platilloAEliminar) => this with { Platillos = Platillos.Where(p => p.Nombre != platilloAEliminar.Nombre).ToList() };
    }
}