using GloboClimaAPI.Models;
using GloboClimaAPI.Data;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Implementação do serviço de favoritos, que gerencia as cidades e países favoritos de um usuário.
    /// </summary>
    public class FavoritesService : IFavoritesService
    {
        private readonly IUserFavoritesRepository _repository;

        /// <summary>
        /// Construtor com injeção do repositório.
        /// </summary>
        public FavoritesService(IUserFavoritesRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Adiciona uma cidade aos favoritos de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="city">O modelo de cidade a ser adicionada aos favoritos.</param>
        /// <returns>Retorna um valor booleano indicando se a operação foi bem-sucedida.</returns>
        public async Task<bool> AddCityToFavorites(string email, FavoriteCityModel city)
        {
            // Verifica se o email e a cidade fornecidos são válidos
            if (string.IsNullOrEmpty(email) || city == null)
            {
                return false;
            }

            // Verifica se o usuário já tem uma lista de favoritos
            var userFavorites = await _repository.GetUserFavoritesByEmailAsync(email);

            if (userFavorites == null)
            {
                // Cria uma nova lista de favoritos se o usuário não tiver
                userFavorites = new UserFavorites
                {
                    Email = email,
                    FavoriteCities = new List<FavoriteCityModel>()
                };
            }

            // Verifica se a cidade já está nos favoritos
            if (userFavorites.FavoriteCities.Any(c => c.CityName == city.CityName))
            {
                return false; // A cidade já está nos favoritos
            }

            // Adiciona a cidade aos favoritos
            userFavorites.FavoriteCities.Add(city);

            // Atualiza ou insere a lista de favoritos no banco de dados
            await _repository.SaveUserFavoritesAsync(userFavorites);

            return true;
        }

        /// <summary>
        /// Obtém a lista de cidades favoritas de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>Retorna uma lista de modelos de cidades favoritas.</returns>
        public async Task<List<FavoriteCityModel>> GetFavoriteCities(string email)
        {
            // Recupera os favoritos do usuário no banco de dados
            var userFavorites = await _repository.GetUserFavoritesByEmailAsync(email);

            // Retorna a lista de favoritos ou uma lista vazia caso não existam
            return userFavorites?.FavoriteCities ?? new List<FavoriteCityModel>();
        }

        /// <summary>
        /// Remove uma cidade dos favoritos de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="cityName">O nome da cidade a ser removida dos favoritos.</param>
        /// <returns>Retorna um valor booleano indicando se a operação foi bem-sucedida.</returns>
        public async Task<bool> RemoveCityFromFavorites(string email, string cityName)
        {
            // Recupera os favoritos do usuário
            var userFavorites = await _repository.GetUserFavoritesByEmailAsync(email);

            if (userFavorites == null)
            {
                return false; // Usuário não possui favoritos
            }

            // Verifica se a cidade está nos favoritos
            var city = userFavorites.FavoriteCities.FirstOrDefault(c => c.CityName == cityName);
            if (city != null)
            {
                // Remove a cidade dos favoritos
                userFavorites.FavoriteCities.Remove(city);

                // Atualiza a lista de favoritos no banco de dados
                await _repository.SaveUserFavoritesAsync(userFavorites);

                return true;
            }

            return false; // Cidade não encontrada nos favoritos
        }
    }
}
