using Be3.Application.DTOs;

namespace Be3.Application.Services;

public interface IConvenioService
{
    Task<IEnumerable<ConvenioDTO>> ObterTodosAtivosAsync();
}
