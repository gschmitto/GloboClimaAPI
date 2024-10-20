using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using GloboClimaAPI.DTOs;

namespace GloboClimaAPI.Controllers
{
    /// <summary>
    /// Controlador responsável pela autenticação e registro de usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Construtor do AuthController que injeta dependências necessárias.
        /// </summary>
        /// <param name="userService">Serviço de usuário para autenticação e registro.</param>
        /// <param name="configuration">Configuração do aplicativo para acessar JWT e outros valores de configuração.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public AuthController(IUserService userService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="userRegisterDto">DTO com informações para registro de usuário.</param>
        /// <returns>Retorna uma mensagem de sucesso ou erro.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _userService.UserExistsAsync(userRegisterDto.Email))
            {
                return BadRequest("Usuário já existe.");
            }

            var user = await _userService.RegisterUserAsync(userRegisterDto);
            if (user == null)
            {
                _logger.LogError("Erro ao registrar o usuário: {Email}", userRegisterDto.Email);
                return StatusCode(500, "Erro ao registrar o usuário. Tente novamente.");
            }

            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="userLoginDto">DTO com email e senha do usuário.</param>
        /// <returns>Retorna o token JWT se o login for bem-sucedido.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.AuthenticateUserAsync(userLoginDto.Email, userLoginDto.Password);
            if (result.Data == null)
            {
                _logger.LogWarning("Tentativa de login falhou para o usuário: {Email}", userLoginDto.Email);
                return Unauthorized("Credenciais inválidas.");
            }

            var token = GenerateJwtToken((User)result.Data);
            var response = new LoginResponseDto { Token = token };
            return Ok(response);
        }

        /// <summary>
        /// Função privada para gerar o token JWT.
        /// </summary>
        /// <param name="user">Usuário autenticado para geração de token.</param>
        /// <returns>Retorna o token JWT gerado.</returns>
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var jwtKey = GetJwtKey();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Obtém a chave JWT a partir da configuração.
        /// </summary>
        /// <returns>A chave JWT.</returns>
        private string GetJwtKey()
        {
            return _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "Chave de segurança JWT não configurada.");
        }
    }
}
