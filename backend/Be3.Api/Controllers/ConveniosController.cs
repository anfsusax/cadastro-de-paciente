using Be3.Application.DTOs;
using Be3.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Be3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConveniosController : ControllerBase
{
    private readonly IConvenioService _convenioService;

    public ConveniosController(IConvenioService convenioService)
    {
        _convenioService = convenioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConvenioDTO>>> ObterTodosAtivos()
    {
        var convenios = await _convenioService.ObterTodosAtivosAsync();
        return Ok(convenios);
    }
}
