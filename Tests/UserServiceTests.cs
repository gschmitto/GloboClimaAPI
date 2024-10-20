using GloboClimaAPI.Data;
using GloboClimaAPI.DTOs;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Moq;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using Microsoft.Extensions.Logging;

namespace GloboClimaAPI.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        private readonly Mock<ILogger<UserService>> _loggerMock;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_mockUserRepository.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = new byte[64],
                PasswordSalt = new byte[128]
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.UserExistsAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UserExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.UserExistsAsync(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnRegisteredUser()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto { Email = "test@example.com", Password = "password" };
            User registeredUser = null;

            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .Callback<User>(user => registeredUser = user);

            // Act
            var result = await _userService.RegisterUserAsync(userRegisterDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuário registrado com sucesso.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(userRegisterDto.Email, ((User)result.Data).Email);
            Assert.NotNull(registeredUser.PasswordHash);
            Assert.NotNull(registeredUser.PasswordSalt);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var hmac = new HMACSHA512();
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.AuthenticateUserAsync(email, password);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuário autenticado com sucesso.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(email, ((User)result.Data).Email);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFailure_WhenCredentialsAreInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            var hmac = new HMACSHA512();
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password")), // Senha correta
                PasswordSalt = hmac.Key
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.AuthenticateUserAsync(email, password);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Senha incorreta.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";

            _mockUserRepository.Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.AuthenticateUserAsync(email, password);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado.", result.Message);
            Assert.Null(result.Data);
        }
    }
}
