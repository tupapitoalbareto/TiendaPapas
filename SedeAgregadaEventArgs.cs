using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class SedeAgregadaEventArgs : EventArgs
    {
        public string NombreSede { get; set; }
        public string Ubicacion { get; set; }
        public SedeAgregadaEventArgs(string nombreSede, string ubicacion)
        {
            NombreSede = nombreSede;
            Ubicacion = ubicacion;
        }
    }
}
