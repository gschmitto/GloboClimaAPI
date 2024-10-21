using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GloboClimaAPI.Models;

namespace GloboClimaAPI.Data
{
    /// <summary>
    /// Repositório de usuários para gerenciamento de dados persistentes no DynamoDB.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="dynamoDbContext">O contexto do DynamoDB para operações de banco de dados.</param>
        /// <param name="logger">O logger para registrar mensagens.</param>
        public UserRepository(DynamoDBContext dynamoDbContext, ILogger<UserRepository> logger)
        {
            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Obtém um usuário pelo seu email.
        /// </summary>
        /// <param name="email">O email do usuário a ser recuperado.</param>
        /// <returns>Um objeto <see cref="User"/> se encontrado; caso contrário, null.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _dynamoDbContext.LoadAsync<User>(email);
                if (user == null)
                {
                    _logger.LogWarning($"Usuário com email '{email}' não encontrado.");
                }
                return user;
            }
            catch (AmazonDynamoDBException dbEx)
            {
                _logger.LogError($"Erro de comunicação com o DynamoDB: {dbEx.Message}");
                throw new Exception("Falha de comunicação com o banco de dados.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado ao tentar recuperar o usuário: {ex.Message}");
                throw new Exception("Erro inesperado ao tentar recuperar o usuário.");
            }
        }

        /// <summary>
        /// Adiciona um novo usuário ao repositório.
        /// </summary>
        /// <param name="user">O objeto <see cref="User"/> a ser adicionado.</param>
        /// <returns>O objeto <see cref="User"/> adicionado.</returns>
        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                await _dynamoDbContext.SaveAsync(user);
                _logger.LogInformation($"Usuário '{user.Email}' adicionado com sucesso.");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar usuário: {ex.Message}");
                throw new Exception("Erro ao adicionar usuário.");
            }
        }

        /// <summary>
        /// Verifica se um usuário existe com base no email fornecido.
        /// </summary>
        /// <param name="email">O email do usuário a ser verificado.</param>
        /// <returns>Um valor booleano indicando se o usuário existe ou não.</returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                var user = await _dynamoDbContext.LoadAsync<User>(email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao verificar se o usuário existe: {ex.Message}");
                throw new Exception("Erro ao verificar se o usuário existe.");
            }
        }
    }
}
