using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class ProductoAgregadoEventArgs : EventArgs
    {
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string Marca { get; set; }

        public ProductoAgregadoEventArgs(string nombreProducto, int cantidad, string marca)
        {
            NombreProducto = nombreProducto;
            Cantidad = cantidad;
            Marca = marca;
        }
    }
}
