export interface Paciente {
  id: number;
  nome: string;
  sobrenome: string;
  dataNascimento: string;
  genero: number;
  cpf?: string;
  rg: string;
  ufRG: number;
  email: string;
  celular?: string;
  telefoneFixo?: string;
  convenioId?: number;
  numeroCarteirinha?: string;
  validadeCarteirinha?: string;
  ativo: boolean;
  convenio?: Convenio;
}

export interface CreatePaciente {
  nome: string;
  sobrenome: string;
  dataNascimento: string;
  genero: number;
  cpf?: string;
  rg: string;
  ufRG: number;
  email: string;
  celular?: string;
  telefoneFixo?: string;
  convenioId?: number;
  numeroCarteirinha?: string;
  validadeCarteirinha?: string;
}

export interface UpdatePaciente {
  nome: string;
  sobrenome: string;
  dataNascimento: string;
  genero: number;
  cpf?: string;
  rg: string;
  ufRG: number;
  email: string;
  celular?: string;
  telefoneFixo?: string;
  convenioId?: number;
  numeroCarteirinha?: string;
  validadeCarteirinha?: string;
}

export interface Convenio {
  id: number;
  nome: string;
}

export enum Genero {
  Masculino = 1,
  Feminino = 2,
  Outro = 3
}

export enum Uf {
  AC = 1, AL = 2, AP = 3, AM = 4, BA = 5, CE = 6, DF = 7, ES = 8, GO = 9, MA = 10,
  MT = 11, MS = 12, MG = 13, PA = 14, PB = 15, PR = 16, PE = 17, PI = 18, RJ = 19, RN = 20,
  RS = 21, RO = 22, RR = 23, SC = 24, SP = 25, SE = 26, TO = 27
}

export const UFS = [
  { value: 1, label: 'AC' }, { value: 2, label: 'AL' }, { value: 3, label: 'AP' },
  { value: 4, label: 'AM' }, { value: 5, label: 'BA' }, { value: 6, label: 'CE' },
  { value: 7, label: 'DF' }, { value: 8, label: 'ES' }, { value: 9, label: 'GO' },
  { value: 10, label: 'MA' }, { value: 11, label: 'MT' }, { value: 12, label: 'MS' },
  { value: 13, label: 'MG' }, { value: 14, label: 'PA' }, { value: 15, label: 'PB' },
  { value: 16, label: 'PR' }, { value: 17, label: 'PE' }, { value: 18, label: 'PI' },
  { value: 19, label: 'RJ' }, { value: 20, label: 'RN' }, { value: 21, label: 'RS' },
  { value: 22, label: 'RO' }, { value: 23, label: 'RR' }, { value: 24, label: 'SC' },
  { value: 25, label: 'SP' }, { value: 26, label: 'SE' }, { value: 27, label: 'TO' }
];

export const GENEROS = [
  { value: 1, label: 'Masculino' },
  { value: 2, label: 'Feminino' },
  { value: 3, label: 'Outro' }
];
