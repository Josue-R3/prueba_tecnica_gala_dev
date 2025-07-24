import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faCheckCircle,
  faExclamationTriangle,
  faInfoCircle,
  faTimes,
  faExclamationCircle,
} from '@fortawesome/free-solid-svg-icons';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  template: `
    <div class="notifications-container">
      @for (notification of notificationService.notifications$(); track
      notification.id) {
      <div class="notification" [ngClass]="'notification-' + notification.type">
        <div class="notification-content">
          <fa-icon
            [icon]="getIcon(notification.type)"
            class="notification-icon"
          ></fa-icon>
          <span class="notification-message">{{ notification.message }}</span>
        </div>
        <button
          class="notification-close"
          (click)="notificationService.removeNotification(notification.id)"
        >
          <fa-icon [icon]="faTimes"></fa-icon>
        </button>
      </div>
      }
    </div>
  `,
  styles: [
    `
      .notifications-container {
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9999;
        max-width: 400px;
      }

      .notification {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 12px 16px;
        margin-bottom: 8px;
        border-radius: 8px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        animation: slideIn 0.3s ease-out;
        min-width: 300px;
      }

      .notification-success {
        background-color: #d4edda;
        border: 1px solid #c3e6cb;
        color: #155724;
      }

      .notification-error {
        background-color: #f8d7da;
        border: 1px solid #f5c6cb;
        color: #721c24;
      }

      .notification-warning {
        background-color: #fff3cd;
        border: 1px solid #ffeaa7;
        color: #856404;
      }

      .notification-info {
        background-color: #d1ecf1;
        border: 1px solid #bee5eb;
        color: #0c5460;
      }

      .notification-content {
        display: flex;
        align-items: center;
        gap: 8px;
        flex-grow: 1;
      }

      .notification-icon {
        font-size: 16px;
      }

      .notification-message {
        font-size: 14px;
        font-weight: 500;
      }

      .notification-close {
        background: none;
        border: none;
        cursor: pointer;
        padding: 4px;
        border-radius: 4px;
        opacity: 0.7;
        transition: opacity 0.2s;
      }

      .notification-close:hover {
        opacity: 1;
      }

      @keyframes slideIn {
        from {
          transform: translateX(100%);
          opacity: 0;
        }
        to {
          transform: translateX(0);
          opacity: 1;
        }
      }
    `,
  ],
})
export class NotificationsComponent {
  faCheckCircle = faCheckCircle;
  faExclamationTriangle = faExclamationTriangle;
  faInfoCircle = faInfoCircle;
  faTimes = faTimes;
  faExclamationCircle = faExclamationCircle;

  constructor(public notificationService: NotificationService) {}

  getIcon(type: string) {
    switch (type) {
      case 'success':
        return this.faCheckCircle;
      case 'error':
        return this.faExclamationCircle;
      case 'warning':
        return this.faExclamationTriangle;
      case 'info':
        return this.faInfoCircle;
      default:
        return this.faInfoCircle;
    }
  }
}
