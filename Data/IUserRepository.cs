using GloboClimaAPI.Models;

namespace GloboClimaAPI.Data
{
    /// <summary>
    /// Define os métodos para o repositório de usuários.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtém um usuário pelo seu email.
        /// </summary>
        /// <param name="email">O email do usuário a ser recuperado.</param>
        /// <returns>
        /// Um objeto <see cref="User"/> se encontrado; caso contrário, null.
        /// </returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Adiciona um novo usuário ao repositório.
        /// </summary>
        /// <param name="user">O objeto <see cref="User"/> a ser adicionado.</param>
        /// <returns>
        /// O objeto <see cref="User"/> adicionado.
        /// </returns>
        Task<User> AddUserAsync(User user);

        /// <summary>
        /// Verifica se um usuário existe com base no email fornecido.
        /// </summary>
        /// <param name="email">O email do usuário a ser verificado.</param>
        /// <returns>
        /// Um valor booleano indicando se o usuário existe ou não.
        /// </returns>
        Task<bool> UserExistsAsync(string email);
    }
}
