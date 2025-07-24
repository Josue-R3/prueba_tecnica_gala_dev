import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NotificationsComponent } from './components/notifications.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NotificationsComponent],
  template: `
    <router-outlet></router-outlet>
    <app-notifications></app-notifications>
  `,
  styleUrl: './app.css',
})
export class App {
  title = 'Sistema de Gestión';
}
