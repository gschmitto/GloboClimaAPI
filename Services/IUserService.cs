using GloboClimaAPI.Models;
using GloboClimaAPI.DTOs;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Interface para gerenciar os serviços de autenticação e registro de usuários.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Verifica se um usuário com o email informado já existe.
        /// </summary>
        /// <param name="email">O email do usuário a ser verificado.</param>
        /// <returns>Um valor booleano indicando se o usuário existe ou não.</returns>
        Task<bool> UserExistsAsync(string email);

        /// <summary>
        /// Registra um novo usuário com base nos dados de registro fornecidos.
        /// </summary>
        /// <param name="userRegisterDto">Os dados de registro do usuário.</param>
        /// <returns>O objeto do usuário registrado.</returns>
        Task<OperationResult> RegisterUserAsync(UserRegisterDto userRegisterDto);

        /// <summary>
        /// Autentica um usuário com o email e senha fornecidos.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <returns>O objeto do usuário autenticado, se bem-sucedido; caso contrário, null.</returns>
        Task<OperationResult> AuthenticateUserAsync(string email, string password);
    }
}
