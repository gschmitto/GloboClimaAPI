namespace GloboClimaAPI.DTOs
{
    /// <summary>
    /// DTO usado para autenticação de usuários.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// O email do usuário para login.
        /// </summary>
        /// <example>usuario@email.com</example>
        public required string Email { get; set; }

        /// <summary>
        /// A senha do usuário para login.
        /// </summary>
        /// <example>SenhaSegura123!</example>
        public required string Password { get; set; }
    }
}
