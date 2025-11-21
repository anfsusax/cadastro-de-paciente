import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PacienteService } from '../../services/paciente.service';
import { Paciente } from '../../models/paciente.model';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-paciente-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './paciente-list.html',
  styleUrl: './paciente-list.css'
})
export class PacienteListComponent implements OnInit {
  private pacienteService = inject(PacienteService);
  private notificationService = inject(NotificationService);
  
  pacientes: Paciente[] = [];
  loading = false;
  error: string | null = null;
  pacienteSelecionado: { id: number; nome: string; ativo: boolean } | null = null;
  mostrarModal = false;

  ngOnInit(): void {
    this.carregarPacientes();
  }

  carregarPacientes(): void {
    this.loading = true;
    this.error = null;
    this.pacienteService.obterTodos().subscribe({
      next: (data) => {
        this.pacientes = data.filter(p => p.id != null && p.id > 0);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao carregar pacientes. Por favor, tente novamente.';
        this.loading = false;
        console.error(err);
      }
    });
  }

  abrirModalConfirmacao(paciente: Paciente): void {
    if (this.loading || this.mostrarModal || !paciente || !paciente.id) {
      console.warn('Não é possível abrir modal:', { paciente, loading: this.loading, mostrarModal: this.mostrarModal });
      return;
    }
    
    const ativo = paciente.ativo === true || paciente.ativo === undefined;
    
    this.pacienteSelecionado = {
      id: paciente.id,
      nome: `${paciente.nome} ${paciente.sobrenome}`,
      ativo: ativo
    };
    this.mostrarModal = true;
  }

  fecharModal(): void {
    this.mostrarModal = false;
    this.pacienteSelecionado = null;
  }

  confirmarAcao(): void {
    if (!this.pacienteSelecionado || this.loading || !this.pacienteSelecionado.id) {
      console.error('Não é possível confirmar ação:', { pacienteSelecionado: this.pacienteSelecionado, loading: this.loading });
      this.error = 'Erro: ID do paciente não encontrado.';
      return;
    }

    this.loading = true;
    this.error = null;
    
    const pacienteId = this.pacienteSelecionado.id;
    const isAtivando = !this.pacienteSelecionado.ativo;
    
    console.log('Confirmando ação:', { pacienteId, isAtivando, ativo: this.pacienteSelecionado.ativo });
    
    const operacao = isAtivando
      ? this.pacienteService.ativar(pacienteId)
      : this.pacienteService.desativar(pacienteId);

    operacao.subscribe({
      next: () => {
        this.loading = false;
        this.notificationService.show({
          message: `Paciente ${isAtivando ? 'ativado' : 'desativado'} com sucesso!`,
          type: 'success'
        });
        this.fecharModal();
        this.carregarPacientes();
      },
      error: (err) => {
        this.loading = false;
        const errorMessage = `Erro ao ${isAtivando ? 'ativar' : 'desativar'} o paciente. Tente novamente.`;
        this.error = errorMessage;
        this.notificationService.show({
          message: errorMessage,
          type: 'error'
        });
        console.error('Erro ao alterar status:', err);
        this.fecharModal();
        setTimeout(() => {
          this.error = null;
        }, 5000);
      }
    });
  }

  formatarCpf(cpf?: string): string {
    if (!cpf) return '-';
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }

  formatarData(data: string): string {
    return new Date(data).toLocaleDateString('pt-BR');
  }
}
