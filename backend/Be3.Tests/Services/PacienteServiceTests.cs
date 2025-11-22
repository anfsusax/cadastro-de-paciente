using AutoMapper;
using Be3.Application.DTOs;
using Be3.Application.Services;
using Be3.Application.Validators;
using Be3.Domain.Models;
using Be3.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be3.Tests.Services;

public class PacienteServiceTests
{
    private readonly Mock<IPacienteRepository> _repositoryMock;
    private readonly Mock<IConvenioRepository> _convenioRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PacienteValidator _validator;
    private readonly PacienteService _service;

    public PacienteServiceTests()
    {
        _repositoryMock = new Mock<IPacienteRepository>();
        _convenioRepositoryMock = new Mock<IConvenioRepository>();
        _mapperMock = new Mock<IMapper>();
        _validator = new PacienteValidator(
            _repositoryMock.Object,
            _convenioRepositoryMock.Object);
        
        _service = new PacienteService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _validator);
    }

    [Fact]
    public async Task ObterTodosAsync_DeveRetornarListaDePacientes()
    {
        // Arrange
        var pacientes = new List<Paciente>
        {
            new Paciente
            {
                Id = 1,
                Nome = "João",
                Sobrenome = "Silva",
                DataNascimento = new DateTime(1990, 1, 1),
                Genero = Genero.Masculino,
                RG = "1234567",
                UfRG = Uf.SP,
                Email = "joao@email.com",
                Ativo = true
            }
        };

        var pacientesDto = new List<PacienteDTO>
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

        _repositoryMock.Setup(x => x.ObterTodosAsync())
            .ReturnsAsync(pacientes);
        _mapperMock.Setup(x => x.Map<IEnumerable<PacienteDTO>>(pacientes))
            .Returns(pacientesDto);

        // Act
        var result = await _service.ObterTodosAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Nome.Should().Be("João");
        _repositoryMock.Verify(x => x.ObterTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoPacienteExiste_DeveRetornarPaciente()
    {
        // Arrange
        var paciente = new Paciente
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = Genero.Masculino,
            RG = "1234567",
            UfRG = Uf.SP,
            Email = "joao@email.com",
            Ativo = true
        };

        var pacienteDto = new PacienteDTO
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

        _repositoryMock.Setup(x => x.ObterPorIdAsync(1))
            .ReturnsAsync(paciente);
        _mapperMock.Setup(x => x.Map<PacienteDTO>(paciente))
            .Returns(pacienteDto);

        // Act
        var result = await _service.ObterPorIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Nome.Should().Be("João");
        _repositoryMock.Verify(x => x.ObterPorIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoPacienteNaoExiste_DeveRetornarNull()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ObterPorIdAsync(999))
            .ReturnsAsync((Paciente?)null);

        // Act
        var result = await _service.ObterPorIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveCriarPaciente()
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

        var paciente = new Paciente
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = Genero.Masculino,
            RG = "1234567",
            UfRG = Uf.SP,
            Email = "joao@email.com",
            Ativo = true
        };

        var pacienteDto = new PacienteDTO
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

        // O validator será executado normalmente, mas precisamos garantir que não há erros
        _repositoryMock.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), null))
            .ReturnsAsync(false);
        _convenioRepositoryMock.Setup(x => x.ObterPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Convenio?)null);
        _mapperMock.Setup(x => x.Map<Paciente>(createDto))
            .Returns(paciente);
        _repositoryMock.Setup(x => x.CriarAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(paciente);
        _repositoryMock.Setup(x => x.ObterPorIdAsync(1))
            .ReturnsAsync(paciente);
        _mapperMock.Setup(x => x.Map<PacienteDTO>(paciente))
            .Returns(pacienteDto);

        // Act
        var result = await _service.CriarAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("João");
        _repositoryMock.Verify(x => x.CriarAsync(It.IsAny<Paciente>()), Times.Once);
    }

    [Fact]
    public async Task CriarAsync_ComDadosInvalidos_DeveLancarValidationException()
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

        // O validator será executado normalmente e detectará o nome vazio
        _repositoryMock.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), null))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.CriarAsync(createDto));
    }

    [Fact]
    public async Task AtualizarAsync_ComDadosValidos_DeveAtualizarPaciente()
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

        var pacienteExistente = new Paciente
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = Genero.Masculino,
            RG = "1234567",
            UfRG = Uf.SP,
            Email = "joao@email.com",
            Ativo = true
        };

        var pacienteAtualizado = new Paciente
        {
            Id = 1,
            Nome = "João Atualizado",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = Genero.Masculino,
            RG = "1234567",
            UfRG = Uf.SP,
            Email = "joao@email.com",
            Ativo = true
        };

        var pacienteDto = new PacienteDTO
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

        _repositoryMock.SetupSequence(x => x.ObterPorIdAsync(1))
            .ReturnsAsync(pacienteExistente)
            .ReturnsAsync(pacienteAtualizado);
        _repositoryMock.Setup(x => x.ExisteCpfAsync(It.IsAny<string>(), It.IsAny<int?>()))
            .ReturnsAsync(false);
        _convenioRepositoryMock.Setup(x => x.ObterPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Convenio?)null);
        _repositoryMock.Setup(x => x.AtualizarAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(pacienteAtualizado);
        _mapperMock.Setup(x => x.Map<PacienteDTO>(pacienteAtualizado))
            .Returns(pacienteDto);

        // Act
        var result = await _service.AtualizarAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("João Atualizado");
        _repositoryMock.Verify(x => x.AtualizarAsync(It.IsAny<Paciente>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarAsync_QuandoPacienteNaoExiste_DeveLancarNotFoundException()
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

        _repositoryMock.Setup(x => x.ObterPorIdAsync(999))
            .ReturnsAsync((Paciente?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.AtualizarAsync(999, updateDto));
    }

    [Fact]
    public async Task DesativarAsync_QuandoPacienteExiste_DeveDesativar()
    {
        // Arrange
        var paciente = new Paciente
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = Genero.Masculino,
            RG = "1234567",
            UfRG = Uf.SP,
            Email = "joao@email.com",
            Ativo = true
        };

        _repositoryMock.Setup(x => x.ObterPorIdAsync(1))
            .ReturnsAsync(paciente);
        _repositoryMock.Setup(x => x.DesativarAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DesativarAsync(1);

        // Assert
        _repositoryMock.Verify(x => x.DesativarAsync(1), Times.Once);
    }

    [Fact]
    public async Task DesativarAsync_QuandoPacienteNaoExiste_DeveLancarNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ObterPorIdAsync(999))
            .ReturnsAsync((Paciente?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DesativarAsync(999));
    }

    [Fact]
    public async Task AtivarAsync_QuandoPacienteExiste_DeveAtivar()
    {
        // Arrange
        var paciente = new Paciente
        {
            Id = 1,
            Nome = "João",
            Sobrenome = "Silva",
            DataNascimento = new DateTime(1990, 1, 1),
            Genero = Genero.Masculino,
            RG = "1234567",
            UfRG = Uf.SP,
            Email = "joao@email.com",
            Ativo = false
        };

        _repositoryMock.Setup(x => x.ObterPorIdAsync(1))
            .ReturnsAsync(paciente);
        _repositoryMock.Setup(x => x.AtivarAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AtivarAsync(1);

        // Assert
        _repositoryMock.Verify(x => x.AtivarAsync(1), Times.Once);
    }

    [Fact]
    public async Task AtivarAsync_QuandoPacienteNaoExiste_DeveLancarNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(x => x.ObterPorIdAsync(999))
            .ReturnsAsync((Paciente?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.AtivarAsync(999));
    }
}

