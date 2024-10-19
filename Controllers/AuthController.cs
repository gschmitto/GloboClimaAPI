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

        /// <summary>
        /// Construtor do AuthController que injeta dependências necessárias.
        /// </summary>
        /// <param name="userService">Serviço de usuário para autenticação e registro.</param>
        /// <param name="configuration">Configuração do aplicativo para acessar JWT e outros valores de configuração.</param>
        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="userRegisterDto">DTO com informações para registro de usuário.</param>
        /// <returns>Retorna uma mensagem de sucesso ou erro.</returns>
        /// <response code="200">Sucesso: Usuário registrado com sucesso.</response>
        /// <response code="400">Erro: Dados inválidos ou usuário já existe.</response>
        /// <response code="500">Erro: Falha ao registrar o usuário.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos.");
            }

            var userExists = await _userService.UserExistsAsync(userRegisterDto.Email);
            if (userExists)
            {
                return BadRequest("Usuário já existe.");
            }

            var user = await _userService.RegisterUserAsync(userRegisterDto);

            if (user == null)
            {
                return StatusCode(500, "Erro ao registrar o usuário. Tente novamente.");
            }

            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="userLoginDto">DTO com email e senha do usuário.</param>
        /// <returns>Retorna o token JWT se o login for bem-sucedido.</returns>
        /// <response code="200">Sucesso: Token JWT retornado.</response>
        /// <response code="400">Erro: Dados inválidos.</response>
        /// <response code="401">Erro: Credenciais inválidas.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos.");
            }

            var user = await _userService.AuthenticateUserAsync(userLoginDto.Email, userLoginDto.Password);
            if (user == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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
    }
}
