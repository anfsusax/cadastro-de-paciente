using Be3.Api.Controllers;
using Be3.Application.DTOs;
using Be3.Application.Services;
using Be3.Application.Validators;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Be3.Tests.Controllers;

public class PacientesControllerTests
{
    private readonly Mock<IPacienteService> _pacienteServiceMock;
    private readonly PacientesController _controller;

    public PacientesControllerTests()
    {
        _pacienteServiceMock = new Mock<IPacienteService>();
        _controller = new PacientesController(_pacienteServiceMock.Object);
    }

    [Fact]
    public async Task ObterTodos_DeveRetornarListaDePacientes()
    {
        // Arrange
        var pacientes = new List<PacienteDTO>
        {
            new PacienteDTO
            {
                Id = 1,
                Nome = "João",
                Sobrenome = "Silva",
                DataNascimento = new DateTime(1990, 1, 1),
                Genero = 1,
                RG = "1234567",
                UfRG = 25,
                Email = "joao@email.com",
                Ativo = true
            }
        };

        _pacienteServiceMock.Setup(x => x.ObterTodosAsync())
            .ReturnsAsync(pacientes);

        // Act
        var result = await _controller.ObterTodos();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeAssignableTo<IEnumerable<PacienteDTO>>().Subject;
        returnValue.Should().HaveCount(1);
        returnValue.First().Nome.Should().Be("João");
    }

    [Fact]
    public async Task ObterPorId_QuandoPacienteExiste_DeveRetornarPaciente()
    {
        // Arrange
        var paciente = new PacienteDTO
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Ativo = true
        };

        _pacienteServiceMock.Setup(x => x.ObterPorIdAsync(1))
            .ReturnsAsync(paciente);

        // Act
        var result = await _controller.ObterPorId(1);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeOfType<PacienteDTO>().Subject;
        returnValue.Id.Should().Be(1);
        returnValue.Nome.Should().Be("João");
    }

    [Fact]
    public async Task ObterPorId_QuandoPacienteNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        _pacienteServiceMock.Setup(x => x.ObterPorIdAsync(999))
            .ReturnsAsync((PacienteDTO?)null);

        // Act
        var result = await _controller.ObterPorId(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Criar_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var createDto = new CreatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        var pacienteCriado = new PacienteDTO
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Ativo = true
        };

        _pacienteServiceMock.Setup(x => x.CriarAsync(It.IsAny<CreatePacienteDTO>()))
            .ReturnsAsync(pacienteCriado);

        // Act
        var result = await _controller.Criar(createDto);

        // Assert
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.Value.Should().BeOfType<PacienteDTO>();
        _pacienteServiceMock.Verify(x => x.CriarAsync(It.IsAny<CreatePacienteDTO>()), Times.Once);
    }

    [Fact]
    public async Task Criar_ComErroDeValidacao_DeveRetornarBadRequest()
    {
        // Arrange
        var createDto = new CreatePacienteDTO
        {
            Nome = "",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com"
        };

        var erros = new List<ValidationError>
        {
            new ValidationError("Nome", "Nome é obrigatório.")
        }.AsReadOnly();

        _controller.ModelState.AddModelError("Nome", "Nome é obrigatório.");
        _pacienteServiceMock.Setup(x => x.CriarAsync(It.IsAny<CreatePacienteDTO>()))
            .ThrowsAsync(new ValidationException(erros));

        // Act
        var result = await _controller.Criar(createDto);

        // Assert
        if (!_controller.ModelState.IsValid)
        {
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }

    [Fact]
    public async Task Atualizar_ComDadosValidos_DeveRetornarOk()
    {
        // Arrange
        var updateDto = new UpdatePacienteDTO
        {
            Nome = "João Atualizado",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Celular = "11999999999"
        };

        var pacienteAtualizado = new PacienteDTO
        {
            Id = 1,
            Nome = "João Atualizado",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com",
            Ativo = true
        };

        _pacienteServiceMock.Setup(x => x.AtualizarAsync(1, It.IsAny<UpdatePacienteDTO>()))
            .ReturnsAsync(pacienteAtualizado);

        // Act
        var result = await _controller.Atualizar(1, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnValue = okResult.Value.Should().BeOfType<PacienteDTO>().Subject;
        returnValue.Nome.Should().Be("João Atualizado");
        _pacienteServiceMock.Verify(x => x.AtualizarAsync(1, It.IsAny<UpdatePacienteDTO>()), Times.Once);
    }

    [Fact]
    public async Task Atualizar_QuandoPacienteNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        var updateDto = new UpdatePacienteDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = 1,
            RG = "1234567",
            UfRG = 25,
            Email = "joao@email.com"
        };

        _pacienteServiceMock.Setup(x => x.AtualizarAsync(999, It.IsAny<UpdatePacienteDTO>()))
            .ThrowsAsync(new NotFoundException("Paciente com ID 999 não encontrado."));

        // Act
        var result = await _controller.Atualizar(999, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Desativar_QuandoPacienteExiste_DeveRetornarNoContent()
    {
        // Arrange
        _pacienteServiceMock.Setup(x => x.DesativarAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Desativar(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _pacienteServiceMock.Verify(x => x.DesativarAsync(1), Times.Once);
    }

    [Fact]
    public async Task Desativar_QuandoPacienteNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        _pacienteServiceMock.Setup(x => x.DesativarAsync(999))
            .ThrowsAsync(new NotFoundException("Paciente com ID 999 não encontrado."));

        // Act
        var result = await _controller.Desativar(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Ativar_QuandoPacienteExiste_DeveRetornarNoContent()
    {
        // Arrange
        _pacienteServiceMock.Setup(x => x.AtivarAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Ativar(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _pacienteServiceMock.Verify(x => x.AtivarAsync(1), Times.Once);
    }

    [Fact]
    public async Task Ativar_QuandoPacienteNaoExiste_DeveRetornarNotFound()
    {
        // Arrange
        _pacienteServiceMock.Setup(x => x.AtivarAsync(999))
            .ThrowsAsync(new NotFoundException("Paciente com ID 999 não encontrado."));

        // Act
        var result = await _controller.Ativar(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

