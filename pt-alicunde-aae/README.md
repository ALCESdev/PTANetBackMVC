# Proyecto de Prueba Técnica

## Descripción del Proyecto

Este proyecto es parte de una prueba técnica para evaluar las habilidades en desarrollo backend utilizando .NET, C#, y SQL Server. 

La aplicación consume una API externa, almacena los datos en una base de datos SQL Server y expone los datos a través de una API REST.

## Instrucciones Originales de la Prueba

1. Realizar un programa en .NET - C# que cumpla con los siguientes requisitos:
    - Haz un fork de este proyecto
    - Consumir la siguiente API: [https://api.opendata.esett.com/](https://api.opendata.esett.com/). Escoge sólo 1 servicio cualquiera de los proporcionados por la API.
    - Almacenar la información obtenida en la base de datos. (usa SQL Server en contenedor de docker para esto)
    - Implementar un controlador que permita filtrar por Primary Key en la base de datos.
    - Construir una API REST con Swagger que permita visualizar los datos almacenados en la base de datos.
    - Usar contenedores Docker para DDBB y la propia App
    - Usa arquitectura MVC (sólo API imagina que existe un segundo proyecto con el frontend, por tanto las vistas serán DTOs)
    - Haz un pull request con tu nombre completo y comenta lo que creas necesario al evaluador técnico.
    - Elige entre implementar CRUD o CQRS

## Criterios de evaluación:

Se valorará positivamente (pero no es obligatorio cumplir con todos estos puntos):

1. El uso de código limpio y buenas prácticas de programación tanto en el frontend como en el backend.
2. Utilizar código generado a mano en lugar de depender excesivamente de herramientas de generación automática.
3. Hacer commits frecuentes y bien explicados durante el desarrollo.
4. Demostrar conocimientos en patrones de diseño, tanto en el frontend como en el backend.
5. Gestion correcta de los secretos como cadenas de conexión, usuarios, passwords...
6. Uso del inglés en código y comentarios
7. Uso de elementos de monitoreo y observabilidad como ILogger
8. Uso de Eventos
9. Manejo de excepciones con patron monad
10. Pruebas de test

## Tecnologías utilizadas

- .NET - C#
- SQL Server
- MVC

## Configuración y Ejecución del Proyecto

### Requisitos Previos

- [.NET SDK](https://dotnet.microsoft.com/download) instalado en tu máquina.
- [Docker](https://www.docker.com/get-started) instalado y corriendo, con una instancia de SQL Server configurada.

### Gestión de Secretos y Variables de Entorno

Este proyecto utiliza variables de entorno para gestionar la cadena de conexión a la base de datos, lo que permite mantener los datos sensibles seguros y fuera del código.

#### Configuración de la Variable de Entorno

1. **Windows:**
   - Abre una ventana de comandos y ejecuta:
     ```cmd
     setx DB_CONNECTION_STRING "Server=localhost,1433;Database=alicundepruebatecnica;User Id=sa;Password=YourStrong(!)Password;TrustServerCertificate=True;"
     ```

2. **macOS/Linux:**
   - Abre una terminal y ejecuta:
     ```bash
     export DB_CONNECTION_STRING="Server=localhost,1433;Database=alicundepruebatecnica;User Id=sa;Password=YourStrong(!)Password;TrustServerCertificate=True;"
     ```

#### Configuración del Proyecto

1. Clona este repositorio en tu máquina local.
2. Asegúrate de que la variable de entorno `DB_CONNECTION_STRING` esté configurada correctamente.

### Ejecución del Proyecto

1. Abre el proyecto en tu IDE preferido (por ejemplo, Visual Studio o Visual Studio Code).
2. Compila y ejecuta la aplicación usando las herramientas de tu IDE o el comando `dotnet run`.
3. La aplicación se conectará a la base de datos SQL Server configurada y debería funcionar sin problemas.

### Pruebas Unitarias

Este proyecto incluye un conjunto de pruebas unitarias que aseguran el correcto funcionamiento de los métodos principales del servicio `BankService`.

#### Ejecución de las Pruebas

Para ejecutar las pruebas unitarias, usa el siguiente comando en la terminal:

```bash
dotnet test
```
Esto ejecutará todas las pruebas unitarias definidas y mostrará un resumen de los resultados.

#### Descripción de las Pruebas Unitarias

- **FetchAndStoreBanksAsync_ReturnsSuccess_WhenBanksAreFetchedAndStored:** Verifica que los bancos se obtienen y almacenan correctamente cuando la API responde con éxito.
- **FetchAndStoreBanksAsync_ReturnsFailure_WhenApiFails:** Verifica que se maneja correctamente un fallo en la API.
- **GetAllBanksAsync_ReturnsListOfBanks_WhenBanksExistInDatabase:** Asegura que se recupera una lista de bancos cuando existen registros en la base de datos.
- **GetBankByIdAsync_ReturnsBank_WhenBankExists:** Verifica que se devuelve el banco correcto cuando se busca por su ID.
- **GetBankByIdAsync_ReturnsFailure_WhenBankDoesNotExist:** Asegura que se maneja adecuadamente la búsqueda de un banco inexistente.
- **UpdateBankAsync_ReturnsSuccess_WhenBankIsUpdated:** Asegura que un banco existente se actualiza correctamente.
- **DeleteBankAsync_ReturnsSuccess_WhenBankIsDeleted:** Verifica que un banco se elimina correctamente de la base de datos.

### Pruebas y Verificación

Puedes verificar que la aplicación se está conectando correctamente a la base de datos y que las funcionalidades principales funcionan mediante la ejecución de las pruebas unitarias.

Para ejecutar las pruebas unitarias, usa el comando `dotnet test`.

## Notas Técnicas

- **Patrón de Arquitectura:** El proyecto sigue el patrón de arquitectura MVC (Model-View-Controller), aunque en este caso solo se implementa la API (sin vistas).
- **Manejo de Excepciones:** Se ha implementado el patrón Monad para manejar las excepciones en el servicio `BankService`. Esto permite un manejo de errores más limpio y estructurado, facilitando la propagación de mensajes de error detallados sin necesidad de bloques `try-catch` repetitivos.
- **Documentación API:** La API está documentada utilizando Swagger, lo que facilita la prueba y exploración de los endpoints disponibles.
- **Pruebas Unitarias:** El proyecto incluye pruebas unitarias detalladas para el servicio `BankService`, las cuales cubren casos de éxito, fallos y manejo de datos en la base de datos en memoria.

## Consideraciones Finales

Este proyecto ha sido desarrollado para cumplir con los requisitos especificados en la prueba técnica, siguiendo buenas prácticas de desarrollo y asegurando la seguridad de los datos sensibles.

## Comentarios para el revisor

A título personal, he aprendido mucho durante el desarrollo de este proyecto ya que desconocía el patrón monad y me ha parecido muy interesante y útil. También me ha servido para refrescar conceptos con Docker y las pruebas unitarias.


