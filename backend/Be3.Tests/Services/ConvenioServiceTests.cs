using AutoMapper;
using Be3.Application.DTOs;
using Be3.Application.Services;
using Be3.Domain.Models;
using Be3.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be3.Tests.Services;

public class ConvenioServiceTests
{
    private readonly Mock<IConvenioRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ConvenioService _service;

    public ConvenioServiceTests()
    {
        _repositoryMock = new Mock<IConvenioRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new ConvenioService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task ObterTodosAtivosAsync_DeveRetornarListaDeConvenios()
    {
        // Arrange
        var convenios = new List<Convenio>
        {
            new Convenio { Id = 1, Nome = "Unimed", Ativo = true },
            new Convenio { Id = 2, Nome = "Bradesco Saúde", Ativo = true }
        };

        var conveniosDto = new List<ConvenioDTO>
        {
            new ConvenioDTO { Id = 1, Nome = "Unimed" },
            new ConvenioDTO { Id = 2, Nome = "Bradesco Saúde" }
        };

        _repositoryMock.Setup(x => x.ObterTodosAtivosAsync())
            .ReturnsAsync(convenios);
        _mapperMock.Setup(x => x.Map<IEnumerable<ConvenioDTO>>(convenios))
            .Returns(conveniosDto);

        // Act
        var result = await _service.ObterTodosAtivosAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Nome.Should().Be("Unimed");
        result.Last().Nome.Should().Be("Bradesco Saúde");
        _repositoryMock.Verify(x => x.ObterTodosAtivosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAtivosAsync_QuandoNaoHaConvenios_DeveRetornarListaVazia()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ObterTodosAtivosAsync())
            .ReturnsAsync(new List<Convenio>());

        // Act
        var result = await _service.ObterTodosAtivosAsync();

        // Assert
        result.Should().BeEmpty();
    }
}

