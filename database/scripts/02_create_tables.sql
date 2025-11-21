USE Be3DB;
GO

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
