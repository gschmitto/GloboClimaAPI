using GloboClimaAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <param name="email">O email do usuário.</param>
        /// <param name="city">O modelo de cidade a ser adicionada aos favoritos.</param>
        /// <returns>Retorna um <see cref="OperationResult"/> indicando se a operação foi bem-sucedida.</returns>
        Task<OperationResult> AddCityToFavorites(string email, FavoriteCityModel city);

        /// <summary>
        /// Obtém a lista de cidades favoritas de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>Retorna uma lista de modelos de cidades favoritas.</returns>
        Task<List<FavoriteCityModel>> GetFavoriteCities(string email);

        /// <summary>
        /// Remove uma cidade dos favoritos de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="cityName">O nome da cidade a ser removida dos favoritos.</param>
        /// <returns>Retorna um <see cref="OperationResult"/> indicando se a operação foi bem-sucedida.</returns>
        Task<OperationResult> RemoveCityFromFavorites(string email, string cityName);
    }
}
