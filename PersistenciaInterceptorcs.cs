using Castle.DynamicProxy;
using System;
namespace TiendaPapas
{
    public class PersistenciaInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("[LOG] Iniciando guardado...");
            
            invocation.Proceed();
            
            Console.WriteLine("[LOG] Datos guardados correctamente.");
        }
    }
}
 