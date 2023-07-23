using ControleVendas.Server.Repositories;
using ControleVendas.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendas.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private ClienteRepository _clienteRepository;

        public ClienteController(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteRepository.GetAll();

            if (clientes.Any())
                return Ok(clientes);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            await _clienteRepository.Save(cliente);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Cliente cliente)
        {
            await _clienteRepository.Update(cliente);
            return Ok();
        }

        [HttpPost("inativar")]
        public async Task<IActionResult> Inativar([FromBody] Cliente cliente)
        {
            cliente.Ativo = false;
            await _clienteRepository.Update(cliente);
            return Ok();
        }

        [HttpPost("ativar")]
        public async Task<IActionResult> Ativar([FromBody] Cliente cliente)
        {
            cliente.Ativo = true;
            await _clienteRepository.Update(cliente);
            return Ok();
        }
    }
}
