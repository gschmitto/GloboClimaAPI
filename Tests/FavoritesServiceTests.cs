using GloboClimaAPI.Models;
using GloboClimaAPI.Data;
using GloboClimaAPI.Services;
using Moq;
using Xunit;

namespace GloboClimaAPI.Tests
{
    public class FavoritesServiceTests
    {
        private readonly Mock<IUserFavoritesRepository> _repositoryMock;
        private readonly Mock<ILogger<FavoritesService>> _loggerMock;
        private readonly FavoritesService _service;

        public FavoritesServiceTests()
        {
            _repositoryMock = new Mock<IUserFavoritesRepository>();
            _loggerMock = new Mock<ILogger<FavoritesService>>();
            _service = new FavoritesService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldAddCity_WhenCityIsValid()
        {
            // Arrange
            var email = "user@example.com";
            var city = new FavoriteCityModel { CityName = "City1" };
            _repositoryMock.Setup(r => r.GetUserFavoritesByEmailAsync(email))
                .ReturnsAsync(new UserFavorites { Email = email, FavoriteCities = new List<FavoriteCityModel>() });

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cidade adicionada aos favoritos com sucesso.", result.Message);
            _repositoryMock.Verify(r => r.SaveUserFavoritesAsync(It.IsAny<UserFavorites>()), Times.Once);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldReturnError_WhenCityIsNull()
        {
            // Arrange
            var email = "user@example.com";

            // Act
            var result = await _service.AddCityToFavorites(email, null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Erro ao adicionar cidade: Value cannot be null. (Parameter 'city')", result.Message);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldReturnError_WhenCityAlreadyExists()
        {
            // Arrange
            var email = "user@example.com";
            var city = new FavoriteCityModel { CityName = "City1" };
            var existingFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel> { city }
            };
            _repositoryMock.Setup(r => r.GetUserFavoritesByEmailAsync(email))
                .ReturnsAsync(existingFavorites);

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("A cidade já está nos favoritos.", result.Message);
        }

        [Fact]
        public async Task RemoveCityFromFavorites_ShouldRemoveCity_WhenCityExists()
        {
            // Arrange
            var email = "user@example.com";
            var cityName = "City1";
            var city = new FavoriteCityModel { CityName = cityName };
            var existingFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel> { city }
            };
            _repositoryMock.Setup(r => r.GetUserFavoritesByEmailAsync(email))
                .ReturnsAsync(existingFavorites);

            // Act
            var result = await _service.RemoveCityFromFavorites(email, cityName);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Cidade removida dos favoritos com sucesso.", result.Message);
            _repositoryMock.Verify(r => r.SaveUserFavoritesAsync(It.IsAny<UserFavorites>()), Times.Once);
        }

        [Fact]
        public async Task RemoveCityFromFavorites_ShouldReturnError_WhenCityDoesNotExist()
        {
            // Arrange
            var email = "user@example.com";
            var cityName = "City1";
            var existingFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel>()
            };
            _repositoryMock.Setup(r => r.GetUserFavoritesByEmailAsync(email))
                .ReturnsAsync(existingFavorites);

            // Act
            var result = await _service.RemoveCityFromFavorites(email, cityName);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Cidade não encontrada nos favoritos.", result.Message);
        }

        [Fact]
        public async Task GetFavoriteCities_ShouldReturnFavoriteCities_WhenUserHasFavorites()
        {
            // Arrange
            var email = "user@example.com";
            var cities = new List<FavoriteCityModel>
            {
                new FavoriteCityModel { CityName = "City1" },
                new FavoriteCityModel { CityName = "City2" }
            };
            var existingFavorites = new UserFavorites { Email = email, FavoriteCities = cities };
            _repositoryMock.Setup(r => r.GetUserFavoritesByEmailAsync(email))
                .ReturnsAsync(existingFavorites);

            // Act
            var result = await _service.GetFavoriteCities(email);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.CityName == "City1");
            Assert.Contains(result, c => c.CityName == "City2");
        }

        [Fact]
        public async Task GetFavoriteCities_ShouldReturnEmptyList_WhenUserHasNoFavorites()
        {
            // Arrange
            var email = "user@example.com";
            var existingFavorites = new UserFavorites { Email = email, FavoriteCities = new List<FavoriteCityModel>() };
            _repositoryMock.Setup(r => r.GetUserFavoritesByEmailAsync(email))
                .ReturnsAsync(existingFavorites);

            // Act
            var result = await _service.GetFavoriteCities(email);

            // Assert
            Assert.Empty(result);
        }
    }
}
