using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    public abstract class Empleado

    {
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {

                char[] descompuesto = value.ToCharArray();
                for (int i = 0; i < descompuesto.Length; i++)
                {
                    if ((int)descompuesto[i] < 65 && (int)descompuesto[i] >= 122)
                    {
                        throw new ArgumentException("El nombre no puede contener caracteres especiales o numeros");
                    }
                }
                nombre = value;
            }
        }
        public int ID { get; set; }

        public Empleado(string nombre, int id)
        {
            Nombre = nombre;
            ID = id;
        }

    }
}
