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
        private readonly ILogger<FavoritesService> _logger;

        /// <summary>
        /// Construtor com injeção do repositório e logger.
        /// </summary>
        public FavoritesService(IUserFavoritesRepository repository, ILogger<FavoritesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Adiciona uma cidade aos favoritos de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="city">O modelo de cidade a ser adicionada aos favoritos.</param>
        /// <returns>Retorna um resultado da operação, contendo sucesso e mensagem.</returns>
        public async Task<OperationResult> AddCityToFavorites(string email, FavoriteCityModel city)
        {
            try
            {
                if (city == null) throw new ArgumentNullException(nameof(city));

                var userFavorites = await GetOrCreateUserFavorites(email);

                // Verifica se a cidade já está nos favoritos
                if (userFavorites.FavoriteCities.Any(c => c.CityName == city.CityName))
                {
                    _logger.LogWarning($"A cidade {city.CityName} já está nos favoritos do usuário {email}.");
                    return new OperationResult { Success = false, Message = "A cidade já está nos favoritos."};
                }

                // Adiciona a cidade aos favoritos
                userFavorites.FavoriteCities.Add(city);
                await _repository.SaveUserFavoritesAsync(userFavorites);

                _logger.LogInformation($"Cidade {city.CityName} adicionada aos favoritos do usuário {email}.");
                return new OperationResult { Success = true, Message = "Cidade adicionada aos favoritos com sucesso." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar cidade aos favoritos.");
                return new OperationResult { Success = false, Message = $"Erro ao adicionar cidade: {ex.Message}" };
            }
        }

        /// <summary>
        /// Obtém a lista de cidades favoritas de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>Retorna uma lista de modelos de cidades favoritas.</returns>
        public async Task<List<FavoriteCityModel>> GetFavoriteCities(string email)
        {
            try
            {
                // Recupera os favoritos do usuário no banco de dados
                var userFavorites = await _repository.GetUserFavoritesByEmailAsync(email);

                _logger.LogInformation($"Favoritos do usuário {email} foram obtidos com sucesso.");
                // Retorna a lista de favoritos ou uma lista vazia caso não existam
                return userFavorites?.FavoriteCities ?? new List<FavoriteCityModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter cidades favoritas.");
                throw;
            }
        }

        /// <summary>
        /// Remove uma cidade dos favoritos de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="cityName">O nome da cidade a ser removida dos favoritos.</param>
        /// <returns>Retorna um resultado da operação, contendo sucesso e mensagem.</returns>
        public async Task<OperationResult> RemoveCityFromFavorites(string email, string cityName)
        {
            try
            {
                var userFavorites = await _repository.GetUserFavoritesByEmailAsync(email);

                if (userFavorites == null)
                {
                    _logger.LogWarning($"Usuário {email} não possui favoritos.");
                    return new OperationResult { Success = false, Message = "Usuário não possui favoritos." };
                }

                // Verifica se a cidade está nos favoritos
                var city = userFavorites.FavoriteCities.FirstOrDefault(c => c.CityName == cityName);
                if (city != null)
                {
                    // Remove a cidade dos favoritos
                    userFavorites.FavoriteCities.Remove(city);
                    await _repository.SaveUserFavoritesAsync(userFavorites);

                    _logger.LogInformation($"Cidade {cityName} removida dos favoritos do usuário {email}.");
                    return new OperationResult { Success = true, Message = "Cidade removida dos favoritos com sucesso." };
                }

                _logger.LogWarning($"Cidade {cityName} não encontrada nos favoritos do usuário {email}.");
                return new OperationResult { Success = false, Message = "Cidade não encontrada nos favoritos." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover cidade dos favoritos.");
                return new OperationResult { Success = false, Message = $"Erro ao remover cidade: {ex.Message}" };
            }
        }

        /// <summary>
        /// Método auxiliar para recuperar ou criar uma lista de favoritos de um usuário.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <returns>Retorna o objeto de favoritos do usuário.</returns>
        private async Task<UserFavorites> GetOrCreateUserFavorites(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email não pode ser vazio", nameof(email));

            var userFavorites = await _repository.GetUserFavoritesByEmailAsync(email);

            if (userFavorites == null)
            {
                userFavorites = new UserFavorites
                {
                    Email = email,
                    FavoriteCities = new List<FavoriteCityModel>()
                };
            }

            return userFavorites;
        }
    }

    /// <summary>
    /// Classe que encapsula o resultado de uma operação, indicando seu sucesso e uma mensagem associada.
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Indica se a operação foi bem-sucedida ou não.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensagem detalhada sobre o resultado da operação.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dados adicionais relacionados à operação.
        /// </summary>
        public object Data { get; set; }
    }
}
