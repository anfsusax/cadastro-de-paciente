-- Script de inicialização completa do banco de dados
-- Este script executa todos os passos de inicialização em sequência

SET QUOTED_IDENTIFIER ON;
GO

-- 1. Criar database se não existir
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Be3DB')
BEGIN
    CREATE DATABASE Be3DB;
END
GO

USE Be3DB;
GO

-- 2. Criar tabelas
IF OBJECT_ID('dbo.Pacientes', 'U') IS NOT NULL
    DROP TABLE dbo.Pacientes;
GO

IF OBJECT_ID('dbo.Convenios', 'U') IS NOT NULL
    DROP TABLE dbo.Convenios;
GO

CREATE TABLE dbo.Convenios
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(200) NOT NULL,
    Ativo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE dbo.Pacientes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Sobrenome NVARCHAR(100) NOT NULL,
    DataNascimento DATE NOT NULL,
    Genero INT NOT NULL,
    CPF NVARCHAR(11) NULL,
    RG NVARCHAR(20) NOT NULL,
    UfRG INT NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    Celular NVARCHAR(20) NULL,
    TelefoneFixo NVARCHAR(20) NULL,
    ConvenioId INT NULL,
    NumeroCarteirinha NVARCHAR(50) NULL,
    ValidadeCarteirinha DATE NULL,
    Ativo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Pacientes_Convenios FOREIGN KEY (ConvenioId) REFERENCES dbo.Convenios(Id),
    CONSTRAINT CHK_Pacientes_Telefone CHECK (Celular IS NOT NULL OR TelefoneFixo IS NOT NULL),
    CONSTRAINT CHK_Pacientes_DataNascimento CHECK (DataNascimento <= CAST(GETDATE() AS DATE)),
    CONSTRAINT UQ_Pacientes_CPF UNIQUE (CPF)
);
GO

CREATE INDEX IX_Pacientes_CPF ON dbo.Pacientes(CPF) WHERE CPF IS NOT NULL;
CREATE INDEX IX_Pacientes_Email ON dbo.Pacientes(Email);
CREATE INDEX IX_Pacientes_Ativo ON dbo.Pacientes(Ativo);
CREATE INDEX IX_Pacientes_ConvenioId ON dbo.Pacientes(ConvenioId);
GO

-- 3. Seed Convenios
IF NOT EXISTS (SELECT 1 FROM dbo.Convenios)
BEGIN
    INSERT INTO dbo.Convenios (Nome, Ativo) VALUES
    ('Unimed', 1),
    ('SulAmérica', 1),
    ('Bradesco Saúde', 1),
    ('Amil', 1),
    ('NotreDame Intermédica', 1),
    ('Golden Cross', 1),
    ('Medial Saúde', 1);
END
GO

-- 4. Seed Pacientes
IF NOT EXISTS (SELECT 1 FROM dbo.Pacientes WHERE CPF IS NOT NULL)
BEGIN
    INSERT INTO dbo.Pacientes (
        Nome, Sobrenome, DataNascimento, Genero, CPF, RG, UfRG,
        Email, Celular, TelefoneFixo, ConvenioId, NumeroCarteirinha,
        ValidadeCarteirinha, Ativo
    ) VALUES
    -- Paciente 1: João Silva (CPF válido: 111.444.777-35)
    (
        'João',
        'Silva',
        '1985-03-15',
        1, -- Masculino
        '11144477735',
        '12.345.678-9',
        25, -- SP
        'joao.silva@email.com',
        '(11) 98765-4321',
        '(11) 3456-7890',
        1, -- Unimed
        '123456789',
        '2025-12-31',
        1
    ),
    -- Paciente 2: Maria Santos (CPF válido: 222.555.888-36)
    (
        'Maria',
        'Santos',
        '1990-07-22',
        2, -- Feminino
        '22255588836',
        '23.456.789-0',
        19, -- RJ
        'maria.santos@email.com',
        '(21) 97654-3210',
        NULL,
        2, -- SulAmérica
        '987654321',
        '2026-06-30',
        1
    ),
    -- Paciente 3: Pedro Oliveira (CPF válido: 333.666.999-37)
    (
        'Pedro',
        'Oliveira',
        '1992-11-08',
        1, -- Masculino
        '33366699937',
        '34.567.890-1',
        13, -- MG
        'pedro.oliveira@email.com',
        NULL,
        '(31) 3456-7890',
        3, -- Bradesco Saúde
        '456789123',
        '2025-09-15',
        1
    ),
    -- Paciente 4: Ana Costa (CPF válido: 444.777.111-38)
    (
        'Ana',
        'Costa',
        '1988-05-30',
        2, -- Feminino
        '44477711138',
        '45.678.901-2',
        16, -- PR
        'ana.costa@email.com',
        '(41) 98888-7777',
        '(41) 3333-4444',
        4, -- Amil
        '789123456',
        NULL,
        1
    ),
    -- Paciente 5: Carlos Pereira (sem CPF, mas com todos os outros campos)
    (
        'Carlos',
        'Pereira',
        '1995-01-18',
        1, -- Masculino
        NULL, -- CPF não obrigatório
        '56.789.012-3',
        6, -- CE
        'carlos.pereira@email.com',
        '(85) 99999-8888',
        NULL,
        5, -- NotreDame Intermédica
        '321654987',
        '2026-03-31',
        1
    );
END
GO

PRINT 'Database initialization completed successfully!';
GO

