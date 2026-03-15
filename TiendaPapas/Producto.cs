using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Producto
    {
        public int Cantidad { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }

        public Producto(int cantidad, string nombre, string marca)
        {
            Cantidad = cantidad;
            Nombre = nombre;
            Marca = marca;
        }
        public override string ToString()
        {
            return $"{Nombre} - cantidad: {Cantidad} - marca: {Marca}";
        }
    }
}
