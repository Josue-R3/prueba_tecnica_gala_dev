import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';

// Interfaces para autenticación
export interface LoginRequest {
  usuario: string;
  contrasenia: string;
}

export interface LoginResponse {
  success: boolean;
  token?: string;
  user?: AuthUser;
  error?: string;
}

export interface AuthUser {
  id: number;
  usuario: string;
  rolId: number;
  empleadoId: number | null;
  rol: {
    id: number;
    nombre: string;
  };
  empleado?: {
    id: number;
    nombre: string;
    apellido: string;
    cargo: string;
  } | null;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/api/auth`;
  private currentUserSubject = new BehaviorSubject<AuthUser | null>(null);
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  private currentUserSignal = signal<AuthUser | null>(null);
  private isAuthenticatedSignal = signal<boolean>(false);

  constructor(private http: HttpClient) {
    // Verificar si hay un token almacenado al inicializar
    this.checkStoredToken();
  }

  get currentUser$() {
    return this.currentUserSubject.asObservable();
  }

  get isAuthenticated$() {
    return this.isAuthenticatedSubject.asObservable();
  }

  get currentUser() {
    return this.currentUserSignal.asReadonly();
  }

  get isAuthenticated() {
    return this.isAuthenticatedSignal.asReadonly();
  }

  login(usuario: string, contrasenia: string): Observable<LoginResponse> {
    const credentials: LoginRequest = { usuario, contrasenia };

    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap((response) => {
          if (response.success && response.token && response.user) {
            // Guardar token y usuario
            sessionStorage.setItem('authToken', response.token);
            sessionStorage.setItem(
              'currentUser',
              JSON.stringify(response.user)
            );

            // Actualizar estado
            this.currentUserSubject.next(response.user);
            this.isAuthenticatedSubject.next(true);
            this.currentUserSignal.set(response.user);
            this.isAuthenticatedSignal.set(true);
          }
        }),
        catchError((error) => {
          console.error('Error en login:', error);
          return of({
            success: false,
            error: error.error?.error || 'Error de conexión con el servidor',
          });
        })
      );
  }

  // Método sincrónico para compatibilidad
  async loginSync(
    usuario: string,
    contrasenia: string
  ): Promise<{ success: boolean; user?: AuthUser; error?: string }> {
    return new Promise((resolve) => {
      this.login(usuario, contrasenia).subscribe({
        next: (response) =>
          resolve({
            success: response.success,
            user: response.user,
            error: response.error,
          }),
        error: (error) =>
          resolve({
            success: false,
            error: error.message || 'Error de conexión',
          }),
      });
    });
  }

  logout(): void {
    // Limpiar almacenamiento
    sessionStorage.removeItem('authToken');
    sessionStorage.removeItem('currentUser');

    // Actualizar estado
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
    this.currentUserSignal.set(null);
    this.isAuthenticatedSignal.set(false);
  }

  // Verificar token almacenado
  private checkStoredToken(): void {
    const token = sessionStorage.getItem('authToken');
    const userStr = sessionStorage.getItem('currentUser');

    if (token && userStr) {
      try {
        const user = JSON.parse(userStr) as AuthUser;
        this.currentUserSubject.next(user);
        this.isAuthenticatedSubject.next(true);
        this.currentUserSignal.set(user);
        this.isAuthenticatedSignal.set(true);
      } catch (error) {
        console.error('Error parsing stored user:', error);
        this.logout();
      }
    }
  }

  // Obtener token para requests
  getToken(): string | null {
    return sessionStorage.getItem('authToken');
  }

  // Verificar si está autenticado (sincrónico)
  isAuthenticatedSync(): boolean {
    return this.isAuthenticatedSubject.value;
  }

  // Método para obtener credenciales de prueba (solo para referencia)
  getTestCredentials(userType: 'admin' | 'manager' | 'empleado'): {
    usuario: string;
    contrasenia: string;
  } {
    const credentials = {
      admin: { usuario: 'admin', contrasenia: 'Admin123' },
      manager: { usuario: 'manager', contrasenia: 'Manager123' },
      empleado: { usuario: 'ana.login', contrasenia: 'Ana123' },
    };

    return credentials[userType];
  }
}
