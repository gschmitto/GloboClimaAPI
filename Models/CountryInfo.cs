using Swashbuckle.AspNetCore.Annotations;

namespace GloboClimaAPI.Models
{
    /// <summary>
    /// Representa informações gerais sobre um país.
    /// </summary>
    public class CountryInfo
    {
        /// <summary>
        /// Nome completo do país.
        /// </summary>
        [SwaggerSchema("Nome completo do país.")]
        public string Name { get; set; }

        /// <summary>
        /// Código de 2 letras (ISO 3166-1 alpha-2) do país.
        /// </summary>
        [SwaggerSchema("Código de 2 letras (ISO 3166-1 alpha-2) do país.")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Capital do país.
        /// </summary>
        [SwaggerSchema("Capital do país.")]
        public string Capital { get; set; }

        /// <summary>
        /// Continente onde o país está localizado (ex: 'Europa', 'América').
        /// </summary>
        [SwaggerSchema("Continente onde o país está localizado (ex: 'Europa', 'América').")]
        public string Continent { get; set; }

        /// <summary>
        /// População do país.
        /// </summary>
        [SwaggerSchema("População do país.")]
        public long Population { get; set; }

        /// <summary>
        /// Superfície do país em quilômetros quadrados.
        /// </summary>
        [SwaggerSchema("Superfície do país em quilômetros quadrados.")]
        public double Area { get; set; }

        /// <summary>
        /// Língua oficial do país.
        /// </summary>
        [SwaggerSchema("Língua oficial do país.")]
        public string OfficialLanguage { get; set; }

        /// <summary>
        /// Moeda utilizada no país.
        /// </summary>
        [SwaggerSchema("Moeda utilizada no país.")]
        public string Currency { get; set; }

        /// <summary>
        /// Data de independência do país.
        /// </summary>
        [SwaggerSchema("Data de independência do país.")]
        public string IndependenceDate { get; set; }

        /// <summary>
        /// Código de telefone internacional do país.
        /// </summary>
        [SwaggerSchema("Código de telefone internacional do país.")]
        public string PhoneCode { get; set; }

        /// <summary>
        /// Sigla de fuso horário do país (ex: UTC-03:00).
        /// </summary>
        [SwaggerSchema("Sigla de fuso horário do país (ex: UTC-03:00).")]
        public string TimeZone { get; set; }
    }
}
