namespace Be3.Infrastructure.Queries;

public static class ConvenioQueries
{
    public const string ObterTodosAtivos = @"
        SELECT Id, Nome, Ativo
        FROM Convenios
        WHERE Ativo = 1
        ORDER BY Nome";

    public const string ObterPorId = @"
        SELECT Id, Nome, Ativo
        FROM Convenios
        WHERE Id = @Id";
}
