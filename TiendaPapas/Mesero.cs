using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Mesero : Empleados
    {
        public int MesaAsignada { get; set; }
        public Mesero(string nombre , int id , int MesaAsignada) : base(nombre , id) { 
            this.MesaAsignada = MesaAsignada;
        }
        public override void Trabajar(Recetas platillo)
        {
            Console.WriteLine($"El mesero {Nombre} (ID: {ID}) está atendiendo la mesa {MesaAsignada} la cual tie eun pedido de {platillo}");
        }
    }
}
