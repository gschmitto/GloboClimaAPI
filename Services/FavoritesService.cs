using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Implementação do serviço de favoritos, que gerencia as cidades e países favoritos de um usuário.
    /// </summary>
    public class FavoritesService : IFavoritesService
    {
        // Um repositório simulado para armazenar as cidades favoritas.
        // Em um cenário real, você usaria um banco de dados.
        private readonly Dictionary<string, List<FavoriteCityModel>> _userFavorites = new();

        /// <summary>
        /// Adiciona uma cidade aos favoritos de um usuário.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <param name="city">O modelo de cidade a ser adicionada aos favoritos.</param>
        /// <returns>Retorna um valor booleano indicando se a operação foi bem-sucedida.</returns>
        public async Task<bool> AddCityToFavorites(string userId, FavoriteCityModel city)
        {
            // Verifica se o usuário já tem uma lista de favoritos
            if (!_userFavorites.ContainsKey(userId))
            {
                _userFavorites[userId] = new List<FavoriteCityModel>();
            }

            // Verifica se a cidade já está nos favoritos
            if (_userFavorites[userId].Any(c => c.CityName == city.CityName))
            {
                return false; // A cidade já está nos favoritos
            }

            // Adiciona a cidade aos favoritos
            _userFavorites[userId].Add(city);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Obtém a lista de cidades favoritas de um usuário.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <returns>Retorna uma lista de modelos de cidades favoritas.</returns>
        public async Task<List<FavoriteCityModel>> GetFavoriteCities(string userId)
        {
            // Retorna a lista de cidades favoritas do usuário
            if (_userFavorites.ContainsKey(userId))
            {
                return await Task.FromResult(_userFavorites[userId]);
            }

            // Retorna uma lista vazia caso o usuário não tenha favoritos
            return await Task.FromResult(new List<FavoriteCityModel>());
        }

        /// <summary>
        /// Remove uma cidade dos favoritos de um usuário.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <param name="cityName">O nome da cidade a ser removida dos favoritos.</param>
        /// <returns>Retorna um valor booleano indicando se a operação foi bem-sucedida.</returns>
        public async Task<bool> RemoveCityFromFavorites(string userId, string cityName)
        {
            // Verifica se o usuário tem a cidade nos favoritos
            if (_userFavorites.ContainsKey(userId))
            {
                var city = _userFavorites[userId].FirstOrDefault(c => c.CityName == cityName);
                if (city != null)
                {
                    // Remove a cidade dos favoritos
                    _userFavorites[userId].Remove(city);
                    return await Task.FromResult(true);
                }
            }

            return await Task.FromResult(false);
        }
    }
}
