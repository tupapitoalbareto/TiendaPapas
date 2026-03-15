using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Recetas
    {
        List<Producto> Ingredientes { get; set; }
        string Nombre { get; set; }

        Recetas(string nombre)
        {
            Nombre = nombre;
        }
        public void HacerReceta()
        {
            for (int i = 0; i < Ingredientes.Count; i++)
            {
                if (Ingredientes[i] == null )
                {
                    throw new Exception("el ingrediente no existe");
                }
            }
            Console.WriteLine($"{Nombre} se ha hecho con los ingredientes:");
            foreach (var ingrediente in Ingredientes)
            {
                Console.WriteLine(ingrediente);
            }
        }
    }
}
