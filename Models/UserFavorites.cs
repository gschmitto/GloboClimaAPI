namespace GloboClimaAPI.Models
{
    /// <summary>
    /// Representa os favoritos de um usuário, incluindo cidades e países.
    /// </summary>
    public class UserFavorites
    {
        /// <summary>
        /// O identificador único do usuário.
        /// </summary>
        /// <example>user123</example>
        public string UserId { get; set; }

        /// <summary>
        /// A lista de cidades favoritas do usuário.
        /// </summary>
        /// <example>["New York", "Los Angeles"]</example>
        public List<string> Cities { get; set; }

        /// <summary>
        /// A lista de países favoritos do usuário.
        /// </summary>
        /// <example>["USA", "Canada"]</example>
        public List<string> Countries { get; set; }
    }
}
