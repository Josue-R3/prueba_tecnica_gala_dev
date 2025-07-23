import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID, Inject } from '@angular/core';
import { Observable, of, delay } from 'rxjs';

export interface Role {
  id: number;
  nombre: string;
}

export interface Empleado {
  id: number;
  nombre: string;
  apellido: string;
  cargo: string;
}

export interface Usuario {
  id: number;
  usuario: string;
  contrasenia: string;
  rolId: number;
  empleadoId: number | null;
  estado: number;
  fechaCreado: string;
  empleado: Empleado | null;
  rol: Role;
}

// Datos de usuarios de prueba basados en el script SQL
const testUsers: Usuario[] = [
  {
    id: 1,
    usuario: 'admin',
    contrasenia: 'Admin123',
    rolId: 1,
    empleadoId: null,
    estado: 1,
    fechaCreado: '2024-01-01',
    empleado: null,
    rol: { id: 1, nombre: 'Admin' },
  },
  {
    id: 2,
    usuario: 'manager',
    contrasenia: 'Manager123',
    rolId: 2,
    empleadoId: null,
    estado: 1,
    fechaCreado: '2024-01-01',
    empleado: null,
    rol: { id: 2, nombre: 'Manager' },
  },
  {
    id: 3,
    usuario: 'ana.login',
    contrasenia: 'Ana123',
    rolId: 3,
    empleadoId: 1,
    estado: 1,
    fechaCreado: '2024-01-10',
    empleado: { id: 1, nombre: 'Ana', apellido: 'Pérez', cargo: 'Cajera' },
    rol: { id: 3, nombre: 'Empleado' },
  },
];

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSignal = signal<Usuario | null>(null);
  private isAuthenticatedSignal = signal<boolean>(false);

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // Verificar si hay una sesión guardada al inicializar
    this.checkStoredSession();
  }

  get currentUser() {
    return this.currentUserSignal.asReadonly();
  }

  get isAuthenticated() {
    return this.isAuthenticatedSignal.asReadonly();
  }

  private checkStoredSession() {
    if (isPlatformBrowser(this.platformId)) {
      const savedAuth = localStorage.getItem('isAuthenticated');
      const savedUser = localStorage.getItem('currentUser');

      if (savedAuth === 'true' && savedUser) {
        this.isAuthenticatedSignal.set(true);
        this.currentUserSignal.set(JSON.parse(savedUser));
      }
    }
  }

  login(
    usuario: string,
    contrasenia: string
  ): Observable<{ success: boolean; user?: Usuario; error?: string }> {
    // Buscar usuario en los datos de prueba
    const user = testUsers.find(
      (u) =>
        u.usuario === usuario && u.contrasenia === contrasenia && u.estado === 1
    );

    if (user) {
      this.currentUserSignal.set(user);
      this.isAuthenticatedSignal.set(true);
      if (isPlatformBrowser(this.platformId)) {
        localStorage.setItem('isAuthenticated', 'true');
        localStorage.setItem('currentUser', JSON.stringify(user));
      }
      return of({ success: true, user }).pipe(delay(1000));
    } else {
      return of({
        success: false,
        error: 'Usuario o contraseña incorrectos',
      }).pipe(delay(1000));
    }
  }

  // Método sincrónico para login (similar al proyecto de referencia)
  loginSync(
    usuario: string,
    contrasenia: string
  ): Promise<{ success: boolean; user?: Usuario; error?: string }> {
    return new Promise((resolve) => {
      // Simular delay
      setTimeout(() => {
        const user = testUsers.find(
          (u) =>
            u.usuario === usuario &&
            u.contrasenia === contrasenia &&
            u.estado === 1
        );

        if (user) {
          this.currentUserSignal.set(user);
          this.isAuthenticatedSignal.set(true);
          if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem('isAuthenticated', 'true');
            localStorage.setItem('currentUser', JSON.stringify(user));
          }
          resolve({ success: true, user });
        } else {
          resolve({
            success: false,
            error: 'Usuario o contraseña incorrectos',
          });
        }
      }, 1000);
    });
  }

  logout(): void {
    this.currentUserSignal.set(null);
    this.isAuthenticatedSignal.set(false);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('isAuthenticated');
      localStorage.removeItem('currentUser');
    }
  }

  getTestCredentials(userType: 'admin' | 'manager' | 'empleado') {
    const testCreds = {
      admin: { usuario: 'admin', contrasenia: 'Admin123' },
      manager: { usuario: 'manager', contrasenia: 'Manager123' },
      empleado: { usuario: 'ana.login', contrasenia: 'Ana123' },
    };

    return testCreds[userType];
  }
}
