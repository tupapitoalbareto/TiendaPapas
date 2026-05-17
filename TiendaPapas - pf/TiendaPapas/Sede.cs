using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Sede
    {
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public List<Empleado> Empleados { get; set; }

        public event EventHandler<SedeAgregadaEventArgs> SedeAgregada;


        public Sede(string nombre, string ubicacion)
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
            Empleados = new List<Empleado>();
        }
        public void AgregarSede()
        {
            Console.WriteLine("Sede agregada correctamente");

            OnSedeAgregada();
        }

        protected virtual void OnSedeAgregada()
        {
            SedeAgregada?.Invoke(
                this, new SedeAgregadaEventArgs(Nombre, Ubicacion)
            );
        }
        public void AgregarEmpleado(Empleado empleado)
        {
            Empleados.Add(empleado);
        }
        public void EliminarEmpleado(Empleado empleado)
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
