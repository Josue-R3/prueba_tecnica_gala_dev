import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, forkJoin } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { EmpleadoService } from './empleado.service';
import { TiendaService } from './tienda.service';
import { NotificationService } from './notification.service';
import { ErrorHandlerService } from './error-handler.service';
import {
  EmpleadoDto,
  CreateEmpleadoDto,
  UpdateEmpleadoDto,
  TiendaDto,
  CreateTiendaDto,
  UpdateTiendaDto,
} from '../models';

export interface Role {
  id: number;
  nombre: string;
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
  // Signals para datos locales (roles y usuarios - no hay API aún)
  private rolesSignal = signal<Role[]>([
    { id: 1, nombre: 'Admin' },
    { id: 2, nombre: 'Manager' },
    { id: 3, nombre: 'Empleado' },
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

  // Signals para datos de API
  private empleadosFromApi = signal<EmpleadoDto[]>([]);
  private tiendasFromApi = signal<TiendaDto[]>([]);

  constructor(
    private http: HttpClient,
    private empleadoService: EmpleadoService,
    private tiendaService: TiendaService,
    private notificationService: NotificationService,
    private errorHandler: ErrorHandlerService
  ) {
    // Cargar datos iniciales
    this.loadInitialData();
  }

  // Computed properties
  get roles() {
    return this.rolesSignal.asReadonly();
  }

  get usuarios() {
    return this.usuariosSignal.asReadonly();
  }

  get empleados() {
    return this.empleadosFromApi.asReadonly();
  }

  get tiendas() {
    return this.tiendasFromApi.asReadonly();
  }

  // Cargar datos iniciales
  private loadInitialData(): void {
    forkJoin({
      empleados: this.empleadoService.getAll().pipe(
        catchError((error) => {
          this.errorHandler.handleError(error);
          return of([]);
        })
      ),
      tiendas: this.tiendaService.getAll().pipe(
        catchError((error) => {
          this.errorHandler.handleError(error);
          return of([]);
        })
      ),
    }).subscribe({
      next: ({ empleados, tiendas }) => {
        this.empleadosFromApi.set(empleados);
        this.tiendasFromApi.set(tiendas);
        if (empleados.length > 0 && tiendas.length > 0) {
          this.errorHandler.showInfo('Datos cargados correctamente');
        }
      },
      error: (error) => {
        this.errorHandler.handleError(error);
      },
    });
  }

  // Método público para recargar datos
  refreshData(): void {
    this.loadInitialData();
  }

  // Funciones para obtener nombres
  getTiendaNombre(tiendaId: number): string {
    return (
      this.tiendasFromApi().find((tienda) => tienda.id === tiendaId)?.nombre ||
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
    const empleado = this.empleadosFromApi().find(
      (emp) => emp.id === empleadoId
    );
    return empleado
      ? `${empleado.nombre} ${empleado.apellido}`
      : 'Sin empleado';
  }

  // Estadísticas (solo elementos activos)
  getEstadisticas() {
    return {
      totalTiendas: this.tiendasFromApi().filter((t) => t.estado).length,
      totalEmpleados: this.empleadosFromApi().filter((e) => e.estado).length,
      totalUsuarios: this.usuariosSignal().filter((u) => u.estado === 1).length,
      totalRoles: this.rolesSignal().length,
    };
  }

  // CRUD para Tiendas (usando API)
  addTienda(tienda: CreateTiendaDto): Observable<TiendaDto> {
    return this.tiendaService.create(tienda).pipe(
      tap((nuevaTienda) => {
        this.tiendasFromApi.update((tiendas) => [...tiendas, nuevaTienda]);
        this.notificationService.success('Tienda creada exitosamente');
      }),
      catchError((error) => {
        this.notificationService.error('Error al crear la tienda');
        throw error;
      })
    );
  }

  deleteTienda(id: number): Observable<void> {
    return this.tiendaService.delete(id).pipe(
      tap(() => {
        this.tiendasFromApi.update((tiendas) =>
          tiendas.map((tienda) =>
            tienda.id === id ? { ...tienda, estado: false } : tienda
          )
        );
        this.notificationService.success('Tienda eliminada exitosamente');
      }),
      catchError((error) => {
        this.notificationService.error('Error al eliminar la tienda');
        throw error;
      })
    );
  }

  // CRUD para Empleados (usando API)
  addEmpleado(empleado: CreateEmpleadoDto): Observable<EmpleadoDto> {
    return this.empleadoService.create(empleado).pipe(
      tap((nuevoEmpleado) => {
        this.empleadosFromApi.update((empleados) => [
          ...empleados,
          nuevoEmpleado,
        ]);
        this.notificationService.success('Empleado creado exitosamente');
      }),
      catchError((error) => {
        this.notificationService.error('Error al crear el empleado');
        throw error;
      })
    );
  }

  deleteEmpleado(id: number): Observable<void> {
    return this.empleadoService.delete(id).pipe(
      tap(() => {
        this.empleadosFromApi.update((empleados) =>
          empleados.map((empleado) =>
            empleado.id === id ? { ...empleado, estado: false } : empleado
          )
        );
        this.notificationService.success('Empleado eliminado exitosamente');
      }),
      catchError((error) => {
        this.notificationService.error('Error al eliminar el empleado');
        throw error;
      })
    );
  }

  // CRUD para Usuarios (local - sin API aún)
  addUsuario(
    usuario: Omit<Usuario, 'id' | 'fechaCreado' | 'contrasenia'>
  ): void {
    const newUsuario: Usuario = {
      id: Math.max(...this.usuariosSignal().map((u) => u.id)) + 1,
      fechaCreado: new Date().toISOString().split('T')[0],
      contrasenia: '********',
      ...usuario,
    };
    this.usuariosSignal.update((usuarios) => [...usuarios, newUsuario]);
    this.notificationService.success('Usuario creado exitosamente');
  }

  deleteUsuario(id: number): void {
    this.usuariosSignal.update((usuarios) =>
      usuarios.map((usuario) =>
        usuario.id === id ? { ...usuario, estado: 0 } : usuario
      )
    );
    this.notificationService.success('Usuario eliminado exitosamente');
  }

  // CRUD para Roles (local - sin API aún)
  addRole(role: Omit<Role, 'id'>): void {
    const newRole: Role = {
      id: Math.max(...this.rolesSignal().map((r) => r.id)) + 1,
      ...role,
    };
    this.rolesSignal.update((roles) => [...roles, newRole]);
    this.notificationService.success('Rol creado exitosamente');
  }
}
