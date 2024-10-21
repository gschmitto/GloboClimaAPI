using GloboClimaAPI.DTOs;
using GloboClimaAPI.Models;
using GloboClimaAPI.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Classe de serviço responsável pela gestão de usuários, incluindo registro, autenticação e favoritos.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="UserService"/> com o repositório de usuários e um logger.
        /// </summary>
        /// <param name="userRepository">O repositório para gerenciamento dos usuários.</param>
        /// <param name="logger">O logger para registrar informações e erros.</param>
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Verifica se um usuário com o email informado já existe.
        /// </summary>
        /// <param name="email">O email do usuário a ser verificado.</param>
        /// <returns>Um valor booleano indicando se o usuário existe ou não.</returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar se o usuário existe.");
                throw; // Propaga a exceção para o chamador
            }
        }

        /// <summary>
        /// Registra um novo usuário com base nos dados de registro fornecidos.
        /// </summary>
        /// <param name="userRegisterDto">Os dados de registro do usuário.</param>
        /// <returns>Um resultado da operação, incluindo o usuário registrado ou erro.</returns>
        public async Task<OperationResult> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null) 
                return new OperationResult { Success = false, Message = "Os dados de registro não podem ser nulos." };

            try
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
                _logger.LogInformation($"Usuário {user.Email} registrado com sucesso.");

                return new OperationResult { Success = true, Message = "Usuário registrado com sucesso.", Data = user };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário.");
                return new OperationResult { Success = false, Message = $"Erro ao registrar usuário: {ex.Message}" };
            }
        }

        /// <summary>
        /// Autentica um usuário com o email e senha fornecidos.
        /// </summary>
        /// <param name="email">O email do usuário.</param>
        /// <param name="password">A senha do usuário.</param>
        /// <returns>Um resultado da operação, incluindo o usuário autenticado ou erro.</returns>
        public async Task<OperationResult> AuthenticateUserAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return new OperationResult { Success = false, Message = "Email e senha não podem ser vazios." };

            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null) return new OperationResult { Success = false, Message = "Usuário não encontrado." };

                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i]) return new OperationResult { Success = false, Message = "Senha incorreta." };
                }

                _logger.LogInformation($"Usuário {email} autenticado com sucesso.");
                return new OperationResult { Success = true, Message = "Usuário autenticado com sucesso.", Data = user };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao autenticar usuário.");
                return new OperationResult { Success = false, Message = $"Erro ao autenticar usuário: {ex.Message}" };
            }
        }
    }
}
