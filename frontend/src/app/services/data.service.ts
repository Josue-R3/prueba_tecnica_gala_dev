import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

export interface Role {
  id: number;
  nombre: string;
}

export interface Tienda {
  id: number;
  nombre: string;
  direccion: string;
  estado: number; // BIT: 1 = Activo, 0 = Inactivo
}

export interface Empleado {
  id: number;
  nombre: string;
  apellido: string;
  correo: string;
  cargo: string;
  fechaIngreso: string;
  estado: number; // BIT: 1 = Activo, 0 = Inactivo
  tiendaId: number;
}

export interface Usuario {
  id: number;
  usuario: string;
  contrasenia: string;
  rolId: number;
  empleadoId: number | null;
  estado: number; // BIT: 1 = Activo, 0 = Inactivo
  fechaCreado: string;
}

@Injectable({
  providedIn: 'root',
})
export class DataService {
  // Datos de prueba basados exactamente en el script SQL
  private rolesSignal = signal<Role[]>([
    { id: 1, nombre: 'Admin' },
    { id: 2, nombre: 'Manager' },
    { id: 3, nombre: 'Empleado' },
  ]);

  private tiendasSignal = signal<Tienda[]>([
    {
      id: 1,
      nombre: 'Tienda Centro',
      direccion: 'Av. Principal 123',
      estado: 1,
    },
    {
      id: 2,
      nombre: 'Tienda Norte',
      direccion: 'Calle 5 y Av. 8',
      estado: 1,
    },
    {
      id: 3,
      nombre: 'Tienda Cerrada',
      direccion: 'Vía Antigua S/N',
      estado: 0,
    },
  ]);

  private empleadosSignal = signal<Empleado[]>([
    {
      id: 1,
      nombre: 'Ana',
      apellido: 'Pérez',
      correo: 'ana@demo.com',
      cargo: 'Cajera',
      fechaIngreso: '2024-01-10',
      estado: 1,
      tiendaId: 1,
    },
    {
      id: 2,
      nombre: 'Luis',
      apellido: 'Mora',
      correo: 'luis@demo.com',
      cargo: 'Supervisor',
      fechaIngreso: '2023-09-02',
      estado: 1,
      tiendaId: 1,
    },
    {
      id: 3,
      nombre: 'Marta',
      apellido: 'Ríos',
      correo: 'mrios@demo.com',
      cargo: 'Bodega',
      fechaIngreso: '2022-05-21',
      estado: 0,
      tiendaId: 3,
    },
  ]);

  private usuariosSignal = signal<Usuario[]>([
    {
      id: 1,
      usuario: 'admin',
      contrasenia: '********',
      rolId: 1,
      empleadoId: null,
      estado: 1,
      fechaCreado: '2024-01-01',
    },
    {
      id: 2,
      usuario: 'manager',
      contrasenia: '********',
      rolId: 2,
      empleadoId: null,
      estado: 1,
      fechaCreado: '2024-01-01',
    },
    {
      id: 3,
      usuario: 'ana.login',
      contrasenia: '********',
      rolId: 3,
      empleadoId: 1,
      estado: 1,
      fechaCreado: '2024-01-10',
    },
  ]);

  constructor(private http: HttpClient) {}

  get roles() {
    return this.rolesSignal.asReadonly();
  }

  get tiendas() {
    return this.tiendasSignal.asReadonly();
  }

  get empleados() {
    return this.empleadosSignal.asReadonly();
  }

  get usuarios() {
    return this.usuariosSignal.asReadonly();
  }

  // Funciones para obtener nombres
  getTiendaNombre(tiendaId: number): string {
    return (
      this.tiendasSignal().find((tienda) => tienda.id === tiendaId)?.nombre ||
      'Sin asignar'
    );
  }

  getRolNombre(rolId: number): string {
    return (
      this.rolesSignal().find((rol) => rol.id === rolId)?.nombre || 'Sin rol'
    );
  }

  getEmpleadoNombre(empleadoId: number | null): string {
    if (!empleadoId) return 'Sin empleado';
    const empleado = this.empleadosSignal().find(
      (emp) => emp.id === empleadoId
    );
    return empleado
      ? `${empleado.nombre} ${empleado.apellido}`
      : 'Sin empleado';
  }

  // Estadísticas (solo elementos activos - estado = 1)
  getEstadisticas() {
    return {
      totalTiendas: this.tiendasSignal().filter((t) => t.estado === 1).length,
      totalEmpleados: this.empleadosSignal().filter((e) => e.estado === 1)
        .length,
      totalUsuarios: this.usuariosSignal().filter((u) => u.estado === 1).length,
      totalRoles: this.rolesSignal().length,
    };
  }

  // Funciones CRUD para Tiendas
  addTienda(tienda: Omit<Tienda, 'id'>): void {
    const newTienda: Tienda = {
      id: Math.max(...this.tiendasSignal().map((t) => t.id)) + 1,
      ...tienda,
    };
    this.tiendasSignal.update((tiendas) => [...tiendas, newTienda]);
  }

  deleteTienda(id: number): void {
    this.tiendasSignal.update((tiendas) =>
      tiendas.map((tienda) =>
        tienda.id === id ? { ...tienda, estado: 0 } : tienda
      )
    );
  }

  // Funciones CRUD para Empleados
  addEmpleado(empleado: Omit<Empleado, 'id' | 'fechaIngreso'>): void {
    const newEmpleado: Empleado = {
      id: Math.max(...this.empleadosSignal().map((e) => e.id)) + 1,
      fechaIngreso: new Date().toISOString().split('T')[0],
      ...empleado,
    };
    this.empleadosSignal.update((empleados) => [...empleados, newEmpleado]);
  }

  deleteEmpleado(id: number): void {
    this.empleadosSignal.update((empleados) =>
      empleados.map((empleado) =>
        empleado.id === id ? { ...empleado, estado: 0 } : empleado
      )
    );
  }

  // Funciones CRUD para Usuarios
  addUsuario(
    usuario: Omit<Usuario, 'id' | 'fechaCreado' | 'contrasenia'>
  ): void {
    const newUsuario: Usuario = {
      id: Math.max(...this.usuariosSignal().map((u) => u.id)) + 1,
      fechaCreado: new Date().toISOString().split('T')[0],
      contrasenia: '********', // En producción, esto se hashearía
      ...usuario,
    };
    this.usuariosSignal.update((usuarios) => [...usuarios, newUsuario]);
  }

  deleteUsuario(id: number): void {
    this.usuariosSignal.update((usuarios) =>
      usuarios.map((usuario) =>
        usuario.id === id ? { ...usuario, estado: 0 } : usuario
      )
    );
  }

  // Funciones CRUD para Roles
  addRole(role: Omit<Role, 'id'>): void {
    const newRole: Role = {
      id: Math.max(...this.rolesSignal().map((r) => r.id)) + 1,
      ...role,
    };
    this.rolesSignal.update((roles) => [...roles, newRole]);
  }
}
