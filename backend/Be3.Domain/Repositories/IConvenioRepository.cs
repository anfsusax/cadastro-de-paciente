using Be3.Domain.Models;

namespace Be3.Domain.Repositories;

public interface IConvenioRepository
{
    Task<IEnumerable<Convenio>> ObterTodosAtivosAsync();
    Task<Convenio?> ObterPorIdAsync(int id);
}
