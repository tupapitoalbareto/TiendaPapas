using System;

namespace TiendaPapas
{
    // Interfaz para autenticación
    public interface IAutentic
    {
        bool Autenticar(string usuario, string contraseña);
    }

    // Implementación
    public class Administrador : IAutentic
    {
        public bool Autenticar(string usuario, string contraseña)
        {
            // Validación simple
            if (usuario == "admin" && contraseña == "123")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}