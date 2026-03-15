using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Carta
    {
        public List<Recetas> Platillos { get; set; }
        public Carta(List<Recetas> platillos)
        {
            Platillos = platillos;
        }
        public void AgregarPlatillo(Recetas platillo)
        {
            if (platillo != null)
            {
                Platillos.Add(platillo);
            }
        }
        public void EliminarPlatillo(Recetas platillo)
        {

            if (platillo == null)
            { 
                throw new Exception("estas eliminando un platillo que no existe");
            }else
            {
                Platillos.Remove(platillo);
            }
        }
        public void MostrarCarta()
        {
            Console.WriteLine("Carta de la Tienda de Papas:");
            foreach (var platillo in Platillos)
            {
                Console.WriteLine(platillo);
            }
        }
    }
}
