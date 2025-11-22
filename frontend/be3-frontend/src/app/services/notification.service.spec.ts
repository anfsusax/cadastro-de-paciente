import { TestBed } from '@angular/core/testing';
import { NotificationService } from './notification.service';

describe('NotificationService', () => {
  let service: NotificationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NotificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should show notification with default duration', (done) => {
    service.notification$.subscribe(notification => {
      if (notification) {
        expect(notification.message).toBe('Test message');
        expect(notification.type).toBe('success');
        expect(notification.duration).toBe(5000);
        done();
      }
    });

    service.show({ message: 'Test message', type: 'success' });
  });

  it('should show notification with custom duration', (done) => {
    service.notification$.subscribe(notification => {
      if (notification) {
        expect(notification.duration).toBe(3000);
        done();
      }
    });

    service.show({ message: 'Test', type: 'error', duration: 3000 });
  });

  it('should clear notification', (done) => {
    service.show({ message: 'Test', type: 'info' });
    
    service.notification$.subscribe(notification => {
      if (notification === null) {
        done();
      }
    });

    service.clear();
  });

  it('should emit different notification types', (done) => {
    const types: Array<'success' | 'error' | 'info' | 'warning'> = ['success', 'error', 'info', 'warning'];
    let index = 0;

    service.notification$.subscribe(notification => {
      if (notification) {
        expect(notification.type).toBe(types[index]);
        index++;
        if (index === types.length) {
          done();
        }
      }
    });

    types.forEach(type => {
      service.show({ message: 'Test', type });
      service.clear();
    });
  });
});

