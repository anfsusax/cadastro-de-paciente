import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { of, throwError } from 'rxjs';
import { PacienteListComponent } from './paciente-list.component';
import { PacienteService } from '../../services/paciente.service';
import { NotificationService } from '../../services/notification.service';
import { Paciente } from '../../models/paciente.model';

describe('PacienteListComponent', () => {
  let component: PacienteListComponent;
  let fixture: ComponentFixture<PacienteListComponent>;
  let pacienteService: jasmine.SpyObj<PacienteService>;
  let notificationService: jasmine.SpyObj<NotificationService>;

  const mockPacientes: Paciente[] = [
    {
      id: 1,
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01T00:00:00',
      genero: 1,
      cpf: '12345678901',
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '11999999999',
      ativo: true
    },
    {
      id: 2,
      nome: 'Maria',
      sobrenome: 'Santos',
      dataNascimento: '1985-05-15T00:00:00',
      genero: 2,
      rg: '7654321',
      ufRG: 25,
      email: 'maria@email.com',
      telefoneFixo: '1133333333',
      ativo: false
    }
  ];

  beforeEach(async () => {
    const pacienteServiceSpy = jasmine.createSpyObj('PacienteService', [
      'obterTodos',
      'ativar',
      'desativar'
    ]);
    const notificationServiceSpy = jasmine.createSpyObj('NotificationService', ['show']);

    await TestBed.configureTestingModule({
      imports: [
        PacienteListComponent,
        HttpClientTestingModule,
        RouterTestingModule
      ],
      providers: [
        { provide: PacienteService, useValue: pacienteServiceSpy },
        { provide: NotificationService, useValue: notificationServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PacienteListComponent);
    component = fixture.componentInstance;
    pacienteService = TestBed.inject(PacienteService) as jasmine.SpyObj<PacienteService>;
    notificationService = TestBed.inject(NotificationService) as jasmine.SpyObj<NotificationService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load pacientes on init', () => {
    pacienteService.obterTodos.and.returnValue(of(mockPacientes));
    
    fixture.detectChanges();
    
    expect(pacienteService.obterTodos).toHaveBeenCalled();
    expect(component.pacientes.length).toBe(2);
    expect(component.loading).toBe(false);
  });

  it('should filter out invalid pacientes', () => {
    const pacientesComInvalidos = [
      ...mockPacientes,
      { id: 0, nome: 'Invalid', sobrenome: 'Test', dataNascimento: '1990-01-01', genero: 1, rg: '123', ufRG: 1, email: 'test@test.com', ativo: true },
      { id: null as any, nome: 'Null', sobrenome: 'Test', dataNascimento: '1990-01-01', genero: 1, rg: '123', ufRG: 1, email: 'test@test.com', ativo: true }
    ];
    
    pacienteService.obterTodos.and.returnValue(of(pacientesComInvalidos));
    
    fixture.detectChanges();
    
    expect(component.pacientes.length).toBe(2);
    expect(component.pacientes.every(p => p.id != null && p.id > 0)).toBe(true);
  });

  it('should handle error when loading pacientes', () => {
    pacienteService.obterTodos.and.returnValue(throwError(() => new Error('Server error')));
    
    fixture.detectChanges();
    
    expect(component.error).toBe('Erro ao carregar pacientes. Por favor, tente novamente.');
    expect(component.loading).toBe(false);
    expect(component.pacientes.length).toBe(0);
  });

  it('should open confirmation modal for paciente', () => {
    pacienteService.obterTodos.and.returnValue(of(mockPacientes));
    fixture.detectChanges();
    
    component.abrirModalConfirmacao(mockPacientes[0]);
    
    expect(component.mostrarModal).toBe(true);
    expect(component.pacienteSelecionado).toEqual({
      id: 1,
      nome: 'João Silva',
      ativo: true
    });
  });

  it('should not open modal if paciente is invalid', () => {
    pacienteService.obterTodos.and.returnValue(of(mockPacientes));
    fixture.detectChanges();
    
    const pacienteSemId = { ...mockPacientes[0], id: null as any };
    component.abrirModalConfirmacao(pacienteSemId);
    
    expect(component.mostrarModal).toBe(false);
  });

  it('should close modal', () => {
    component.mostrarModal = true;
    component.pacienteSelecionado = { id: 1, nome: 'Test', ativo: true };
    
    component.fecharModal();
    
    expect(component.mostrarModal).toBe(false);
    expect(component.pacienteSelecionado).toBeNull();
  });

  it('should activate paciente', () => {
    pacienteService.obterTodos.and.returnValue(of(mockPacientes));
    pacienteService.ativar.and.returnValue(of(void 0));
    
    fixture.detectChanges();
    
    component.pacienteSelecionado = { id: 2, nome: 'Maria Santos', ativo: false };
    component.confirmarAcao();
    
    expect(pacienteService.ativar).toHaveBeenCalledWith(2);
    expect(notificationService.show).toHaveBeenCalledWith({
      message: 'Paciente ativado com sucesso!',
      type: 'success'
    });
  });

  it('should deactivate paciente', () => {
    pacienteService.obterTodos.and.returnValue(of(mockPacientes));
    pacienteService.desativar.and.returnValue(of(void 0));
    
    fixture.detectChanges();
    
    component.pacienteSelecionado = { id: 1, nome: 'João Silva', ativo: true };
    component.confirmarAcao();
    
    expect(pacienteService.desativar).toHaveBeenCalledWith(1);
    expect(notificationService.show).toHaveBeenCalledWith({
      message: 'Paciente desativado com sucesso!',
      type: 'success'
    });
  });

  it('should handle error when activating paciente', () => {
    pacienteService.obterTodos.and.returnValue(of(mockPacientes));
    pacienteService.ativar.and.returnValue(throwError(() => new Error('Error')));
    
    fixture.detectChanges();
    
    component.pacienteSelecionado = { id: 2, nome: 'Maria Santos', ativo: false };
    component.confirmarAcao();
    
    expect(component.error).toBe('Erro ao ativar o paciente. Tente novamente.');
    expect(notificationService.show).toHaveBeenCalledWith({
      message: 'Erro ao ativar o paciente. Tente novamente.',
      type: 'error'
    });
  });

  it('should format CPF correctly', () => {
    expect(component.formatarCpf('12345678901')).toBe('123.456.789-01');
    expect(component.formatarCpf('')).toBe('-');
    expect(component.formatarCpf(undefined)).toBe('-');
  });

  it('should format date correctly', () => {
    const formatted = component.formatarData('1990-01-01T00:00:00');
    expect(formatted).toContain('01/01/1990');
  });
});

