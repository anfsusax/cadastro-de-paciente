import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PacienteService } from './paciente.service';
import { Paciente, CreatePaciente, UpdatePaciente } from '../models/paciente.model';

describe('PacienteService', () => {
  let service: PacienteService;
  let httpMock: HttpTestingController;
  const apiUrl = 'http://localhost:5123/api/pacientes';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PacienteService]
    });
    service = TestBed.inject(PacienteService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all pacientes', () => {
    const mockPacientes: Paciente[] = [
      {
        id: 1,
        nome: 'João',
        sobrenome: 'Silva',
        dataNascimento: '1990-01-01T00:00:00',
        genero: 1,
        rg: '1234567',
        ufRG: 25,
        email: 'joao@email.com',
        ativo: true
      }
    ];

    service.obterTodos().subscribe(pacientes => {
      expect(pacientes.length).toBe(1);
      expect(pacientes[0].nome).toBe('João');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockPacientes);
  });

  it('should get paciente by id', () => {
    const mockPaciente: Paciente = {
      id: 1,
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01T00:00:00',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com',
      ativo: true
    };

    service.obterPorId(1).subscribe(paciente => {
      expect(paciente.id).toBe(1);
      expect(paciente.nome).toBe('João');
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockPaciente);
  });

  it('should create paciente', () => {
    const createPaciente: CreatePaciente = {
      nome: 'João',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01T00:00:00',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com'
    };

    const mockPaciente: Paciente = {
      id: 1,
      ...createPaciente,
      ativo: true
    };

    service.criar(createPaciente).subscribe(paciente => {
      expect(paciente.id).toBe(1);
      expect(paciente.nome).toBe('João');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(createPaciente);
    req.flush(mockPaciente);
  });

  it('should update paciente', () => {
    const updatePaciente: UpdatePaciente = {
      nome: 'João Atualizado',
      sobrenome: 'Silva',
      dataNascimento: '1990-01-01T00:00:00',
      genero: 1,
      rg: '1234567',
      ufRG: 25,
      email: 'joao@email.com'
    };

    const mockPaciente: Paciente = {
      id: 1,
      ...updatePaciente,
      ativo: true
    };

    service.atualizar(1, updatePaciente).subscribe(paciente => {
      expect(paciente.nome).toBe('João Atualizado');
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatePaciente);
    req.flush(mockPaciente);
  });

  it('should deactivate paciente', () => {
    service.desativar(1).subscribe(() => {
      expect(true).toBe(true);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });

  it('should activate paciente', () => {
    service.ativar(1).subscribe(() => {
      expect(true).toBe(true);
    });

    const req = httpMock.expectOne(`${apiUrl}/1/ativar`);
    expect(req.request.method).toBe('PATCH');
    req.flush(null);
  });
});

