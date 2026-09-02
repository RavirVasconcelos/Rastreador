using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.DTOs;
using Rastreador.Models;
using System.Globalization;

namespace Rastreador.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly RastreadorContext _rastreadorContext;

        public ClientesController(RastreadorContext rastreadorContext)
        {
            _rastreadorContext = rastreadorContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _rastreadorContext.Clientes.ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("GetCliente")]
        public async Task<IActionResult> GetCliente()
        {
            var usuarioLogadoId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value);

            var cliente = await _rastreadorContext.Clientes.FindAsync(usuarioLogadoId);

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCliente([FromBody] CadastrarClienteDTO cadastrarClienteDTO)
        {
            if (!DateTime.TryParseExact(
                cadastrarClienteDTO.DataNascimento,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dataNascimento))
            {
                return BadRequest("Data de nascimento inválida. Utilize o formato dd/MM/yyyy.");
            }

            cadastrarClienteDTO.Senha = BCrypt.Net.BCrypt.HashPassword(cadastrarClienteDTO.Senha);

            var clienteExistente = await _rastreadorContext.Clientes
                .FirstOrDefaultAsync(c => c.Email == cadastrarClienteDTO.Email && c.Senha == cadastrarClienteDTO.Senha && c.Cpf == cadastrarClienteDTO.Cpf);

            if (clienteExistente != null)
            {
                return BadRequest("Cliente já cadastrado.");
            }

            var cliente = new Cliente
            {
                Nome = cadastrarClienteDTO.Nome,
                Email = cadastrarClienteDTO.Email,
                Senha = cadastrarClienteDTO.Senha,
                Cpf = cadastrarClienteDTO.Cpf,
                DataNascimento = dataNascimento,
                DataCadastro = DateTime.UtcNow
            };

            _rastreadorContext.Clientes.Add(cliente);
            await _rastreadorContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, cliente);
        }
    }
}