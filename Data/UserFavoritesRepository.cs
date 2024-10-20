using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Data
{
    /// <summary>
    /// Repositório de favoritos do usuário com operações CRUD utilizando o DynamoDB.
    /// </summary>
    public class UserFavoritesRepository : IUserFavoritesRepository // Implementando a interface
    {
        private readonly DynamoDBContext _context;
        private readonly ILogger<UserFavoritesRepository> _logger;

        /// <summary>
        /// Construtor do repositório. Inicializa o contexto do DynamoDB e o logger.
        /// </summary>
        /// <param name="dynamoDbClient">Cliente do DynamoDB.</param>
        /// <param name="logger">O logger para registrar mensagens.</param>
        public UserFavoritesRepository(IAmazonDynamoDB dynamoDbClient, ILogger<UserFavoritesRepository> logger)
        {
            _context = new DynamoDBContext(dynamoDbClient);
            _logger = logger;
        }

        /// <summary>
        /// Salva os favoritos de um usuário no DynamoDB.
        /// </summary>
        /// <param name="userFavorites">Objeto contendo os favoritos do usuário.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        public async Task SaveUserFavoritesAsync(UserFavorites userFavorites)
        {
            try
            {
                await _context.SaveAsync(userFavorites);
                _logger.LogInformation($"Favoritos do usuário '{userFavorites.Email}' salvos com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao salvar favoritos do usuário: {ex.Message}");
                throw new Exception("Erro ao salvar favoritos do usuário.");
            }
        }

        /// <summary>
        /// Recupera os favoritos de um usuário do DynamoDB com base no ID do usuário.
        /// </summary>
        /// <param name="userId">ID do usuário para buscar os favoritos.</param>
        /// <returns>Objeto contendo os favoritos do usuário.</returns>
        public async Task<UserFavorites> GetUserFavoritesAsync(string userId)
        {
            try
            {
                var userFavorites = await _context.LoadAsync<UserFavorites>(userId);
                if (userFavorites == null)
                {
                    _logger.LogWarning($"Favoritos do usuário com ID '{userId}' não encontrados.");
                }
                return userFavorites;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao recuperar favoritos do usuário: {ex.Message}");
                throw new Exception("Erro ao recuperar favoritos do usuário.");
            }
        }

        /// <summary>
        /// Deleta os favoritos de um usuário do DynamoDB com base no ID do usuário.
        /// </summary>
        /// <param name="userId">ID do usuário para excluir os favoritos.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        public async Task DeleteUserFavoritesAsync(string userId)
        {
            try
            {
                await _context.DeleteAsync<UserFavorites>(userId);
                _logger.LogInformation($"Favoritos do usuário com ID '{userId}' excluídos com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar favoritos do usuário: {ex.Message}");
                throw new Exception("Erro ao deletar favoritos do usuário.");
            }
        }

        /// <summary>
        /// Recupera os favoritos de um usuário do DynamoDB com base no email.
        /// </summary>
        /// <param name="email">Email do usuário para buscar os favoritos.</param>
        /// <returns>Objeto contendo os favoritos do usuário ou null se não existir.</returns>
        public async Task<UserFavorites?> GetUserFavoritesByEmailAsync(string email)
        {
            try
            {
                var scanConditions = new List<ScanCondition>
                {
                    new ScanCondition(nameof(UserFavorites.Email), ScanOperator.Equal, email)
                };

                var search = _context.ScanAsync<UserFavorites>(scanConditions);
                var results = await search.GetRemainingAsync();

                return results.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao recuperar favoritos pelo email '{email}': {ex.Message}");
                throw new Exception("Erro ao recuperar favoritos pelo email.");
            }
        }

        /// <summary>
        /// Verifica se um registro de favoritos de um usuário existe no DynamoDB com base no email.
        /// </summary>
        /// <param name="email">Email do usuário para verificar.</param>
        /// <returns>True se o registro existir, false caso contrário.</returns>
        public async Task<bool> UserFavoritesExistsAsync(string email)
        {
            try
            {
                var userFavorites = await GetUserFavoritesByEmailAsync(email);
                return userFavorites != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao verificar se os favoritos do usuário com email '{email}' existem: {ex.Message}");
                throw new Exception("Erro ao verificar se os favoritos do usuário existem.");
            }
        }
    }
}
