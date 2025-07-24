import { Injectable, signal } from '@angular/core';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  duration?: number;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private notifications = signal<Notification[]>([]);

  get notifications$() {
    return this.notifications.asReadonly();
  }

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  private addNotification(notification: Omit<Notification, 'id'>): void {
    const id = this.generateId();
    const newNotification: Notification = { ...notification, id };

    this.notifications.update((notifications) => [
      ...notifications,
      newNotification,
    ]);

    // Auto-remover después del tiempo especificado (por defecto 5 segundos)
    const duration = notification.duration || 5000;
    setTimeout(() => {
      this.removeNotification(id);
    }, duration);
  }

  success(message: string, duration?: number): void {
    this.addNotification({ type: 'success', message, duration });
  }

  error(message: string, duration?: number): void {
    this.addNotification({ type: 'error', message, duration });
  }

  warning(message: string, duration?: number): void {
    this.addNotification({ type: 'warning', message, duration });
  }

  info(message: string, duration?: number): void {
    this.addNotification({ type: 'info', message, duration });
  }

  removeNotification(id: string): void {
    this.notifications.update((notifications) =>
      notifications.filter((notification) => notification.id !== id)
    );
  }

  clear(): void {
    this.notifications.set([]);
  }
}
