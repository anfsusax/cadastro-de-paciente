import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ConvenioService } from './convenio.service';
import { Convenio } from '../models/paciente.model';

describe('ConvenioService', () => {
  let service: ConvenioService;
  let httpMock: HttpTestingController;
  const apiUrl = 'http://localhost:5123/api/convenios';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ConvenioService]
    });
    service = TestBed.inject(ConvenioService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all convenios', () => {
    const mockConvenios: Convenio[] = [
      { id: 1, nome: 'Unimed' },
      { id: 2, nome: 'Bradesco Saúde' }
    ];

    service.obterTodos().subscribe(convenios => {
      expect(convenios.length).toBe(2);
      expect(convenios[0].nome).toBe('Unimed');
      expect(convenios[1].nome).toBe('Bradesco Saúde');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockConvenios);
  });
});

