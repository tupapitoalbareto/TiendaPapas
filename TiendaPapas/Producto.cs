using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Producto
    {
        int Cantidad { get; set; }
        string Nombre { get; set; }
        string Marca { get; set; }

        public Producto(int cantidad, string nombre, string marca)
        {
            Cantidad = cantidad;
            Nombre = nombre;
            Marca = marca;
        }
    }
}
