using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Siniestros.Application.Commands;
using Siniestros.Application.Handlers;
using Siniestros.Application.Interfaces;
using Siniestros.Domain.Entities;

namespace Siniestros.Application.Tests.Handlers
{
    [TestFixture]
    public class LoginHandlerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IConfiguration> _configMock;
        private Mock<ILogger<LoginHandler>> _loggerMock;
        private LoginHandler _handler;

        [SetUp]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<LoginHandler>>();

            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _configMock.Setup(c => c["Jwt:Key"]).Returns("super-secret-key-123456-1072527948-123456789");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("audience");

            _handler = new LoginHandler(
                _uowMock.Object,
                _configMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public async Task Handle_UserExists_ShouldReturnToken()
        {
            _userRepoMock.Setup(u => u.GetByUsernameAsync("admin", It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new User("admin", "hash"));

            var result = await _handler.Handle(new LoginCommand("admin", "123"), CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value.Token);
        }

        [Test]
        public void Handle_UserNotFound_ShouldThrow()
        {
            _userRepoMock.Setup(u => u.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((User)null);

            Assert.ThrowsAsync<NullReferenceException>(() =>
                _handler.Handle(new LoginCommand("x", "x"), CancellationToken.None));
        }
    }
}
