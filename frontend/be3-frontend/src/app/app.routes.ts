import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/pacientes',
    pathMatch: 'full'
  },
  {
    path: 'pacientes',
    loadComponent: () => import('./components/paciente-list/paciente-list.component').then(m => m.PacienteListComponent)
  },
  {
    path: 'pacientes/novo',
    loadComponent: () => import('./components/paciente-form/paciente-form').then(m => m.PacienteForm)
  },
  {
    path: 'pacientes/editar/:id',
    loadComponent: () => import('./components/paciente-form/paciente-form').then(m => m.PacienteForm)
  }
];