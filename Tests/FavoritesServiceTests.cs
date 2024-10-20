using GloboClimaAPI.Data;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Moq;
using Xunit;

namespace GloboClimaAPI.Tests
{
    public class FavoritesServiceTests
    {
        private readonly Mock<IUserFavoritesRepository> _mockRepository; // Use a interface aqui
        private readonly FavoritesService _service;

        public FavoritesServiceTests()
        {
            _mockRepository = new Mock<IUserFavoritesRepository>(); // Corrigido para Mock da interface
            _service = new FavoritesService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldReturnFalse_WhenEmailIsNull()
        {
            // Arrange
            string email = null;
            var city = new FavoriteCityModel { CityName = "São Paulo" };

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldReturnFalse_WhenCityIsNull()
        {
            // Arrange
            string email = "test@example.com";
            FavoriteCityModel city = null;

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldCreateNewFavorites_WhenUserHasNoFavorites()
        {
            // Arrange
            var email = "test@example.com";
            var city = new FavoriteCityModel { CityName = "São Paulo" };
            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync((UserFavorites)null);

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.SaveUserFavoritesAsync(It.IsAny<UserFavorites>()), Times.Once);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldReturnFalse_WhenCityAlreadyExists()
        {
            // Arrange
            var email = "test@example.com";
            var city = new FavoriteCityModel { CityName = "São Paulo" };
            var userFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel> { city }
            };

            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync(userFavorites);

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.SaveUserFavoritesAsync(It.IsAny<UserFavorites>()), Times.Never);
        }

        [Fact]
        public async Task AddCityToFavorites_ShouldAddCity_WhenValidInput()
        {
            // Arrange
            var email = "test@example.com";
            var city = new FavoriteCityModel { CityName = "São Paulo" };
            var userFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel>()
            };

            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync(userFavorites);

            // Act
            var result = await _service.AddCityToFavorites(email, city);

            // Assert
            Assert.True(result);
            Assert.Contains(city, userFavorites.FavoriteCities);
            _mockRepository.Verify(repo => repo.SaveUserFavoritesAsync(userFavorites), Times.Once);
        }

        [Fact]
        public async Task GetFavoriteCities_ShouldReturnEmptyList_WhenUserHasNoFavorites()
        {
            // Arrange
            var email = "test@example.com";
            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync((UserFavorites)null);

            // Act
            var result = await _service.GetFavoriteCities(email);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFavoriteCities_ShouldReturnListOfFavorites_WhenUserHasFavorites()
        {
            // Arrange
            var email = "test@example.com";
            var favorites = new List<FavoriteCityModel>
            {
                new FavoriteCityModel { CityName = "São Paulo" },
                new FavoriteCityModel { CityName = "Rio de Janeiro" }
            };

            var userFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = favorites
            };

            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync(userFavorites);

            // Act
            var result = await _service.GetFavoriteCities(email);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.CityName == "São Paulo");
            Assert.Contains(result, c => c.CityName == "Rio de Janeiro");
        }

        [Fact]
        public async Task RemoveCityFromFavorites_ShouldReturnFalse_WhenUserHasNoFavorites()
        {
            // Arrange
            var email = "test@example.com";
            var cityName = "São Paulo";
            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync((UserFavorites)null);

            // Act
            var result = await _service.RemoveCityFromFavorites(email, cityName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveCityFromFavorites_ShouldReturnFalse_WhenCityDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            var cityName = "São Paulo";
            var userFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel>()
            };

            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync(userFavorites);

            // Act
            var result = await _service.RemoveCityFromFavorites(email, cityName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveCityFromFavorites_ShouldRemoveCity_WhenCityExists()
        {
            // Arrange
            var email = "test@example.com";
            var cityName = "São Paulo";
            var cityToRemove = new FavoriteCityModel { CityName = cityName };
            var userFavorites = new UserFavorites
            {
                Email = email,
                FavoriteCities = new List<FavoriteCityModel> { cityToRemove }
            };

            _mockRepository.Setup(repo => repo.GetUserFavoritesByEmailAsync(email)).ReturnsAsync(userFavorites);

            // Act
            var result = await _service.RemoveCityFromFavorites(email, cityName);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(cityToRemove, userFavorites.FavoriteCities);
            _mockRepository.Verify(repo => repo.SaveUserFavoritesAsync(userFavorites), Times.Once);
        }
    }
}
