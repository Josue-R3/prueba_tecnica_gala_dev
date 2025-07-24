import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  // Clonar la request y agregar headers comunes
  const apiReq = req.clone({
    setHeaders: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
  });

  return next(apiReq).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'Ha ocurrido un error desconocido';

      if (error.error instanceof ErrorEvent) {
        // Error del lado del cliente o de red
        errorMessage = `Error de conexión: ${error.error.message}`;
      } else if (error.status === 0) {
        // Error de conexión (backend no disponible)
        errorMessage =
          'No se puede conectar al servidor. Verifique que el backend esté ejecutándose.';
      } else {
        // Error del lado del servidor
        switch (error.status) {
          case 400:
            if (error.error?.Message) {
              errorMessage = error.error.Message;
            } else if (error.error?.Errors) {
              errorMessage = 'Errores de validación encontrados';
            } else {
              errorMessage = 'Datos de entrada no válidos';
            }
            break;
          case 401:
            errorMessage = 'No autorizado';
            router.navigate(['/login']);
            break;
          case 403:
            errorMessage = 'Acceso denegado';
            break;
          case 404:
            errorMessage = 'Recurso no encontrado';
            break;
          case 500:
            errorMessage =
              'Error interno del servidor. Verifique la conexión a la base de datos.';
            break;
          case 502:
          case 503:
          case 504:
            errorMessage = 'El servidor no está disponible. Intente más tarde.';
            break;
          default:
            errorMessage = `Error del servidor (${error.status}): ${
              error.statusText || 'Error desconocido'
            }`;
        }
      }

      console.error('Error HTTP:', error);
      return throwError(() => new Error(errorMessage));
    })
  );
};
