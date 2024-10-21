namespace GloboClimaAPI.DTOs
{
    /// <summary>
    /// Representa a resposta após uma tentativa de login bem-sucedida.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT gerado para usuários autenticados.
        /// </summary>
        public required string Token { get; set; }
    }
}
