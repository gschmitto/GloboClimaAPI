namespace GloboClimaAPI.Models
{
    /// <summary>
    /// Representa os favoritos de um usuário, incluindo cidades e países.
    /// </summary>
    public class UserFavorites
    {
        /// <summary>
        /// O email do usuário.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// A lista de cidades favoritas do usuário.
        /// </summary>
        public required List<FavoriteCityModel> FavoriteCities { get; set; }
    }
}
