Descripción del sistema. 

TindaPapas es una aplicación de consola desarrollada en C# y .NET, la cual está enfocada en la gestión y administración de un restaurante de comida rápida, centrado en papas fritas. El sistema permitirá gestionar y administrar la información relacionada con las sedes, empleados, productos entre otros. Consideramos que mientras más tiendas de papas o más sedes se crean en la vida real, más complejo será la administración y gestión de estas. Por ello, este sistema es importante, ya que permite centralizar y manejar las tiendas de una forma mucho más organizada y controlada para un administrador. Este sistema permite el CRUD, crear, buscar, actualizar y eliminar en los siguientes componentes, gestión de sedes, gestión de empleados, gestión de producto y gestión de recetas.


1:

Para este parámetro se decidió reutilizar la estructura, clases y relaciones establecidas previamente en el taller numero 2 



2:

El proyecto implementa la programación orientada a aspectos utilizando el Castle Windsor y Castle Dynamic Proxy, configurados y funcionando. 

Las interfaces implementadas fueron IAutentic e IPersistencia 


En el caso de IAutentic se implementó un interceptor llamado AutenticatorInterceptor que realiza una autenticación automática al iniciar el programa, en donde el administrador tendrá que registrarse con el usuario y con su contraseña. Si esta persona ingresa valores erróneos en su autenticación, el sistema no lo dejará acceder y le denegará su entrada.


Y para IPersistencia se implementó un interceptor llamado PersistenciaInterceptor el cual conserva la persistencia del programa para producto y sedes 


3:

El sistema incorpora elementos de programación funcional aplicados sobre el dominio del negocio.

Uso de LINQ

Se implementaron consultas utilizando: Where, Select, Aggregate

Ejemplos: Filtrado de productos con bajo stock, Obtención de nombres de productos, Cálculo del total de inventario


Funciones puras

Se desarrollaron funciones encargadas únicamente de procesar información sin modificar el estado del sistema.

Funciones de alto orden

Se utilizaron delegados Func<> y Action<> para manejar operaciones y comportamientos reutilizables.

Tipo inmutable

La clase Carta fue implementada como un record, permitiendo trabajar con un objeto inmutable alineado con el paradigma funcional.



4:

Programación Orientada a Eventos

El sistema implementa eventos personalizados en C# para representar cambios importantes dentro del dominio de negocio.

Eventos implementados
ProductoAgregado

Se dispara automáticamente cuando un nuevo producto es agregado al inventario.

SedeCreada

Se ejecuta cuando una nueva sede es registrada dentro del sistema.

Características de los eventos
Uso de event, Uso de EventHandler, Implementación de EventArgs personalizados, Reacciones automáticas mediante eventos




Aplicación de Principios SOLID

SRP - Single Responsibility Principle

Cada clase posee una responsabilidad específica dentro del sistema.

OCP - Open/Closed Principle

El sistema puede extenderse mediante nuevas clases derivadas sin modificar las existentes.

LSP - Liskov Substitution Principle

Las clases Mesero y Cocinero pueden sustituir correctamente a la clase base Empleado.

ISP - Interface Segregation Principle

Las interfaces fueron diseñadas con responsabilidades específicas evitando métodos innecesarios.

DIP - Dependency Inversion Principle

Los servicios dependen de abstracciones mediante interfaces y son resueltos usando Inyección de Dependencias.


