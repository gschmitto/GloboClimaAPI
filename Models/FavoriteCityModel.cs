using Swashbuckle.AspNetCore.Annotations;

namespace GloboClimaAPI.Models
{
    /// <summary>
    /// Representa uma cidade favorita com informações adicionais.
    /// </summary>
    public class FavoriteCityModel
    {
        /// <summary>
        /// Nome da cidade favorita.
        /// </summary>
        [SwaggerSchema("Nome da cidade favorita.")]
        public required string CityName { get; set; }
    }
}
