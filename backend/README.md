# Sistema de Gestión de Empleados y Tiendas - Backend

Este es el backend del sistema de gestión de empleados y tiendas desarrollado con .NET 8 y arquitectura Clean.

## Arquitectura

El proyecto sigue los principios de Clean Architecture con las siguientes capas:

- **Domain**: Contiene las entidades, interfaces y reglas de negocio
- **Application**: Contiene los casos de uso, DTOs, servicios y validaciones
- **Infrastructure**: Contiene la implementación de la persistencia con Entity Framework Core
- **Web.API**: Contiene los controladores y configuración de la API

## Tecnologías Utilizadas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acceso a datos
- **SQL Server**: Base de datos
- **AutoMapper**: Mapeo entre entidades y DTOs
- **FluentValidation**: Validaciones
- **Swagger**: Documentación de API

## Prerequisitos

- .NET 8 SDK
- SQL Server o SQL Server LocalDB
- Visual Studio 2022 o VS Code

## Configuración

1. **Clonar el repositorio**
   ```bash
   git clone [url-del-repositorio]
   cd prueba_tecnica_gala_dev/backend
   ```

2. **Configurar la cadena de conexión**
   
   Editar `appsettings.json` y `appsettings.Development.json` para configurar la conexión a tu base de datos SQL Server:
   
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StoreManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

4. **Ejecutar migraciones de base de datos**
   ```bash
   dotnet ef migrations add InitialCreate --project Infraestructure --startup-project Web.API
   dotnet ef database update --project Infraestructure --startup-project Web.API
   ```

5. **Ejecutar la aplicación**
   ```bash
   cd Web.API
   dotnet run
   ```

   O usar Visual Studio y ejecutar el proyecto Web.API

## Endpoints Disponibles

### Empleados
- `GET /api/employees` - Obtener todos los empleados
- `GET /api/employees/paged?page=1&pageSize=10&searchTerm=` - Obtener empleados paginados
- `GET /api/employees/{id}` - Obtener empleado por ID
- `GET /api/employees/search?searchTerm=` - Buscar empleados
- `POST /api/employees` - Crear nuevo empleado
- `PUT /api/employees/{id}` - Actualizar empleado
- `DELETE /api/employees/{id}` - Eliminar empleado

### Tiendas
- `GET /api/stores` - Obtener todas las tiendas
- `GET /api/stores/paged?page=1&pageSize=10` - Obtener tiendas paginadas
- `GET /api/stores/{id}` - Obtener tienda por ID
- `POST /api/stores` - Crear nueva tienda
- `PUT /api/stores/{id}` - Actualizar tienda
- `DELETE /api/stores/{id}` - Eliminar tienda

## Documentación de API

Una vez que la aplicación esté ejecutándose, puedes acceder a la documentación de Swagger en:
- `https://localhost:7xxx/swagger` (HTTPS)
- `http://localhost:5xxx/swagger` (HTTP)

## Estructura del Proyecto

```
backend/
├── Domain/
│   ├── Entities/          # Entidades del dominio
│   ├── Enums/            # Enumeraciones
│   ├── Interfaces/       # Interfaces del dominio
│   └── Common/           # Clases base comunes
├── Application/
│   ├── DTOs/             # Data Transfer Objects
│   ├── Interfaces/       # Interfaces de servicios
│   ├── Services/         # Servicios de aplicación
│   ├── Validators/       # Validaciones con FluentValidation
│   ├── Mappings/         # Perfiles de AutoMapper
│   └── Exceptions/       # Excepciones personalizadas
├── Infrastructure/
│   ├── Data/             # DbContext y configuraciones
│   └── Repositories/     # Implementaciones de repositorios
└── Web.API/
    ├── Controllers/      # Controladores de API
    ├── Middleware/       # Middleware personalizado
    └── appsettings.json  # Configuración
```

## Características Implementadas

### Funcionalidades Core
- ✅ CRUD completo de empleados
- ✅ CRUD completo de tiendas
- ✅ Paginación
- ✅ Búsqueda de empleados por nombre o email
- ✅ Validaciones con FluentValidation
- ✅ Manejo de errores centralizado
- ✅ Repository Pattern con Unit of Work
- ✅ DTOs y mapeo con AutoMapper

### Arquitectura y Buenas Prácticas
- ✅ Clean Architecture
- ✅ Separación de responsabilidades
- ✅ Dependency Injection
- ✅ Global Error Handling
- ✅ CORS configurado para Angular
- ✅ Swagger para documentación

## Base de Datos

### Tabla Employees
- Id (int, PK)
- FirstName (nvarchar(50))
- LastName (nvarchar(50))
- Email (nvarchar(100), unique)
- Position (nvarchar(100))
- HireDate (datetime2)
- Status (int) - 0: Inactive, 1: Active
- CreatedAt (datetime2)
- UpdatedAt (datetime2, nullable)

### Tabla Stores
- Id (int, PK)
- Name (nvarchar(100))
- Address (nvarchar(200))
- Status (int) - 0: Inactive, 1: Active
- CreatedAt (datetime2)
- UpdatedAt (datetime2, nullable)

## Comandos Útiles

### Entity Framework
```bash
# Agregar nueva migración
dotnet ef migrations add [NombreMigración] --project Infraestructure --startup-project Web.API

# Actualizar base de datos
dotnet ef database update --project Infraestructure --startup-project Web.API

# Eliminar última migración
dotnet ef migrations remove --project Infraestructure --startup-project Web.API
```

### Construcción y Ejecución
```bash
# Construir solución
dotnet build

# Ejecutar tests (cuando se implementen)
dotnet test

# Publicar aplicación
dotnet publish -c Release
```

## Próximas Mejoras (Opcionales)

- [ ] Autenticación JWT
- [ ] Pruebas unitarias
- [ ] Logging avanzado
- [ ] Cache con Redis
- [ ] Rate limiting
- [ ] Health checks
