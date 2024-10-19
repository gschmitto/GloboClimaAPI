using GloboClimaAPI.DTOs;
using GloboClimaAPI.Models;
using GloboClimaAPI.Data;
using System.Security.Cryptography;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace GloboClimaAPI.Services
{
    /// <summary>
    /// Classe de serviço responsável pela gestão de usuários, incluindo registro, autenticação e favoritos.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly UserFavoritesRepository _userFavoritesRepository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="UserService"/> com o contexto do DynamoDB e o repositório de favoritos do usuário.
        /// </summary>
        /// <param name="dynamoDbContext">O contexto do DynamoDB para acesso aos dados do usuário.</param>
        /// <param name="userFavoritesRepository">O repositório para gerenciamento dos favoritos do usuário.</param>
        public UserService(DynamoDBContext dynamoDbContext, UserFavoritesRepository userFavoritesRepository)
        {
            _dynamoDbContext = dynamoDbContext;
            _userFavoritesRepository = userFavoritesRepository;
        }

        /// <summary>
        /// Verifica se um usuário com o email informado já existe.
        /// </summary>
        /// <param name="email">O email do usuário a ser verificado.</param>
        /// <returns>Um valor booleano indicando se o usuário existe ou não.</returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _dynamoDbContext.LoadAsync<User>(email);
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

            await _dynamoDbContext.SaveAsync(user);

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
            var search = _dynamoDbContext.ScanAsync<User>(new List<ScanCondition>
            {
                new ScanCondition("Email", ScanOperator.Equal, email)
            });

            var users = await search.GetNextSetAsync();

            var user = users.FirstOrDefault();
            if (user == null) return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return null;
            }

            return user;
        }

        /// <summary>
        /// Recupera os favoritos de um usuário.
        /// </summary>
        /// <param name="userId">ID do usuário.</param>
        /// <returns>Os favoritos do usuário.</returns>
        public async Task<UserFavorites> GetUserFavoritesAsync(string userId)
        {
            return await _userFavoritesRepository.GetUserFavoritesAsync(userId);
        }

        /// <summary>
        /// Adiciona ou atualiza os favoritos de um usuário.
        /// </summary>
        /// <param name="userFavorites">Objeto contendo os favoritos do usuário.</param>
        public async Task SaveUserFavoritesAsync(UserFavorites userFavorites)
        {
            await _userFavoritesRepository.SaveUserFavoritesAsync(userFavorites);
        }

        /// <summary>
        /// Deleta os favoritos de um usuário.
        /// </summary>
        /// <param name="userId">ID do usuário para excluir os favoritos.</param>
        public async Task DeleteUserFavoritesAsync(string userId)
        {
            await _userFavoritesRepository.DeleteUserFavoritesAsync(userId);
        }
    }
}
