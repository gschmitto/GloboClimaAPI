using GloboClimaAPI.DTOs;
using GloboClimaAPI.Models;
using GloboClimaAPI.Data;
using System.Security.Cryptography;
using System.Text;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Classe de serviço responsável pela gestão de usuários, incluindo registro, autenticação e favoritos.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="UserService"/> com o repositório de usuários e o repositório de favoritos do usuário.
        /// </summary>
        /// <param name="userRepository">O repositório para gerenciamento dos usuários.</param></param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Verifica se um usuário com o email informado já existe.
        /// </summary>
        /// <param name="email">O email do usuário a ser verificado.</param>
        /// <returns>Um valor booleano indicando se o usuário existe ou não.</returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null;
        }

        /// <summary>
        /// Registra um novo usuário com base nos dados de registro fornecidos.
        /// </summary>
        /// <param name="userRegisterDto">Os dados de registro do usuário.</param>
        /// <returns>O objeto do usuário registrado.</returns>
        public async Task<User> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userRegisterDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _userRepository.AddUserAsync(user);

            return user;
        }

        /// <summary>
        /// Autentica um usuário com o email e senha fornecidos.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <returns>O objeto do usuário autenticado, se bem-sucedido; caso contrário, null.</returns>
        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return null;
            }

            return user;
        }
    }
}
