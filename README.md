Para levantar el contendor de la base de datos

- docker compose up -d

Para reiniciarlo

- docker compose down -v



# Sistema de Gestión de Tiendas - Frontend

Este es un proyecto Angular que implementa un sistema de gestión de tiendas con autenticación y manejo de empleados.

## 📁 Estructura del Proyecto (Solo archivos importantes)

```
frontend/src/
├── app/
│   ├── components/           # Componentes de la aplicación
│   │   ├── login.component.*     # Pantalla de login
│   │   └── dashboard.component.* # Panel principal
│   ├── services/            # Servicios de datos
│   │   ├── auth.service.ts      # Manejo de autenticación
│   │   └── data.service.ts      # Manejo de datos (tiendas, empleados, etc.)
│   ├── guards/              # Guards de protección de rutas
│   │   └── auth.guard.ts        # Protección de rutas autenticadas
│   ├── app.routes.ts        # Configuración de rutas
│   ├── app.config.ts        # Configuración principal
│   └── app.ts              # Componente raíz
├── index.html              # Página principal
├── main.ts                 # Punto de entrada
└── styles.css             # Estilos globales
```

## 🚀 Comandos Principales

```bash
# Instalar dependencias
npm install

# Ejecutar en desarrollo
ng serve

# Compilar para producción
ng build
```

## 👥 Usuarios de Prueba

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
