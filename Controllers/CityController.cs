using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GloboClimaAPI.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações relacionadas às cidades.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFavoritesService _favoritesService;
        private const string OpenWeatherMapApiKey = "f84f6a27103f990a32795cd8fcafdb4f";
        private const string OpenWeatherMapUrl = "http://api.openweathermap.org/data/2.5/weather?q={0}&appid=" + OpenWeatherMapApiKey;

        /// <summary>
        /// Construtor do controlador de cidades.
        /// </summary>
        /// <param name="httpClientFactory">Fábrica de clientes HTTP usada para fazer chamadas de API.</param>
        /// <param name="favoritesService">Serviço para gerenciar cidades favoritas.</param>
        public CityController(IHttpClientFactory httpClientFactory, IFavoritesService favoritesService)
        {
            _httpClientFactory = httpClientFactory;
            _favoritesService = favoritesService;
        }

        /// <summary>
        /// Consulta o clima de uma cidade usando a API OpenWeatherMap.
        /// </summary>
        /// <param name="cityName">Nome da cidade para a qual se deseja obter o clima.</param>
        /// <returns>Objeto contendo as informações do clima da cidade ou uma mensagem de erro.</returns>
        [HttpGet("weather/{cityName}")]
        public async Task<ActionResult<WeatherResponse>> GetWeather(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                return BadRequest("O nome da cidade não pode ser vazio.");
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetStringAsync(string.Format(OpenWeatherMapUrl, cityName));
                var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);
                return Ok(weatherData);
            }
            catch (HttpRequestException)
            {
                return NotFound($"Cidade {cityName} não encontrada.");
            }
            catch (JsonException)
            {
                return BadRequest("Erro ao processar os dados de clima.");
            }
        }

        /// <summary>
        /// Adiciona uma cidade aos favoritos do usuário.
        /// </summary>
        /// <param name="city">Modelo da cidade a ser adicionada aos favoritos.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost("favorites/city")]
        [Authorize]
        public async Task<ActionResult> AddCityToFavorites([FromBody] FavoriteCityModel city)
        {
            if (city == null)
            {
                return BadRequest("Dados da cidade não fornecidos.");
            }

            var email = GetUserEmail();
            if (email == null)
            {
                return Unauthorized("Email do usuário não encontrado no token.");
            }

            var result = await _favoritesService.AddCityToFavorites(email, city);
            bool success = result.Success;

            if (!success)
            {
                return BadRequest("Erro ao adicionar cidade aos favoritos.");
            }

            return Ok("Cidade adicionada aos favoritos.");
        }

        /// <summary>
        /// Obtém a lista de cidades favoritas do usuário.
        /// </summary>
        /// <returns>Lista de cidades favoritas ou mensagem de erro.</returns>
        [HttpGet("favorites/city")]
        [Authorize]
        public async Task<ActionResult> GetFavoriteCities()
        {
            var email = GetUserEmail();
            if (email == null)
            {
                return Unauthorized("Email do usuário não encontrado no token.");
            }

            var favoriteCities = await _favoritesService.GetFavoriteCities(email);
            if (favoriteCities == null || favoriteCities.Count == 0)
            {
                return NotFound("Você não tem cidades favoritas.");
            }

            return Ok(favoriteCities);
        }

        /// <summary>
        /// Deleta uma cidade dos favoritos do usuário.
        /// </summary>
        /// <param name="cityName">Nome da cidade a ser removida.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpDelete("favorites/city/{cityName}")]
        [Authorize]
        public async Task<ActionResult> DeleteCityFromFavorites(string cityName)
        {
            var email = GetUserEmail();
            if (email == null)
            {
                return Unauthorized("Email do usuário não encontrado no token.");
            }

            var result = await _favoritesService.RemoveCityFromFavorites(email, cityName);
            bool success = result.Success;
            
            if (!success)
            {
                return NotFound($"Cidade {cityName} não encontrada nos favoritos.");
            }

            return Ok("Cidade favorita removida com sucesso.");
        }

        /// <summary>
        /// Obtém o email do usuário a partir das claims.
        /// </summary>
        /// <returns>Email do usuário ou null se não encontrado.</returns>
        private string? GetUserEmail()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
