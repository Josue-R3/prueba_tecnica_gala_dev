import { Injectable } from '@angular/core';
import { NotificationService } from './notification.service';

export interface AppError {
  type: 'connection' | 'server' | 'validation' | 'auth' | 'unknown';
  message: string;
  details?: any;
}

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  constructor(private notificationService: NotificationService) {}

  handleError(error: any): AppError {
    let appError: AppError;

    if (error?.message) {
      if (error.message.includes('No se puede conectar al servidor')) {
        appError = {
          type: 'connection',
          message: 'Sin conexión al servidor',
          details:
            'Verifique que el backend esté ejecutándose en https://localhost:7167',
        };
      } else if (error.message.includes('Error interno del servidor')) {
        appError = {
          type: 'server',
          message: 'Error del servidor',
          details:
            'Posible problema con la base de datos o configuración del servidor',
        };
      } else if (error.message.includes('No autorizado')) {
        appError = {
          type: 'auth',
          message: 'Sesión expirada',
          details: 'Por favor, inicie sesión nuevamente',
        };
      } else if (error.message.includes('validación')) {
        appError = {
          type: 'validation',
          message: 'Datos no válidos',
          details: error.message,
        };
      } else {
        appError = {
          type: 'unknown',
          message: 'Error inesperado',
          details: error.message,
        };
      }
    } else {
      appError = {
        type: 'unknown',
        message: 'Error desconocido',
        details: 'Ha ocurrido un error inesperado',
      };
    }

    // Mostrar notificación automáticamente
    this.showErrorNotification(appError);

    // Log para debugging
    console.error('Error manejado:', appError, error);

    return appError;
  }

  private showErrorNotification(error: AppError): void {
    switch (error.type) {
      case 'connection':
        this.notificationService.error(
          '🔌 Sin conexión - Verifique que el backend esté ejecutándose',
          8000
        );
        break;
      case 'server':
        this.notificationService.error(
          '⚠️ Error del servidor - Verifique la base de datos',
          6000
        );
        break;
      case 'auth':
        this.notificationService.warning(
          '🔐 Sesión expirada - Inicie sesión nuevamente',
          5000
        );
        break;
      case 'validation':
        this.notificationService.warning(`📝 ${error.message}`, 4000);
        break;
      default:
        this.notificationService.error(`❌ ${error.message}`, 5000);
    }
  }

  // Método para mostrar mensajes de éxito
  showSuccess(message: string): void {
    this.notificationService.success(`✅ ${message}`, 3000);
  }

  // Método para mostrar información
  showInfo(message: string): void {
    this.notificationService.info(`ℹ️ ${message}`, 4000);
  }
}
