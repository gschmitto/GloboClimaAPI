using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;

namespace GloboClimaAPI.Models
{
    /// <summary>
    /// Representa a resposta do clima para uma cidade.
    /// </summary>
    public class WeatherResponse
    {
        /// <summary>
        /// Coordenadas geográficas (longitude e latitude).
        /// </summary>
        [JsonProperty("coord")]
        [SwaggerSchema("Coordenadas geográficas (longitude e latitude).")]
        public required Coord Coord { get; set; }

        /// <summary>
        /// Lista de condições climáticas.
        /// </summary>
        [JsonProperty("weather")]
        [SwaggerSchema("Lista de condições climáticas.")]
        public List<Weather>? Weather { get; set; }

        /// <summary>
        /// A origem ou fonte dos dados.
        /// </summary>
        [JsonProperty("base")]
        [SwaggerSchema("A origem ou fonte dos dados.")]
        public string? Base { get; set; }

        /// <summary>
        /// Dados principais
        /// </summary>
        [JsonProperty("main")]
        [SwaggerSchema("Dados principais")]
        public required Main Main { get; set; }

        /// <summary>
        /// Visibilidade em metros.
        /// </summary>
        [JsonProperty("visibility")]
        [SwaggerSchema("Visibilidade em metros.")]
        public int Visibility { get; set; }

        /// <summary>
        /// Dados do vento.
        /// </summary>
        [JsonProperty("wind")]
        [SwaggerSchema("Dados do vento.")]
        public Wind? Wind { get; set; }

        /// <summary>
        /// Dados das nuvens.
        /// </summary>
        [JsonProperty("clouds")]
        [SwaggerSchema("Dados das nuvens.")]
        public Clouds? Clouds { get; set; }

        /// <summary>
        /// Data e hora da consulta ao clima, no formato Unix timestamp.
        /// </summary>
        [JsonProperty("dt")]
        [SwaggerSchema("Data e hora da consulta ao clima, no formato Unix timestamp.")]
        public long Dt { get; set; }

        /// <summary>
        /// Informações sobre o sistema (ex: país, horários de nascer e pôr do sol).
        /// </summary>
        [JsonProperty("sys")]
        [SwaggerSchema("Informações sobre o sistema.")]
        public Sys? Sys { get; set; }

        /// <summary>
        /// Fuso horário da cidade em relação ao UTC, expresso em segundos.
        /// </summary>
        [JsonProperty("timezone")]
        [SwaggerSchema("Fuso horário da cidade em relação ao UTC, expresso em segundos.")]
        public int Timezone { get; set; }

        /// <summary>
        /// Identificador único.
        /// </summary>
        [JsonProperty("id")]
        [SwaggerSchema("Identificador único.")]
        public int Id { get; set; }

        /// <summary>
        /// Nome da cidade.
        /// </summary>
        [JsonProperty("name")]
        [SwaggerSchema("Nome da cidade.")]
        public string? Name { get; set; }

        /// <summary>
        /// Código de status da resposta.
        /// </summary>
        [JsonProperty("cod")]
        [SwaggerSchema("Código de status da resposta.")]
        public int Cod { get; set; }
    }

    /// <summary>
    /// Coordenadas geográficas (longitude e latitude).
    /// </summary>
    public class Coord
    {
        /// <summary>
        /// Longitude da cidade.
        /// </summary>
        [JsonProperty("lon")]
        public double Lon { get; set; }

        /// <summary>
        /// Latitude da cidade.
        /// </summary>
        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    /// <summary>
    /// Condição climática individual.
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// ID da condição climática.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Categoria principal da condição climática (ex: "Clouds").
        /// </summary>
        [JsonProperty("main")]
        public string? Main { get; set; }

        /// <summary>
        /// Descrição detalhada da condição climática (ex: "overcast clouds").
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Ícone associado à condição climática.
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }

    /// <summary>
    /// Dados principais.
    /// </summary>
    public class Main
    {
        /// <summary>
        /// Temperatura atual em graus Celsius.
        /// </summary>
        [JsonProperty("temp")]
        [SwaggerSchema("Temperatura atual em graus Celsius.")]
        public double Temp { get; set; }

        /// <summary>
        /// Temperatura que o corpo humano sente, levando em consideração fatores como vento e umidade.
        /// </summary>
        [JsonProperty("feels_like")]
        [SwaggerSchema("Temperatura que o corpo humano sente, levando em consideração fatores como vento e umidade.")]
        public double FeelsLike { get; set; }

        /// <summary>
        /// Temperatura mínima em graus Celsius.
        /// </summary>
        [JsonProperty("temp_min")]
        [SwaggerSchema("Temperatura mínima em graus Celsius.")]
        public double TempMin { get; set; }

        /// <summary>
        /// Temperatura máxima em graus Celsius.
        /// </summary>
        [JsonProperty("temp_max")]
        [SwaggerSchema("Temperatura máxima em graus Celsius.")]
        public double TempMax { get; set; }

        /// <summary>
        /// Pressão atmosférica em hPa (hectopascal).
        /// </summary>
        [JsonProperty("pressure")]
        [SwaggerSchema("Pressão atmosférica em hPa (hectopascal).")]
        public double Pressure { get; set; }

        /// <summary>
        /// Umidade relativa do ar em porcentagem.
        /// </summary>
        [JsonProperty("humidity")]
        [SwaggerSchema("Umidade relativa do ar em porcentagem.")]
        public int Humidity { get; set; }

        /// <summary>
        /// Nível de pressão atmosférica medido ao nível do mar.
        /// </summary>
        [JsonProperty("sea_level")]
        [SwaggerSchema("Nível de pressão atmosférica medido ao nível do mar.")]
        public double SeaLevel { get; set; }

        /// <summary>
        /// Nível de pressão atmosférica medido ao nível do solo.
        /// </summary>
        [JsonProperty("grnd_level")]
        [SwaggerSchema("Nível de pressão atmosférica medido ao nível do solo.")]
        public double GrndLevel { get; set; }
    }

    /// <summary>
    /// Dados do vento.
    /// </summary>
    public class Wind
    {
        /// <summary>
        /// Velocidade do vento em metros por segundo.
        /// </summary>
        [JsonProperty("speed")]
        [SwaggerSchema("Velocidade do vento em metros por segundo.")]
        public double Speed { get; set; }

        /// <summary>
        /// Direção do vento em graus.
        /// </summary>
        [JsonProperty("deg")]
        [SwaggerSchema("Direção do vento em graus.")]
        public double Deg { get; set; }

        /// <summary>
        /// Velocidade das rajadas.
        /// </summary>
        [JsonProperty("gust")]
        [SwaggerSchema("Velocidade das rajadas.")]
        public double Gust { get; set; }
    }

    /// <summary>
    /// Dados das nuvens.
    /// </summary>
    public class Clouds
    {
        /// <summary>
        /// Quantidade total.
        /// </summary>
        [JsonProperty("all")]
        [SwaggerSchema("Quantidade total.")]
        public int All { get; set; }
    }

    /// <summary>
    /// Informações sobre o sistema (ex: país, horários de nascer e pôr do sol).
    /// </summary>
    public class Sys
    {
        /// <summary>
        /// Tipo do sistema.
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// ID do sistema.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// País da cidade.
        /// </summary>
        [JsonProperty("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Horário de nascer do sol em timestamp Unix.
        /// </summary>
        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        /// <summary>
        /// Horário de pôr do sol em timestamp Unix.
        /// </summary>
        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }
}
