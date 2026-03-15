using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Cocinero : Empleados
    {
        public Cocinero(string nombre, int id) : base(nombre, id)
        {
        }
        public override void Trabajar(Recetas platillo)
        {
            Console.WriteLine($"el chef ya esta elaborando el platillo {platillo}");
        }
    }
}
