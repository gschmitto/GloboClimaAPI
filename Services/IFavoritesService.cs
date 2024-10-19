using System.Collections.Generic;
using System.Threading.Tasks;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Interface para o serviço de favoritos, que permite gerenciar as cidades e países favoritos de um usuário.
    /// </summary>
    public interface IFavoritesService
    {
        /// <summary>
        /// Adiciona uma cidade aos favoritos de um usuário.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <param name="city">O modelo de cidade a ser adicionada aos favoritos.</param>
        /// <returns>Retorna um valor booleano indicando se a operação foi bem-sucedida.</returns>
        Task<bool> AddCityToFavorites(string userId, FavoriteCityModel city);

        /// <summary>
        /// Obtém a lista de cidades favoritas de um usuário.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <returns>Retorna uma lista de modelos de cidades favoritas.</returns>
        Task<List<FavoriteCityModel>> GetFavoriteCities(string userId);

        /// <summary>
        /// Remove uma cidade dos favoritos de um usuário.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <param name="cityName">O nome da cidade a ser removida dos favoritos.</param>
        /// <returns>Retorna um valor booleano indicando se a operação foi bem-sucedida.</returns>
        Task<bool> RemoveCityFromFavorites(string userId, string cityName);
    }
}
