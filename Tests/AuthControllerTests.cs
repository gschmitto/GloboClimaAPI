using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using GloboClimaAPI.Controllers;
using GloboClimaAPI.Services;
using GloboClimaAPI.DTOs;
using GloboClimaAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace GloboClimaAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_userServiceMock.Object, _configurationMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Email = "test@example.com",
                Password = "Password123"
            };
            
            var hmac = new HMACSHA512();
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userRegisterDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password)),
                PasswordSalt = hmac.Key
            };

            _userServiceMock.Setup(u => u.UserExistsAsync(userRegisterDto.Email)).ReturnsAsync(false);
            _userServiceMock.Setup(u => u.RegisterUserAsync(userRegisterDto))
                .ReturnsAsync(new OperationResult { Success = true, Message = "Usuário registrado com sucesso.", Data = user });

            // Act
            var result = await _controller.Register(userRegisterDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic returnValue = okResult.Value; // Mudei para evitar Assert.IsType<dynamic>
            Assert.Equal("Usuário registrado com sucesso.", returnValue.message);
        }

        [Fact]
        public async Task Register_UserAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Email = "test@example.com",
                Password = "Password123",
            };

            _userServiceMock.Setup(u => u.UserExistsAsync(userRegisterDto.Email)).ReturnsAsync(true);

            // Act
            var result = await _controller.Register(userRegisterDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Usuário já existe.", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "Password123",
            };

            var hmac = new HMACSHA512();
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userLoginDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password)),
                PasswordSalt = hmac.Key
            };

            _userServiceMock.Setup(u => u.AuthenticateUserAsync(userLoginDto.Email, userLoginDto.Password))
                .ReturnsAsync(new OperationResult { Success = true, Message = "Usuário autenticado com sucesso.", Data = user });

            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("this_is_a_32_byte_long_key__teeeee");
            _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("test_issuer");

            // Act
            var result = await _controller.Login(userLoginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<LoginResponseDto>(okResult.Value); // Atualizado para o tipo específico
            Assert.NotNull(returnValue.Token); // Verifica se o token não é nulo
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword",
            };

            _userServiceMock.Setup(u => u.AuthenticateUserAsync(userLoginDto.Email, userLoginDto.Password))
                .ReturnsAsync(new OperationResult { Success = false, Message = "Credenciais inválidas." });

            // Act
            var result = await _controller.Login(userLoginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Credenciais inválidas.", unauthorizedResult.Value);
        }
    }
}
