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
                        Ingredientes[j].Cantidad <= bodega[i].Cantidad && 
                        Ingredientes[j].Marca == bodega[i].Marca)
                    {
                        contador++;
                    }
                }
            }

            if (contador == Ingredientes.Count )
            {
                Console.WriteLine($"{Nombre} se ha hecho con los ingredientes:");
                
            }
            else
            {
                Console.WriteLine($"No se puede hacer {Nombre} con los ingredientes disponibles.");
                return;
            }
            
        }
        public override string ToString()
        {
            return Nombre ?? base.ToString();
        }
    }
}
