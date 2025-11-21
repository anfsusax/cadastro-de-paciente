import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { PacienteService } from '../../services/paciente.service';
import { ConvenioService } from '../../services/convenio.service';
import { Convenio, GENEROS, UFS, CreatePaciente, UpdatePaciente } from '../../models/paciente.model';

@Component({
  selector: 'app-paciente-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgxMaskDirective],
  templateUrl: './paciente-form.html',
  styleUrl: './paciente-form.css'
})
export class PacienteForm implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private pacienteService = inject(PacienteService);
  private convenioService = inject(ConvenioService);

  form!: FormGroup;
  convenios: Convenio[] = [];
  generos = GENEROS;
  ufs = UFS;
  loading = false;
  error: string | null = null;
  isEditMode = false;
  pacienteId?: number;

  ngOnInit(): void {
    this.carregarConvenios();
    this.criarFormulario();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.pacienteId = +id;
      this.carregarPaciente(+id);
    }
  }

  criarFormulario(): void {
    this.form = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(2)]],
      sobrenome: ['', [Validators.required, Validators.minLength(2)]],
      dataNascimento: ['', [Validators.required]],
      genero: ['', [Validators.required]],
      cpf: [''],
      rg: ['', [Validators.required]],
      ufRG: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      celular: [''],
      telefoneFixo: [''],
      convenioId: [''],
      numeroCarteirinha: [''],
      validadeCarteirinha: ['']
    }, { validators: this.telefoneValidator });
  }

  telefoneValidator(form: FormGroup) {
    const celular = form.get('celular')?.value;
    const telefoneFixo = form.get('telefoneFixo')?.value;
    
    if (!celular && !telefoneFixo) {
      return { telefoneObrigatorio: true };
    }
    return null;
  }

  carregarConvenios(): void {
    this.convenioService.obterTodos().subscribe({
      next: (data) => {
        this.convenios = data;
      },
      error: (err) => {
        console.error('Erro ao carregar convênios:', err);
      }
    });
  }

  carregarPaciente(id: number): void {
    this.loading = true;
    this.pacienteService.obterPorId(id).subscribe({
      next: (paciente) => {
        const dataNasc = paciente.dataNascimento ? paciente.dataNascimento.split('T')[0] : '';
        let validade = '';
        if (paciente.validadeCarteirinha) {
          const date = new Date(paciente.validadeCarteirinha);
          validade = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}`;
        }
        
        this.form.patchValue({
          nome: paciente.nome,
          sobrenome: paciente.sobrenome,
          dataNascimento: dataNasc,
          genero: paciente.genero,
          cpf: paciente.cpf || '',
          rg: paciente.rg,
          ufRG: paciente.ufRG,
          email: paciente.email,
          celular: paciente.celular || '',
          telefoneFixo: paciente.telefoneFixo || '',
          convenioId: paciente.convenioId || '',
          numeroCarteirinha: paciente.numeroCarteirinha || '',
          validadeCarteirinha: validade
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao carregar paciente.';
        this.loading = false;
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.error = null;

    const formValue = this.form.value;
    
    let validadeCarteirinha: string | undefined = undefined;
    if (formValue.validadeCarteirinha && formValue.validadeCarteirinha.trim() !== '') {
      const parts = formValue.validadeCarteirinha.split('-');
      if (parts.length === 2 && parts[0] && parts[1]) {
        validadeCarteirinha = `${parts[0]}-${parts[1]}-01T00:00:00`;
      }
    }
    
    const data: CreatePaciente | UpdatePaciente = {
      nome: formValue.nome?.trim() || '',
      sobrenome: formValue.sobrenome?.trim() || '',
      dataNascimento: formValue.dataNascimento ? `${formValue.dataNascimento}T00:00:00` : '',
      genero: formValue.genero ? +formValue.genero : 0,
      cpf: formValue.cpf ? formValue.cpf.replace(/\D/g, '') : undefined,
      rg: formValue.rg?.trim() || '',
      ufRG: formValue.ufRG ? +formValue.ufRG : 0,
      email: formValue.email?.trim() || '',
      celular: formValue.celular ? formValue.celular.replace(/\D/g, '') : undefined,
      telefoneFixo: formValue.telefoneFixo ? formValue.telefoneFixo.replace(/\D/g, '') : undefined,
      convenioId: formValue.convenioId ? +formValue.convenioId : undefined,
      numeroCarteirinha: formValue.numeroCarteirinha?.trim() || undefined,
      validadeCarteirinha: validadeCarteirinha || undefined
    };

    const operacao = this.isEditMode
      ? this.pacienteService.atualizar(this.pacienteId!, data)
      : this.pacienteService.criar(data);

    operacao.subscribe({
      next: () => {
        this.router.navigate(['/pacientes']);
      },
      error: (err) => {
        this.loading = false;
        console.error('Erro completo:', err);
        
        if (err.error?.erros) {
          const erros = Array.isArray(err.error.erros) 
            ? err.error.erros.map((e: any) => e.mensagem || e.Mensagem || `${e.campo || e.Campo}: ${e.message || e.Message || JSON.stringify(e)}`).join(', ')
            : JSON.stringify(err.error.erros);
          this.error = 'Erros de validação: ' + erros;
        } else if (err.error?.errors) {
          const errosValidacao = Object.entries(err.error.errors)
            .map(([campo, erros]: [string, any]) => `${campo}: ${Array.isArray(erros) ? erros.join(', ') : erros}`)
            .join(' | ');
          this.error = 'Erros de validação: ' + errosValidacao;
        } else if (err.error?.mensagem) {
          this.error = err.error.mensagem;
        } else {
          this.error = 'Erro ao salvar paciente. Verifique os dados e tente novamente.';
        }
      }
    });
  }

  cancelar(): void {
    this.router.navigate(['/pacientes']);
  }

  getFieldError(fieldName: string): string {
    const field = this.form.get(fieldName);
    if (!field || !field.errors || !field.touched) {
      return '';
    }

    if (field.errors['required']) {
      return `${fieldName} é obrigatório.`;
    }
    if (field.errors['email']) {
      return 'Email inválido.';
    }
    if (field.errors['minlength']) {
      return `${fieldName} deve ter no mínimo ${field.errors['minlength'].requiredLength} caracteres.`;
    }
    return '';
  }
}