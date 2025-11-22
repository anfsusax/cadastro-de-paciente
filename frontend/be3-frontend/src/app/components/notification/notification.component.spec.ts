import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NotificationComponent } from './notification.component';
import { NotificationService } from '../../services/notification.service';
import { of } from 'rxjs';

describe('NotificationComponent', () => {
  let component: NotificationComponent;
  let fixture: ComponentFixture<NotificationComponent>;
  let notificationService: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    const notificationServiceSpy = jasmine.createSpyObj('NotificationService', ['clear'], {
      notification$: of(null)
    });

    await TestBed.configureTestingModule({
      imports: [NotificationComponent],
      providers: [
        { provide: NotificationService, useValue: notificationServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    notificationService = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display notification when received', () => {
    const notification = { message: 'Test message', type: 'success' as const };
    notificationService.notification$ = of(notification);
    
    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    expect(component.notification).toEqual({ ...notification, duration: 5000 });
  });

  it('should auto-close notification after duration', (done) => {
    const notification = { message: 'Test message', type: 'success' as const, duration: 100 };
    notificationService.notification$ = of(notification);
    
    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    expect(component.notification).toBeTruthy();
    
    setTimeout(() => {
      expect(component.notification).toBeNull();
      done();
    }, 150);
  });

  it('should close notification manually', () => {
    component.notification = { message: 'Test', type: 'success' };
    
    component.close();
    
    expect(component.notification).toBeNull();
    expect(notificationService.clear).toHaveBeenCalled();
  });

  it('should clear timeout when new notification arrives', () => {
    spyOn(window, 'setTimeout').and.returnValue(123 as any);
    spyOn(window, 'clearTimeout');
    
    const notification1 = { message: 'First', type: 'success' as const, duration: 1000 };
    const notification2 = { message: 'Second', type: 'error' as const, duration: 2000 };
    
    notificationService.notification$ = of(notification1);
    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    notificationService.notification$ = of(notification2);
    fixture.detectChanges();
    
    expect(window.clearTimeout).toHaveBeenCalled();
  });
});

