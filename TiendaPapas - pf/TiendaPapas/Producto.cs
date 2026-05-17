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

        public event EventHandler<ProductoAgregadoEventArgs> ProductoAgregado;

        public Producto(int cantidad, string nombre, string marca)
        {
            Cantidad = cantidad;
            Nombre = nombre;
            Marca = marca;
        }
        public void AgregarProducto()
        {
            Console.WriteLine("Producto agregado correctamente");

            OnProductoAgregado();
        }

        protected virtual void OnProductoAgregado()
        {
            ProductoAgregado?.Invoke(
                this, new ProductoAgregadoEventArgs(Nombre , Cantidad , Marca)
            );
        }
        public override string ToString()
        {
            return $"{Nombre} - cantidad: {Cantidad} - marca: {Marca}";
        }
    }
}
