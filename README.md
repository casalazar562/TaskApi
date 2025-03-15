# API de Gestión de Tareas (.NET)

API RESTful desarrollada con ASP.NET Core para gestionar tareas. Esta API proporciona endpoints para crear, listar, actualizar y eliminar tareas, con la funcionalidad adicional de filtrar tareas por estado.

## Características

- ✅ Operaciones CRUD completas para tareas
- ✅ Filtrado de tareas por estado (pendientes, completadas, todas)
- ✅ Generación automática de IDs
- ✅ Configuración CORS para comunicación con aplicaciones frontend
- ✅ Base de datos en memoria con Entity Framework Core

## Estructura del Proyecto

- **Controllers/TasksController.cs**: Maneja las peticiones HTTP y define los endpoints para las operaciones CRUD
- **Models/Task.cs**: Define la estructura de datos para una tarea
- **Data/TaskDbContext.cs**: Configura el contexto de Entity Framework para la base de datos
- **Program.cs**: Punto de entrada de la aplicación y configuración de servicios

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/) 2019/2022 o [Visual Studio Code](https://code.visualstudio.com/) (opcional)

## Instalación y Ejecución

1. Clona el repositorio o descarga los archivos del proyecto

2. Navega a la carpeta del proyecto:
   ```
   cd TaskApi
   ```

3. Restaura las dependencias:
   ```
   dotnet restore
   ```

4. Ejecuta la aplicación:
   ```
   dotnet run
   ```

   La API estará disponible en `https://localhost:7065` (o el puerto que se configure en tu entorno)

## Endpoints de la API

### Obtener todas las tareas
```
GET /api/Tasks
```
Parámetros de consulta opcionales:
- `filtro`: Filtrar tareas por estado (`pendientes`, `completadas`, `todas`)

### Obtener una tarea específica
```
GET /api/Tasks/{id}
```

### Crear una nueva tarea
```
POST /api/Tasks
```
Cuerpo de la solicitud (JSON):
```json
{
  "titulo": "Nueva tarea",
  "descripcion": "Descripción de la tarea",
  "estado": 0
}
```
Nota: No es necesario incluir el ID, se generará automáticamente.

### Actualizar el estado de una tarea
```
PUT /api/Tasks/{id}/estado
```
Cuerpo de la solicitud: valor numérico (0 para pendiente, 1 para completado)

### Eliminar una tarea
```
DELETE /api/Tasks/{id}
```

## Configuración CORS

La API está configurada para permitir solicitudes desde `http://localhost:4200` (aplicación Angular). Esta configuración se encuentra en el archivo `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", 
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Y más abajo en el código:
app.UseCors("AllowAngularApp");
```

Si necesitas permitir solicitudes desde otros orígenes, modifica la configuración anterior.

## Modelo de datos

El modelo de datos `Task` tiene la siguiente estructura:

```csharp
public class Task
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Titulo { get; set; } = string.Empty;
    
    public string Descripcion { get; set; } = string.Empty;
    
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;
    
    public DateTime FechaCreacion { get; set; }
}

public enum EstadoTarea
{
    Pendiente,
    Completado
}
```

## Solución de problemas comunes

### Error CORS
Si encuentras errores relacionados con CORS:
1. Verifica que la línea `app.UseCors("AllowAngularApp");` esté ubicada antes de `app.UseHttpsRedirection();`
2. Asegúrate de que el origen de tu aplicación frontend coincida con el configurado en CORS

### Error al generar IDs automáticamente
Si encuentras el error "The seed entity for entity type 'Task' cannot be added because a non-zero value is required for property 'Id'":

1. Si estás usando datos semilla, usa valores negativos para los IDs:
   ```csharp
   modelBuilder.Entity<Task>().HasData(
       new Task { 
           Id = -1, 
           Titulo = "Ejemplo", 
           // Otros campos
       }
   );
   ```

2. O elimina completamente los datos semilla:
   ```csharp
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       // Sin datos semilla
   }
   ```

## Desarrollo y pruebas

- Para probar la API, puedes usar [Swagger UI](https://localhost:7065/swagger) que está habilitado en el entorno de desarrollo
- Para pruebas manuales, utiliza herramientas como [Postman](https://www.postman.com/) o [curl](https://curl.se/)

## Tecnologías utilizadas

- ASP.NET Core 6+
- Entity Framework Core
- Base de datos en memoria (para desarrollo y pruebas)
