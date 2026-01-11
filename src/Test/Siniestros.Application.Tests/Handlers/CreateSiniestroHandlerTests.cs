using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Siniestros.Application.Commands;
using Siniestros.Application.Handlers;
using Siniestros.Application.Interfaces;
using Siniestros.Contracts.DTOs;
using Siniestros.Domain.Aggregates;
using Siniestros.Domain.Enum;

namespace Siniestros.Application.Tests.Handlers
{
    [TestFixture]
    public class CreateSiniestroHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<ISiniestroRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<CreateSiniestroHandler>> _loggerMock;

        private CreateSiniestroHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _repoMock = new Mock<ISiniestroRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateSiniestroHandler>>();

            _uowMock.Setup(u => u.Siniestro).Returns(_repoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            _handler = new CreateSiniestroHandler(
                _uowMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccess()
        {
            var command = new CreateSiniestroCommand(
                FechaHora: DateTime.UtcNow,
                Departamento: "Córdoba",
                Ciudad: "Lorica",
                Tipo: TipoSiniestro.Atropello,
                VehiculosInvolucrados: 2,
                NumeroVictimas: 1,
                Descripcion: "Choque leve"
            );

            _mapperMock.Setup(m => m.Map<SiniestroDto>(It.IsAny<Siniestro>()))
                       .Returns(new SiniestroDto { Departamento = "Córdoba" });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Siniestro>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_SaveChangesThrows_ShouldReturnFailure()
        {
            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception("DB error"));

            var command = new CreateSiniestroCommand(
                DateTime.UtcNow,
                "Antioquia",
                "Medellín",
                TipoSiniestro.Atropello,
                1,
                0,
                "Desc"
            );

            Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

    }
}
