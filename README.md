# 🏪 Sistema de Gestión de Empleados y Tiendas

## 📋 Descripción del Proyecto

Sistema completo de gestión de empleados y tiendas desarrollado con:

- **Backend**: .NET 8 Web API con Entity Framework y SQL Server
- **Frontend**: Angular 19 con componentes standalone
- **Base de Datos**: SQL Server 2022 en Docker
- **Autenticación**: JWT (JSON Web Tokens)

## 🚀 Instrucciones de Instalación y Ejecución

### Paso 1: 📁 Clonar el Repositorio

```bash
git clone https://github.com/Josue-R3/prueba_tecnica_gala_dev.git
cd prueba_tecnica_gala_dev
```

### Paso 2: 🛠️ Instalación de Prerrequisitos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

#### Herramientas Básicas

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) - Para la base de datos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) - Para el backend
- [Node.js 20+](https://nodejs.org/) con npm - Para el frontend

#### Herramientas de Línea de Comandos

```bash
# Instalar Angular CLI globalmente
npm install -g @angular/cli@19

# Verificar instalaciones
dotnet --version  # Debe mostrar 8.x.x
node --version    # Debe mostrar v20.x.x o superior
ng version        # Debe mostrar Angular CLI 19.x.x
docker --version  # Verificar que Docker esté disponible
```

### Paso 3: 📦 Instalación de Dependencias del Proyecto

#### Backend (.NET)

```bash
cd backend
dotnet restore
cd ..
```

#### Frontend (Angular)

```bash
cd frontend
npm install
cd ..
```

### Paso 4: 🗄️ Levantar la Base de Datos

```bash
# Navegar a la carpeta de base de datos
cd database

# Levantar SQL Server con Docker
docker-compose up -d

# Verificar que esté corriendo
docker-compose ps
```

**Resultado esperado:**

- SQL Server corriendo en `localhost:1433`
- Base de datos `TiendaDB` creada automáticamente con datos de prueba
- Status: `prueba_tecnica_gala_db   Up   0.0.0.0:1433->1433/tcp`

### Paso 5: 💻 Ejecutar el Proyecto

Ahora puedes elegir tu entorno de desarrollo preferido:

---

## 🎯 Opción A: Desarrollo con Visual Studio Code

### 1. Abrir el proyecto en VS Code

```bash
# Desde la raíz del proyecto
code .
```

### 2. Extensiones Recomendadas

VS Code te sugerirá automáticamente instalar las extensiones necesarias:

- **C# Dev Kit** - Soporte para .NET
- **Angular Language Service** - Soporte para Angular
- **Docker** - Gestión de contenedores

### 3. Usar Tasks Integrados

Una vez abierto VS Code, puedes usar los comandos integrados:

- **Ctrl+Shift+P** → `Tasks: Run Task` → `run-backend` (ejecutar backend)
- **Ctrl+Shift+P** → `Tasks: Run Task` → `run-frontend` (ejecutar frontend)
- **F5** → Debug del backend con breakpoints

### 4. Ejecución Manual en VS Code

**Terminal 1 - Backend:**

```bash
cd backend
dotnet build
dotnet run --project Web.API
```

**Terminal 2 - Frontend:**

```bash
cd frontend
ng serve
```

---

## 🎯 Opción B: Desarrollo con Visual Studio

### 1. Abrir la solución

- Navega a la carpeta `backend`
- Abre el archivo `v1.sln` con Visual Studio

### 2. Configurar proyecto de inicio

- En el Solution Explorer, click derecho en `Web.API`
- Seleccionar "Set as Startup Project"

### 3. Ejecutar el backend

- Presiona **F5** o click en "Start Debugging"
- El backend se ejecutará en `http://localhost:5000`

### 4. Ejecutar el frontend (Terminal separado)

Abre una terminal independiente:

```bash
cd frontend
ng serve
```

---

## 🎯 Opción C: Desarrollo Manual con Terminal

### Backend (.NET)

```bash
# Terminal 1
cd backend
dotnet build
dotnet run --project Web.API
```

### Frontend (Angular)

```bash
# Terminal 2
cd frontend
ng serve
```

---

## 🔑 Credenciales de Acceso

El sistema incluye usuarios de prueba predefinidos:

| Usuario     | Contraseña   | Rol           | Descripción                    |
| ----------- | ------------ | ------------- | ------------------------------ |
| `admin`     | `Admin123`   | Administrador | Acceso completo al sistema     |
| `manager`   | `Manager123` | Gerente       | Gestión de empleados y tiendas |
| `ana.login` | `Ana123`     | Empleado      | Empleado Ana Pérez             |

## 🎯 URLs de Acceso

Una vez que todos los servicios estén corriendo:

- 🌐 **Frontend**: http://localhost:4200
- 🔗 **Backend API**: http://localhost:5000
- 📚 **Swagger**: http://localhost:5000/swagger
- 🗄️ **Base de Datos**: localhost:1433 (usuario: `sa`, password: `GalaDb@2025`)

## 📊 Estructura del Proyecto

```
prueba_tecnica_gala_dev/
├── README.md                   # Esta documentación
├── .vscode/                    # Configuración para VS Code
│   └── tasks.json             # Tasks para ejecutar servicios
├── backend/                    # API .NET 8
│   ├── v1.sln                 # Solución de Visual Studio
│   ├── Web.API/               # Controladores y configuración
│   ├── Application/           # Lógica de negocio y servicios
│   ├── Domain/                # Entidades y interfaces
│   └── Infraestructure/       # Acceso a datos y repositorios
├── frontend/                   # Aplicación Angular 19
│   ├── src/app/               # Componentes y servicios
│   ├── package.json           # Dependencias npm
│   └── angular.json           # Configuración Angular
├── database/                   # Base de datos
│   ├── docker-compose.yml     # Configuración Docker para SQL Server
│   └── scripts/
│       └── init.sql           # Script de inicialización con datos
└── docs/                       # Documentación adicional
    └── database.png           # Diagrama de base de datos
```

## 🎯 Funcionalidades Principales

### 👥 Gestión de Empleados

- ✅ Crear, editar, eliminar empleados
- ✅ Listado paginado con búsqueda
- ✅ Asignación a tiendas
- ✅ Validaciones de formulario

### 🏪 Gestión de Tiendas

- ✅ CRUD completo de tiendas
- ✅ Estados activo/inactivo
- ✅ Relación con empleados

### 🔐 Sistema de Autenticación

- ✅ Login con JWT
- ✅ Roles de usuario (Admin, Manager, Empleado)
- ✅ Sesiones persistentes
- ✅ Guards de protección de rutas

## 🌐 Endpoints de la API

### Autenticación

- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/verify` - Verificar token

### Empleados

- `GET /api/empleados` - Listar empleados
- `GET /api/empleados/{id}` - Obtener empleado
- `POST /api/empleados` - Crear empleado
- `PUT /api/empleados/{id}` - Actualizar empleado
- `DELETE /api/empleados/{id}` - Eliminar empleado

### Tiendas

- `GET /api/tiendas` - Listar tiendas
- `GET /api/tiendas/{id}` - Obtener tienda
- `POST /api/tiendas` - Crear tienda
- `PUT /api/tiendas/{id}` - Actualizar tienda
- `DELETE /api/tiendas/{id}` - Eliminar tienda

## 🐞 Solución de Problemas

### Error: "Puerto ya en uso"

```bash
# Windows - matar proceso en puerto específico
netstat -ano | findstr :5000
taskkill /PID [PID_NUMBER] /F

# Alternativamente, cambiar puerto en el backend:
dotnet run --project Web.API --urls=http://localhost:5001
```

### Error: "No se puede conectar a la base de datos"

```bash
# Verificar que Docker esté corriendo
docker ps

# Desde la carpeta database, reiniciar el contenedor
cd database
docker-compose down
docker-compose up -d

# Ver logs para diagnóstico
docker-compose logs sqlserver
```

### Error: Angular "Module not found" o dependencias

```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
```

### Error: "dotnet command not found"

Asegúrate de tener instalado .NET 8 SDK desde https://dotnet.microsoft.com/download

### Error: "ng command not found"

```bash
npm install -g @angular/cli@19
```

## 📝 Comandos Útiles para Desarrollo

### Base de Datos (Docker)

```bash
cd database

# Ver contenedores corriendo
docker-compose ps

# Ver logs de SQL Server
docker-compose logs -f sqlserver

# Detener base de datos
docker-compose down

# Reiniciar con datos limpios
docker-compose down -v && docker-compose up -d
```

### Backend (.NET)

```bash
cd backend

# Limpiar y reconstruir
dotnet clean
dotnet build

# Ejecutar con hot reload (recompila automáticamente)
dotnet watch run --project Web.API

# Ver información del proyecto
dotnet --info
```

### Frontend (Angular)

```bash
cd frontend

# Compilar para producción
ng build

# Ejecutar en modo desarrollo con recarga automática
ng serve --open

# Ejecutar tests
ng test

# Ver versión de Angular
ng version
```

## 🔧 Configuración Técnica

### Puertos Utilizados

- **Base de Datos**: `1433` (SQL Server)
- **Backend API**: `5000` (HTTP)
- **Frontend**: `4200` (HTTP)

### Configuración de CORS

El backend está configurado para aceptar requests desde `http://localhost:4200`

### Variables de Entorno

- Backend: Configurado en `appsettings.json` y `appsettings.Development.json`
- Frontend: Configurado en `src/environments/environment.ts`

## 💡 Tips para Desarrollo

1. **Orden de inicio**: Siempre iniciar primero la base de datos, luego el backend, y finalmente el frontend
2. **Debugging**: Usa las herramientas de desarrollo del navegador para el frontend y Swagger para probar el backend
3. **Hot Reload**: Tanto `dotnet watch run` como `ng serve` recompilan automáticamente al detectar cambios
4. **Logs**: Mantén abiertas las terminales para ver logs en tiempo real

## 🎉 ¡Listo para usar!

Una vez completados todos los pasos, deberías tener:

1. ✅ Base de datos SQL Server corriendo en Docker (puerto 1433)
2. ✅ Backend .NET 8 API funcionando con Swagger (puerto 5000)
3. ✅ Frontend Angular 19 accesible desde el navegador (puerto 4200)
4. ✅ Sistema de autenticación JWT operativo

**Accede al sistema**: http://localhost:4200

**Para detener todo**:

1. Presiona `Ctrl+C` en las terminales del backend y frontend
2. En la carpeta `database`: `docker-compose down`

## 📄 Licencia

Este proyecto está desarrollado como prueba técnica.

## 👨‍💻 Autor

Desarrollado para evaluación técnica - Sistema de gestión empresarial
