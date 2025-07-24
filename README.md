# Sistema de Gestión de Empleados y Tiendas

Sistema fullstack desarrollado con Angular 19+ (frontend) y .NET 8 (backend) para gestión de empleados y tiendas con funcionalidades CRUD completas.

## 🏗️ Arquitectura del Proyecto

```
prueba_tecnica_gala_dev/
├── backend/                 # API en .NET 8
│   ├── Domain/             # Entidades y interfaces
│   ├── Application/        # Lógica de negocio, DTOs, servicios
│   ├── Infrastructure/     # Repositorios, Entity Framework
│   ├── Web.API/           # Controladores y configuración API
│   └── README.md          # Documentación específica del backend
├── frontend/              # Frontend en Angular 19+
│   └── src/app/          # Componentes, servicios, guards
├── db/                   # Base de datos SQL Server con Docker
│   ├── docker-compose.yml
│   └── scripts/init.sql  # Script de inicialización
└── docs/                 # Documentación
```

## 🚀 Inicio Rápido

### 1. Base de Datos
```bash
cd db
docker-compose up -d
```

### 2. Backend (.NET 8 API)
```bash
cd backend
dotnet build
cd Web.API
dotnet run
```
La API estará disponible en: `https://localhost:7167/swagger`

### 3. Frontend (Angular 19+)
```bash
cd frontend
npm install
ng serve
```
La aplicación estará disponible en: `http://localhost:4200`

## ✨ Funcionalidades Implementadas

### Backend (API RESTful)
- ✅ **CRUD Empleados**: Crear, leer, actualizar, eliminar empleados
- ✅ **CRUD Tiendas**: Gestión completa de tiendas
- ✅ **Búsqueda**: Por nombre o correo en empleados
- ✅ **Paginación**: Listado paginado de empleados
- ✅ **Validaciones**: FluentValidation para todos los DTOs
- ✅ **Manejo de Errores**: Middleware global de excepciones
- ✅ **Soft Delete**: Los registros se marcan como inactivos
- ✅ **CORS**: Configurado para el frontend Angular
- ✅ **Swagger**: Documentación interactiva de la API

### Frontend (Angular 19+)
- ✅ **Formularios Reactivos**: Con validaciones completas
- ✅ **Componentes Modulares**: Arquitectura limpia y reutilizable
- ✅ **Guards**: Protección de rutas
- ✅ **Servicios HTTP**: Interceptores para manejo centralizado
- ✅ **DTOs**: Tipado fuerte con TypeScript
- ✅ **Manejo de Errores**: Centralizado en servicios

### Base de Datos
- ✅ **SQL Server**: Containerizada con Docker
- ✅ **Relaciones**: Entre empleados, tiendas, usuarios y roles
- ✅ **Índices**: Para optimización de consultas
- ✅ **Datos de Prueba**: Incluidos en el script de inicialización

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM
- **AutoMapper**: Mapeo de objetos
- **FluentValidation**: Validaciones
- **Swagger/OpenAPI**: Documentación
- **SQL Server**: Base de datos

### Frontend
- **Angular 19+**: Framework principal
- **TypeScript**: Lenguaje de programación
- **Reactive Forms**: Formularios reactivos
- **HttpClient**: Cliente HTTP
- **Guards**: Protección de rutas

### Infraestructura
- **Docker**: Containerización de SQL Server
- **Git**: Control de versiones

## 📋 Requisitos Cumplidos

### Backend (.NET - C#) ✅
- [x] API RESTful usando .NET 8
- [x] Arquitectura modular (Domain, Application, Infrastructure, API)
- [x] Base de datos SQL Server con Entity Framework Core
- [x] Manejo de excepciones y validaciones
- [x] DTOs y mapeo con AutoMapper
- [x] Repository Pattern y separación de responsabilidades

### Frontend (Angular 19+) ✅
- [x] CRUD de empleados con formularios reactivos
- [x] Validaciones en formularios
- [x] Listado con opciones de editar/eliminar
- [x] Consumo del backend con HttpClient
- [x] Interceptor para evitar duplicación de código
- [x] Manejo de errores centralizado
- [x] DTOs definidos
- [x] Guards implementados
- [x] Buscador en listado de empleados
- [x] Paginación en listado
- [x] Servicio de notificaciones para mensajes

### Adicionales ✅
- [x] Arquitectura por capas bien definida
- [x] Código limpio y buenas prácticas
- [x] Control de versiones con Git
- [x] Documentación completa
- [x] Docker para base de datos

## 👥 Datos de Prueba

La base de datos incluye datos de ejemplo:

### Tiendas
- Tienda Centro (Activa)
- Tienda Norte (Activa)  
- Tienda Cerrada (Inactiva)

### Empleados
- Ana Pérez (ana@demo.com) - Cajera
- Luis Mora (luis@demo.com) - Supervisor
- Marta Ríos (mrios@demo.com) - Bodega (Inactiva)

### Usuarios
- admin/Admin123 (Rol: Admin)
- manager/Manager123 (Rol: Manager)
- ana.login/Ana123 (Rol: Empleado)

## � Documentación Adicional

- [Backend API Documentation](./backend/README.md)
- [Database Schema](./docs/database.png)

Para levantar el contendor de la base de datos

- docker compose up -d

Para reiniciarlo

- docker compose down -v

- **Administrador**: admin / Admin123
- **Manager**: manager / Manager123  
- **Empleado**: ana.login / Ana123

## 🔧 Tecnologías

- Angular 20
- TypeScript
- Font Awesome (iconos)
- CSS3 (estilos personalizados)

## 📝 Funcionalidades

- ✅ Sistema de login con diferentes roles
- ✅ Panel de administración con tabs
- ✅ Gestión de tiendas, empleados, usuarios y roles
- ✅ Permisos basados en roles
- ✅ Interfaz responsive
- ✅ Iconos Font Awesome integrados

## 🎯 Mantenimiento

Para mantener el código:

1. **Componentes**: Edita los archivos `.ts`, `.html` y `.css` en `components/`
2. **Datos**: Modifica los servicios en `services/`
3. **Rutas**: Cambia la navegación en `app.routes.ts`
4. **Estilos**: Ajusta los estilos en los archivos `.css` individuales o en `styles.css`
