import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (notification) {
      <div class="notification" [class]="'notification-' + notification.type">
        <div class="notification-content">
          {{ notification.message }}
        </div>
        <button class="close-btn" (click)="close()">&times;</button>
      </div>
    }
  `,
  styles: [`
    .notification {
      position: fixed;
      top: 20px;
      right: 20px;
      padding: 15px 20px;
      border-radius: 4px;
      color: white;
      display: flex;
      align-items: center;
      justify-content: space-between;
      min-width: 300px;
      max-width: 90%;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
      z-index: 1000;
      animation: slideIn 0.3s ease-out;
    }

    .notification-success {
      background-color: #4caf50;
    }

    .notification-error {
      background-color: #f44336;
    }

    .notification-info {
      background-color: #2196f3;
    }

    .notification-warning {
      background-color: #ff9800;
    }

    .close-btn {
      background: none;
      border: none;
      color: white;
      font-size: 20px;
      cursor: pointer;
      margin-left: 15px;
      padding: 0 5px;
    }

    @keyframes slideIn {
      from { transform: translateX(100%); opacity: 0; }
      to { transform: translateX(0); opacity: 1; }
    }
  `]
})
export class NotificationComponent implements OnInit {
  notification: { message: string; type: 'success' | 'error' | 'info' | 'warning' } | null = null;
  private timeoutId: any;

  constructor(private notificationService: NotificationService) {}

  ngOnInit() {
    this.notificationService.notification$.subscribe(notification => {
      this.notification = notification;
      
      if (notification) {
        clearTimeout(this.timeoutId);
        this.timeoutId = setTimeout(() => {
          this.close();
        }, notification.duration);
      }
    });
  }

  close() {
    this.notification = null;
    this.notificationService.clear();
  }
}
