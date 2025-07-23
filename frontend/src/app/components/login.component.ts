import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faUser,
  faLock,
  faEye,
  faEyeSlash,
  faBolt,
} from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  // Font Awesome icons
  faUser = faUser;
  faLock = faLock;
  faEye = faEye;
  faEyeSlash = faEyeSlash;
  faBolt = faBolt;
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
