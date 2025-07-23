import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="login-container">
      <div class="login-content">
        <!-- Logo/Header -->
        <div class="login-header">
          <div class="logo">
            <svg
              class="user-icon"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              <circle
                cx="12"
                cy="7"
                r="4"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
          </div>
          <h1>Sistema de Gestión</h1>
          <p>Empleados y Tiendas</p>
        </div>

        <!-- Formulario de Login -->
        <div class="login-card">
          <div class="card-content">
            <form (ngSubmit)="handleSubmit()" #loginForm="ngForm">
              <div class="form-group">
                <label for="usuario">Usuario</label>
                <div class="input-container">
                  <svg
                    class="input-icon"
                    viewBox="0 0 24 24"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"
                      stroke="currentColor"
                      stroke-width="2"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    />
                    <circle
                      cx="12"
                      cy="7"
                      r="4"
                      stroke="currentColor"
                      stroke-width="2"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    />
                  </svg>
                  <input
                    id="usuario"
                    type="text"
                    placeholder="Ingresa tu usuario"
                    [(ngModel)]="credentials().usuario"
                    name="usuario"
                    required
                    [disabled]="isLoading()"
                  />
                </div>
              </div>

              <div class="form-group">
                <label for="contrasenia">Contraseña</label>
                <div class="input-container">
                  <svg
                    class="input-icon"
                    viewBox="0 0 24 24"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <rect
                      x="3"
                      y="11"
                      width="18"
                      height="11"
                      rx="2"
                      ry="2"
                      stroke="currentColor"
                      stroke-width="2"
                    />
                    <circle
                      cx="12"
                      cy="16"
                      r="1"
                      stroke="currentColor"
                      stroke-width="2"
                    />
                    <path
                      d="M7 11V7a5 5 0 0 1 10 0v4"
                      stroke="currentColor"
                      stroke-width="2"
                    />
                  </svg>
                  <input
                    id="contrasenia"
                    [type]="showPassword() ? 'text' : 'password'"
                    placeholder="Ingresa tu contraseña"
                    [(ngModel)]="credentials().contrasenia"
                    name="contrasenia"
                    required
                    [disabled]="isLoading()"
                  />
                  <button
                    type="button"
                    class="toggle-password"
                    (click)="togglePasswordVisibility()"
                    [disabled]="isLoading()"
                  >
                    <svg
                      *ngIf="!showPassword()"
                      viewBox="0 0 24 24"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"
                        stroke="currentColor"
                        stroke-width="2"
                      />
                      <circle
                        cx="12"
                        cy="12"
                        r="3"
                        stroke="currentColor"
                        stroke-width="2"
                      />
                    </svg>
                    <svg
                      *ngIf="showPassword()"
                      viewBox="0 0 24 24"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94L6.06 6.06"
                        stroke="currentColor"
                        stroke-width="2"
                      />
                      <path
                        d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19l-6.84-6.84"
                        stroke="currentColor"
                        stroke-width="2"
                      />
                      <path
                        d="M1 1l22 22"
                        stroke="currentColor"
                        stroke-width="2"
                      />
                      <path
                        d="M12 14a2 2 0 1 1-2-2"
                        stroke="currentColor"
                        stroke-width="2"
                      />
                    </svg>
                  </button>
                </div>
              </div>

              <div *ngIf="error()" class="error-alert">
                {{ error() }}
              </div>

              <button
                type="submit"
                class="login-button"
                [disabled]="isLoading() || loginForm.invalid"
              >
                {{ isLoading() ? 'Iniciando sesión...' : 'Iniciar Sesión' }}
              </button>
            </form>

            <!-- Credenciales de prueba -->
            <div class="test-credentials">
              <div class="test-header">
                <svg
                  class="zap-icon"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <polygon
                    points="13,2 3,14 12,14 11,22 21,10 12,10 13,2"
                    stroke="currentColor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  />
                </svg>
                <span>Credenciales de prueba:</span>
              </div>

              <div class="test-buttons">
                <button
                  type="button"
                  class="test-button"
                  (click)="setTestCredentials('admin')"
                  [disabled]="isLoading()"
                >
                  <div class="test-button-content">
                    <div class="test-button-title">Administrador</div>
                    <div class="test-button-subtitle">admin / Admin123</div>
                  </div>
                </button>

                <button
                  type="button"
                  class="test-button"
                  (click)="setTestCredentials('manager')"
                  [disabled]="isLoading()"
                >
                  <div class="test-button-content">
                    <div class="test-button-title">Manager</div>
                    <div class="test-button-subtitle">manager / Manager123</div>
                  </div>
                </button>

                <button
                  type="button"
                  class="test-button"
                  (click)="setTestCredentials('empleado')"
                  [disabled]="isLoading()"
                >
                  <div class="test-button-content">
                    <div class="test-button-title">Empleado</div>
                    <div class="test-button-subtitle">ana.login / Ana123</div>
                  </div>
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Footer -->
        <div class="footer">
          © 2024 Sistema de Gestión. Todos los derechos reservados.
        </div>
      </div>
    </div>
  `,
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  credentials = signal({
    usuario: '',
    contrasenia: '',
  });

  showPassword = signal(false);
  error = signal('');
  isLoading = signal(false);

  constructor(private authService: AuthService, private router: Router) {}

  async handleSubmit() {
    this.isLoading.set(true);
    this.error.set('');

    try {
      const result = await this.authService.loginSync(
        this.credentials().usuario,
        this.credentials().contrasenia
      );

      if (result.success) {
        this.router.navigate(['/dashboard']);
      } else {
        this.error.set(result.error || 'Error desconocido');
      }
    } catch (err) {
      this.error.set('Error de conexión');
    } finally {
      this.isLoading.set(false);
    }
  }

  togglePasswordVisibility() {
    this.showPassword.update((show) => !show);
  }

  setTestCredentials(userType: 'admin' | 'manager' | 'empleado') {
    const testCreds = this.authService.getTestCredentials(userType);
    this.credentials.set(testCreds);
    this.error.set('');
  }
}
