using Be3.Api.Controllers;
using Be3.Application.DTOs;
using Be3.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Be3.Tests.Controllers;

public class ConveniosControllerTests
{
    private readonly Mock<IConvenioService> _convenioServiceMock;
    private readonly ConveniosController _controller;

    public ConveniosControllerTests()
    {
        _convenioServiceMock = new Mock<IConvenioService>();
        _controller = new ConveniosController(_convenioServiceMock.Object);
    }

    [Fact]
    public async Task ObterTodosAtivos_DeveRetornarListaDeConvenios()
    {
        // Arrange
        var convenios = new List<ConvenioDTO>
        {
            new ConvenioDTO { Id = 1, Nome = "Unimed" },
            new ConvenioDTO { Id = 2, Nome = "Bradesco Saúde" }
        };

        _convenioServiceMock.Setup(x => x.ObterTodosAtivosAsync())
            .ReturnsAsync(convenios);

        // Act
        var result = await _controller.ObterTodosAtivos();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<ConvenioDTO>>().Subject;
        returnValue.Should().HaveCount(2);
        returnValue.First().Nome.Should().Be("Unimed");
        returnValue.Last().Nome.Should().Be("Bradesco Saúde");
    }

    [Fact]
    public async Task ObterTodosAtivos_QuandoNaoHaConvenios_DeveRetornarListaVazia()
    {
        // Arrange
        _convenioServiceMock.Setup(x => x.ObterTodosAtivosAsync())
            .ReturnsAsync(new List<ConvenioDTO>());

        // Act
        var result = await _controller.ObterTodosAtivos();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<ConvenioDTO>>().Subject;
        returnValue.Should().BeEmpty();
    }
}

