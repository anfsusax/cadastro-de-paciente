using AutoMapper;
using Be3.Application.DTOs;
using Be3.Domain.Repositories;

namespace Be3.Application.Services;

public class ConvenioService : IConvenioService
{
    private readonly IConvenioRepository _convenioRepository;
    private readonly IMapper _mapper;

    public ConvenioService(IConvenioRepository convenioRepository, IMapper mapper)
    {
        _convenioRepository = convenioRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ConvenioDTO>> ObterTodosAtivosAsync()
    {
        var convenios = await _convenioRepository.ObterTodosAtivosAsync();
        return _mapper.Map<IEnumerable<ConvenioDTO>>(convenios);
    }
}
