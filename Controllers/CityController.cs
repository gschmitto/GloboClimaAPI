using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using GloboClimaAPI.Helpers;


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
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(string.Format(OpenWeatherMapUrl, cityName));

            if (string.IsNullOrEmpty(response))
            {
                return NotFound($"Cidade {cityName} não encontrada.");
            }

            var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);
            return Ok(weatherData);
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
            var userId = User.Identity.Name;

            if (city == null)
            {
                return BadRequest("Dados da cidade não fornecidos.");
            }

            bool success = await _favoritesService.AddCityToFavorites(userId, city);
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
            var userId = User.Identity.Name;

            var favoriteCities = await _favoritesService.GetFavoriteCities(userId);
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
            var userId = User.Identity.Name;

            bool success = await _favoritesService.RemoveCityFromFavorites(userId, cityName);
            if (!success)
            {
                return NotFound($"Cidade com ID {cityName} não encontrada nos favoritos.");
            }

            return Ok(ApiMessages.CityRemovedFromFavorites);
        }
    }
}
