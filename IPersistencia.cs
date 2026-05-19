using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public interface IPersistencia
    {
        void GuardarTodos();
    }

        public class PersistenciaService : IPersistencia
        {
            public void GuardarTodos()
            {
                Console.WriteLine("Guardando datos...");

                // Aquí llaman sus métodos existentes
                Program.GuardarProductos();
                Program.GuardarSedes();
                Program.GuardarCarta();
            }
        }
}

