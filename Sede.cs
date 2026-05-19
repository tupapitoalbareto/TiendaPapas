using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public class Sede
    {
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public List<Mesero> Meseros { get; set; } = new();
        public List<Cocinero> Cocineros { get; set; } = new();
        public List<Empleado> Empleados { get; set; } = new();

        public event EventHandler<SedeAgregadaEventArgs> SedeAgregada;


        public Sede(string nombre, string ubicacion )
        {
            Nombre = nombre;
            Ubicacion = ubicacion;
           
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
        public void AgregarEmpleado(Empleado emp)
        {
            if (emp is Mesero m)
            {
                Meseros.Add(m);
                Empleados.Add(m);

            }
            else if (emp is Cocinero c)
            {
                Cocineros.Add(c);
                Empleados.Add(c);

            }
        }
        public void EliminarEmpleado(Empleado empleado)
        {
    
                if (empleado == null)
                {
                    throw new Exception("estas eliminando un empleado que no existe");
                }
                else
                {
                    if(empleado is Mesero m)
                    {
                            Meseros.Remove(empleado as Mesero);
                    }
                    else
                    {
                        Cocineros.Remove(empleado as Cocinero);
                    }
                    Empleados.Remove(empleado);
                }
        }
    }
}
