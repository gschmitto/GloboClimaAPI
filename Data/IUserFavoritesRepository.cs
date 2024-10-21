using GloboClimaAPI.Models;

namespace GloboClimaAPI.Data
{
    /// <summary>
    /// Interface para o repositório de favoritos do usuário com operações CRUD.
    /// </summary>
    public interface IUserFavoritesRepository
    {
        /// <summary>
        /// Salva os favoritos de um usuário.
        /// </summary>
        /// <param name="userFavorites">Objeto contendo os favoritos do usuário.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        Task SaveUserFavoritesAsync(UserFavorites userFavorites);

        /// <summary>
        /// Recupera os favoritos de um usuário com base no ID.
        /// </summary>
        /// <param name="userId">ID do usuário para buscar os favoritos.</param>
        /// <returns>Objeto contendo os favoritos do usuário.</returns>
        Task<UserFavorites> GetUserFavoritesAsync(string userId);

        /// <summary>
        /// Deleta os favoritos de um usuário.
        /// </summary>
        /// <param name="userId">ID do usuário para excluir os favoritos.</param>
        /// <returns>Uma tarefa assíncrona.</returns>
        Task DeleteUserFavoritesAsync(string userId);

        /// <summary>
        /// Recupera os favoritos de um usuário com base no email.
        /// </summary>
        /// <param name="email">Email do usuário para buscar os favoritos.</param>
        /// <returns>Objeto contendo os favoritos do usuário ou null se não existir.</returns>
        Task<UserFavorites?> GetUserFavoritesByEmailAsync(string email);

        /// <summary>
        /// Verifica se um registro de favoritos de um usuário existe com base no email.
        /// </summary>
        /// <param name="email">Email do usuário para verificar.</param>
        /// <returns>True se o registro existir, false caso contrário.</returns>
        Task<bool> UserFavoritesExistsAsync(string email);
    }
}
