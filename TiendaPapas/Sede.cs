using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Sede
    {
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public List<Empleados> Empleados { get; set; }

        public Sede(string nombre, string ubicacion)
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
            Empleados = new List<Empleados>();
        }
        public void AgregarEmpleado(Empleados empleado)
        {
            Empleados.Add(empleado);
        }
        public void EliminarEmpleado(Empleados empleado)
        {
    
                if (empleado == null)
                {
                    throw new Exception("estas eliminando un empleado que no existe");
                }
                else
                {
                    Empleados.Remove(empleado);
                }
        }
    }
}
