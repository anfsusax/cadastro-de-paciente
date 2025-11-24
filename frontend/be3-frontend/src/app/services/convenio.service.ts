import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Convenio } from '../models/paciente.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConvenioService {
  private apiUrl = `${environment.apiUrl}/convenios`;
  private http = inject(HttpClient);

  obterTodos(): Observable<Convenio[]> {
    return this.http.get<Convenio[]>(this.apiUrl);
  }
}
