namespace GloboClimaAPI.DTOs
{
    /// <summary>
    /// DTO usado para registrar novos usuários.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// O email do usuário.
        /// </summary>
        /// <example>usuario@email.com</example>
        public required string Email { get; set; }

        /// <summary>
        /// A senha do usuário.
        /// </summary>
        /// <example>SenhaSegura123!</example>
        public required string Password { get; set; }
    }
}
