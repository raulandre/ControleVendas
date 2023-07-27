using ControleVendas.Server.Data;
using ControleVendas.Server.Repositories;
using ControleVendas.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendas.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VendaController : ControllerBase
    {
        private VendaRepository _vendaRepository;
        private ProdutoRepository _produtoRepository;
        private IUnitOfWork _unitOfWork;

        public VendaController(VendaRepository vendaRepository, ProdutoRepository produtoRepository, IUnitOfWork unitOfWork)
        {
            _vendaRepository = vendaRepository;
            _produtoRepository = produtoRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int vendedorId, 
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim
        )
        {
            var vendas = await _vendaRepository.GetAll(vendedorId, dataInicio, dataFim);

            if (vendas.Any())
                return Ok(vendas);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Venda venda)
        {
            var produtosSemEstoque = await _produtoRepository.VerificaEstoque(venda.Itens);

            if (produtosSemEstoque.Any())
            {
                return BadRequest(produtosSemEstoque.Select(p => p.Descricao));
            }

            var vendaId = await _vendaRepository.Save(venda);
            await _produtoRepository.UpdateEstoque(vendaId);
            return Ok();
        }
    }
}
