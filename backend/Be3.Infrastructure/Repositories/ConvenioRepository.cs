using Be3.Domain.Models;
using Be3.Domain.Repositories;
using Be3.Infrastructure.Queries;
using Dapper;

namespace Be3.Infrastructure.Repositories;

public class ConvenioRepository : IConvenioRepository
{
    private readonly DapperContext _context;

    public ConvenioRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Convenio>> ObterTodosAtivosAsync()
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Convenio>(ConvenioQueries.ObterTodosAtivos);
    }

    public async Task<Convenio?> ObterPorIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Convenio>(
            ConvenioQueries.ObterPorId,
            new { Id = id }
        );
    }
}
