using Amazon.DynamoDBv2.DataModel;

namespace GloboClimaAPI.Models
{
    /// <summary>
    /// Representa um usuário no sistema.
    /// </summary>
    [DynamoDBTable("Users")]
    public class User
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        /// <example>1</example>
        [DynamoDBProperty]
        public required string Id { get; set; }

        /// <summary>
        /// Email do usuário, utilizado para autenticação.
        /// </summary>
        /// <example>usuario@dominio.com</example>
        [DynamoDBHashKey]
        public required string Email { get; set; }

        /// <summary>
        /// Senha do usuário, armazenada como hash.
        /// </summary>
        /// <example>$2a$11$7kAyX...yYZ</example>
        [DynamoDBProperty]
        public required byte[] PasswordHash { get; set; }

        /// <summary>
        /// Salta (byte array) que é usado para calcular o hash da senha.
        /// </summary>
        [DynamoDBProperty]
        public required byte[] PasswordSalt { get; set; }
    }
}
