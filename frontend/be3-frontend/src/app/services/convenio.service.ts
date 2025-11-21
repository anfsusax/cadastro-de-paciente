import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Convenio } from '../models/paciente.model';

@Injectable({
  providedIn: 'root'
})
export class ConvenioService {
  private apiUrl = 'http://localhost:5123/api/convenios';
  private http = inject(HttpClient);

  obterTodos(): Observable<Convenio[]> {
    return this.http.get<Convenio[]>(this.apiUrl);
  }
}
