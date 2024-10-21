using Xunit;
using Moq;
using GloboClimaAPI.Controllers;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GloboClimaAPI.Tests
{
    public class CityControllerTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IFavoritesService> _favoritesServiceMock;
        private readonly CityController _controller;

        public CityControllerTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _favoritesServiceMock = new Mock<IFavoritesService>();
            _controller = new CityController(_httpClientFactoryMock.Object, _favoritesServiceMock.Object);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldReturnOk_WhenCityIsAddedSuccessfully()
        {
            // Arrange
            var cityModel = new FavoriteCityModel { CityName = "Cidade" };
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user@example.com")
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var operationResult = new OperationResult
            {
                Success = true,
                Message = "Cidade adicionada aos favoritos."
            };

            _favoritesServiceMock.Setup(service => service.AddCityToFavorites("user@example.com", cityModel))
                .ReturnsAsync(operationResult); // Retorna OperationResult

            // Act
            var result = await _controller.AddCityToFavorites(cityModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Cidade adicionada aos favoritos.", okResult.Value);
        }

        [Fact]
        public async Task GetFavoriteCities_ShouldReturnCities_WhenUserHasFavorites()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user@example.com")
            }));

            var favoriteCities = new List<FavoriteCityModel>
            {
                new FavoriteCityModel { CityName = "Cidade" }
            };

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _favoritesServiceMock.Setup(service => service.GetFavoriteCities("user@example.com"))
                .ReturnsAsync(favoriteCities);

            // Act
            var result = await _controller.GetFavoriteCities();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<List<FavoriteCityModel>>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public async Task DeleteCityFromFavorites_ShouldReturnOk_WhenCityIsDeletedSuccessfully()
        {
            // Arrange
            var cityName = "London";
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user@example.com")
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var operationResult = new OperationResult
            {
                Success = true,
                Message = "Cidade favorita removida com sucesso."
            };

            _favoritesServiceMock.Setup(service => service.RemoveCityFromFavorites("user@example.com", cityName))
                .ReturnsAsync(operationResult); // Retorna OperationResult

            // Act
            var result = await _controller.DeleteCityFromFavorites(cityName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Cidade favorita removida com sucesso.", okResult.Value);
        }
    }
}
