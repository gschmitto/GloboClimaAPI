using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Data
{
    /// <summary>
    /// Repositório de favoritos do usuário com operações CRUD utilizando o DynamoDB.
    /// </summary>
    public class UserFavoritesRepository
    {
        private readonly DynamoDBContext _context;

        /// <summary>
        /// Construtor do repositório. Inicializa o contexto do DynamoDB.
        /// </summary>
        public UserFavoritesRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        /// <summary>
        /// Salva os favoritos de um usuário no DynamoDB.
        /// </summary>
        /// <param name="userFavorites">Objeto contendo os favoritos do usuário.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        public async Task SaveUserFavoritesAsync(UserFavorites userFavorites)
        {
            await _context.SaveAsync(userFavorites);
        }

        /// <summary>
        /// Recupera os favoritos de um usuário do DynamoDB.
        /// </summary>
        /// <param name="userId">ID do usuário para buscar os favoritos.</param>
        /// <returns>Objeto contendo os favoritos do usuário.</returns>
        public async Task<UserFavorites> GetUserFavoritesAsync(string userId)
        {
            return await _context.LoadAsync<UserFavorites>(userId);
        }

        /// <summary>
        /// Deleta os favoritos de um usuário do DynamoDB.
        /// </summary>
        /// <param name="userId">ID do usuário para excluir os favoritos.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        public async Task DeleteUserFavoritesAsync(string userId)
        {
            await _context.DeleteAsync<UserFavorites>(userId);
        }
    }
}
