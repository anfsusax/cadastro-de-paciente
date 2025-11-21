import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface Notification {
  message: string;
  type: 'success' | 'error' | 'info' | 'warning';
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notificationSubject = new BehaviorSubject<Notification | null>(null);
  notification$ = this.notificationSubject.asObservable();

  show(notification: Omit<Notification, 'duration'> & { duration?: number }): void {
    this.notificationSubject.next({
      ...notification,
      duration: notification.duration || 5000 // 5 segundos por padrão
    });
  }

  clear(): void {
    this.notificationSubject.next(null);
  }
}
