import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs';
import { PacienteForm } from './paciente-form';
import { PacienteService } from '../../services/paciente.service';
import { ConvenioService } from '../../services/convenio.service';
import { Paciente, Convenio } from '../../models/paciente.model';

describe('PacienteForm', () => {
  let component: PacienteForm;
  let fixture: ComponentFixture<PacienteForm>;
  let pacienteService: jasmine.SpyObj<PacienteService>;
  let convenioService: jasmine.SpyObj<ConvenioService>;
  let router: jasmine.SpyObj<Router>;

  const mockConvenios: Convenio[] = [
    { id: 1, nome: 'Unimed' },
    { id: 2, nome: 'Bradesco Saúde' }
  ];

  const mockPaciente: Paciente = {
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
  };

  beforeEach(async () => {
    const pacienteServiceSpy = jasmine.createSpyObj('PacienteService', [
      'obterPorId',
      'criar',
      'atualizar'
    ]);
    const convenioServiceSpy = jasmine.createSpyObj('ConvenioService', ['obterTodos']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [PacienteForm, HttpClientTestingModule],
      providers: [
        { provide: PacienteService, useValue: pacienteServiceSpy },
        { provide: ConvenioService, useValue: convenioServiceSpy },
        { provide: Router, useValue: routerSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: (key: string) => null
              }
            }
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PacienteForm);
    component = fixture.componentInstance;
    pacienteService = TestBed.inject(PacienteService) as jasmine.SpyObj<PacienteService>;
    convenioService = TestBed.inject(ConvenioService) as jasmine.SpyObj<ConvenioService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load convenios on init', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    
    fixture.detectChanges();
    
    expect(convenioService.obterTodos).toHaveBeenCalled();
    expect(component.convenios.length).toBe(2);
  });

  it('should create form with required fields', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    expect(component.form).toBeDefined();
    expect(component.form.get('nome')).toBeTruthy();
    expect(component.form.get('sobrenome')).toBeTruthy();
    expect(component.form.get('email')).toBeTruthy();
  });

  it('should validate required fields', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    expect(component.form.valid).toBe(false);
    
    component.form.patchValue({
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '11999999999'
    });
    
    expect(component.form.valid).toBe(true);
  });

  it('should validate email format', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    component.form.patchValue({
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'invalid-email',
      celular: '11999999999'
    });
    
    expect(component.form.get('email')?.valid).toBe(false);
  });

  it('should require at least one phone number', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    component.form.patchValue({
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '',
      telefoneFixo: ''
    });
    
    expect(component.form.hasError('telefoneObrigatorio')).toBe(true);
  });

  it('should load paciente in edit mode', () => {
    const route = TestBed.inject(ActivatedRoute);
    spyOn(route.snapshot.paramMap, 'get').and.returnValue('1');
    
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    pacienteService.obterPorId.and.returnValue(of(mockPaciente));
    
    fixture = TestBed.createComponent(PacienteForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    expect(component.isEditMode).toBe(true);
    expect(component.pacienteId).toBe(1);
    expect(pacienteService.obterPorId).toHaveBeenCalledWith(1);
    expect(component.form.get('nome')?.value).toBe('João');
  });

  it('should create new paciente', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    pacienteService.criar.and.returnValue(of(mockPaciente));
    
    fixture.detectChanges();
    
    component.form.patchValue({
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '11999999999'
    });
    
    component.onSubmit();
    
    expect(pacienteService.criar).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/pacientes']);
  });

  it('should update existing paciente', () => {
    const route = TestBed.inject(ActivatedRoute);
    spyOn(route.snapshot.paramMap, 'get').and.returnValue('1');
    
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    pacienteService.obterPorId.and.returnValue(of(mockPaciente));
    pacienteService.atualizar.and.returnValue(of(mockPaciente));
    
    fixture = TestBed.createComponent(PacienteForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
    
    component.form.patchValue({
      nome: 'João Atualizado',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '11999999999'
    });
    
    component.onSubmit();
    
    expect(pacienteService.atualizar).toHaveBeenCalledWith(1, jasmine.any(Object));
    expect(router.navigate).toHaveBeenCalledWith(['/pacientes']);
  });

  it('should not submit if form is invalid', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    component.onSubmit();
    
    expect(pacienteService.criar).not.toHaveBeenCalled();
    expect(pacienteService.atualizar).not.toHaveBeenCalled();
  });

  it('should handle error when creating paciente', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    pacienteService.criar.and.returnValue(
      throwError(() => ({ error: { mensagem: 'Erro ao criar' } }))
    );
    
    fixture.detectChanges();
    
    component.form.patchValue({
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '11999999999'
    });
    
    component.onSubmit();
    
    expect(component.error).toBe('Erro ao criar');
  });

  it('should handle validation errors', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    pacienteService.criar.and.returnValue(
      throwError(() => ({
        error: {
          erros: [
            { campo: 'Email', mensagem: 'Email já cadastrado' }
          ]
        }
      }))
    );
    
    fixture.detectChanges();
    
    component.form.patchValue({
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      celular: '11999999999'
    });
    
    component.onSubmit();
    
    expect(component.error).toContain('Erros de validação');
  });

  it('should cancel and navigate back', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    component.cancelar();
    
    expect(router.navigate).toHaveBeenCalledWith(['/pacientes']);
  });

  it('should get field error messages', () => {
    convenioService.obterTodos.and.returnValue(of(mockConvenios));
    fixture.detectChanges();
    
    const nomeField = component.form.get('nome');
    nomeField?.markAsTouched();
    
    expect(component.getFieldError('nome')).toBe('nome é obrigatório.');
    
    component.form.patchValue({ email: 'invalid' });
    const emailField = component.form.get('email');
    emailField?.markAsTouched();
    
    expect(component.getFieldError('email')).toBe('Email inválido.');
  });
});

