using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Recetas
    {
        List<string> Ingredientes { get; set; }
        string Nombre { get; set; }

        Recetas(string nombre)
        {
            Nombre = nombre;
        }
        public void HacerReceta()
        {
            Console.WriteLine($"{Nombre} se ha hecho con los ingredientes:");
            foreach (var ingrediente in Ingredientes)
            {
                Console.WriteLine(ingrediente);
            }
        }
    }
}
