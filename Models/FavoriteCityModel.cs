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
        public string CityName { get; set; }

        /// <summary>
        /// País onde a cidade favorita está localizada.
        /// </summary>
        [SwaggerSchema("País onde a cidade favorita está localizada.")]
        public string Country { get; set; }

        /// <summary>
        /// Código da cidade (geralmente um identificador único).
        /// </summary>
        [SwaggerSchema("Código da cidade (geralmente um identificador único).")]
        public string CityCode { get; set; }

        /// <summary>
        /// Latitude geográfica da cidade.
        /// </summary>
        [SwaggerSchema("Latitude geográfica da cidade.")]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude geográfica da cidade.
        /// </summary>
        [SwaggerSchema("Longitude geográfica da cidade.")]
        public double Longitude { get; set; }

        /// <summary>
        /// A população estimada da cidade.
        /// </summary>
        [SwaggerSchema("A população estimada da cidade.")]
        public long Population { get; set; }

        /// <summary>
        /// O clima predominante na cidade (ex: "tropical", "temperado").
        /// </summary>
        [SwaggerSchema("O clima predominante na cidade (ex: 'tropical', 'temperado').")]
        public string Climate { get; set; }

        /// <summary>
        /// A data de inclusão da cidade como favorita.
        /// </summary>
        [SwaggerSchema("A data de inclusão da cidade como favorita.")]
        public string AddedDate { get; set; }

        /// <summary>
        /// Indica se a cidade está marcada como favorita (true ou false).
        /// </summary>
        [SwaggerSchema("Indica se a cidade está marcada como favorita (true ou false).")]
        public bool IsFavorite { get; set; }
    }
}
