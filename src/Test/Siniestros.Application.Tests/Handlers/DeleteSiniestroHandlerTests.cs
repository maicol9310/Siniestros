using Microsoft.Extensions.Logging;
using Moq;
using Siniestros.Application.Commands;
using Siniestros.Application.Handlers;
using Siniestros.Application.Interfaces;
using Siniestros.Domain.Aggregates;
using Siniestros.Domain.Enum;

namespace Siniestros.Application.Tests.Handlers
{
    [TestFixture]
    public class DeleteSiniestroHandlerTests
    {
        private Mock<ISiniestroRepository> _repoMock;
        private Mock<IUnitOfWork> _uowMock;
        private Mock<ILogger<DeleteSiniestroHandler>> _loggerMock;
        private DeleteSiniestroHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<ISiniestroRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteSiniestroHandler>>();

            _uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            _handler = new DeleteSiniestroHandler(
                _repoMock.Object,
                _uowMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_SiniestroExists_ShouldDelete()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Siniestro(
                         DateTime.UtcNow, "Depto", "Ciudad", TipoSiniestro.Atropello, 1, 0, "Desc"));

            var result = await _handler.Handle(new DeleteSiniestroCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_SiniestroNotFound_ShouldFail()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Siniestro)null);

            var result = await _handler.Handle(new DeleteSiniestroCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
