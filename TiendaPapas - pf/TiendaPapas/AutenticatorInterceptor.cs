using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaPapas
{
    internal class AutenticatorInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"[log] entrando a: {invocation.Method.Name}");

            try
            {
                invocation.Proceed();
                Console.WriteLine($"[log] metodo ejecutado correctamente: {invocation.Method.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[log] error en: {invocation.Method.Name} - {ex.Message}");
            }

            Console.WriteLine($"[log] saliendo de: {invocation.Method.Name}");
        }
    }
}
