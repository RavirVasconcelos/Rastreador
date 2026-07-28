using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.DTOs;
using Rastreador.Services;

namespace Rastreador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly RastreadorContext _context;

        public AuthsController(JwtService jwtService, IConfiguration configuration, RastreadorContext context)
        {
            _jwtService = jwtService;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            loginRequest.Email = loginRequest.Email.ToLower().Trim();

            var clienteCadastrado = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == loginRequest.Email);

            if (clienteCadastrado == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Senha, clienteCadastrado.Senha))
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var token = _jwtService.GerarToken(clienteCadastrado);
            return Ok(new { Token = token });
        }
    }
}
