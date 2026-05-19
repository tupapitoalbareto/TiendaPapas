using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Recetas
    {
       public List<Producto> Ingredientes { get; set; }
       public string Nombre { get; set; }

        public Recetas(string nombre , List<Producto> Ingredientes)
        {
            Nombre = nombre;
            this.Ingredientes = Ingredientes;
        }
        
        public void ObtenerReceta(List<Producto> bodega)
        {
            int contador = 0;
            for (int j = 0; j < Ingredientes.Count; j++)
            {
                for (int i = 0; i < bodega.Count; i++)
                {
                    if (Ingredientes[j].Nombre == bodega[i].Nombre &&                       
                        Ingredientes[j].Marca == bodega[i].Marca && 
                        Ingredientes[j].Cantidad <= bodega[i].Cantidad)
                    {
                            bodega[i].Cantidad -= Ingredientes[j].Cantidad;
                            Console.WriteLine($"quedan en bodega {bodega[i].Cantidad} unidades de {bodega[i].Nombre} marca {bodega[i].Marca}");
                            contador++;
                    }
                }
            }

            if (contador == Ingredientes.Count )
            {
                Console.WriteLine($"{Nombre} se ha hecho con los ingredientes:");
                for (int i = 0; i < Ingredientes.Count; i++)
                {
                    Console.WriteLine($"{Ingredientes[i].Nombre} - marca: {Ingredientes[i].Marca} - cantidad:{Ingredientes[i].Cantidad}");
                }

            }
            else
            {
                throw new Exception($"No se puede hacer {Nombre} con los ingredientes disponibles.");
                
            }
            
        }
        public override string ToString()
        {
            return Nombre ?? base.ToString();
        }
    }
}
