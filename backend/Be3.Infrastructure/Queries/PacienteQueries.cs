namespace Be3.Infrastructure.Queries;

public static class PacienteQueries
{
    public const string ObterTodos = @"
        SELECT p.*, c.Id, c.Nome, c.Ativo
        FROM Pacientes p
        LEFT JOIN Convenios c ON p.ConvenioId = c.Id
        ORDER BY p.Nome, p.Sobrenome";

    public const string ObterPorId = @"
        SELECT p.*, c.Id, c.Nome, c.Ativo
        FROM Pacientes p
        LEFT JOIN Convenios c ON p.ConvenioId = c.Id
        WHERE p.Id = @Id";

    public const string ObterPorCpf = @"
        SELECT p.*, c.Id, c.Nome, c.Ativo
        FROM Pacientes p
        LEFT JOIN Convenios c ON p.ConvenioId = c.Id
        WHERE p.CPF = @CPF AND p.Ativo = 1";

    public const string ExisteCpf = @"
        SELECT COUNT(1)
        FROM Pacientes
        WHERE CPF = @CPF AND Ativo = 1 AND (@ExcludeId IS NULL OR Id != @ExcludeId)";

    public const string Inserir = @"
        INSERT INTO Pacientes (
            Nome, Sobrenome, DataNascimento, Genero, CPF, RG, UfRG,
            Email, Celular, TelefoneFixo, ConvenioId, NumeroCarteirinha,
            ValidadeCarteirinha, Ativo
        )
        VALUES (
            @Nome, @Sobrenome, @DataNascimento, @Genero, @CPF, @RG, @UfRG,
            @Email, @Celular, @TelefoneFixo, @ConvenioId, @NumeroCarteirinha,
            @ValidadeCarteirinha, @Ativo
        );
        SELECT CAST(SCOPE_IDENTITY() as int);";

    public const string Atualizar = @"
        UPDATE Pacientes
        SET Nome = @Nome,
            Sobrenome = @Sobrenome,
            DataNascimento = @DataNascimento,
            Genero = @Genero,
            CPF = @CPF,
            RG = @RG,
            UfRG = @UfRG,
            Email = @Email,
            Celular = @Celular,
            TelefoneFixo = @TelefoneFixo,
            ConvenioId = @ConvenioId,
            NumeroCarteirinha = @NumeroCarteirinha,
            ValidadeCarteirinha = @ValidadeCarteirinha
        WHERE Id = @Id";

    public const string Desativar = @"
        UPDATE Pacientes
        SET Ativo = 0
        WHERE Id = @Id";

    public const string Ativar = @"
        UPDATE Pacientes
        SET Ativo = 1
        WHERE Id = @Id";
}
