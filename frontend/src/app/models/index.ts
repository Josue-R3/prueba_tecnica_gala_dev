// Interfaces base
export interface BaseResponse<T> {
  data?: T;
  message?: string;
  success: boolean;
}

export interface PagedResult<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// DTOs de Empleado
export interface EmpleadoDto {
  id: number;
  nombre: string;
  apellido: string;
  correo: string;
  cargo: string;
  fechaIngreso: string;
  estado: boolean;
  tiendaId: number;
  tiendaNombre: string;
}

export interface CreateEmpleadoDto {
  nombre: string;
  apellido: string;
  correo: string;
  cargo: string;
  fechaIngreso: string;
  tiendaId: number;
}

export interface UpdateEmpleadoDto {
  id: number;
  nombre: string;
  apellido: string;
  correo: string;
  cargo: string;
  fechaIngreso: string;
  tiendaId: number;
}

// DTOs de Tienda
export interface TiendaDto {
  id: number;
  nombre: string;
  direccion: string;
  estado: boolean;
  empleadosCount: number;
}

export interface CreateTiendaDto {
  nombre: string;
  direccion: string;
}

export interface UpdateTiendaDto {
  id: number;
  nombre: string;
  direccion: string;
}

// Interfaces para la aplicación local
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
  estado: number;
  fechaCreado: string;
  empleado?: EmpleadoDto | null;
  rol?: Role;
}
