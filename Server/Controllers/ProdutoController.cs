using ControleVendas.Server.Repositories;
using ControleVendas.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendas.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private ProdutoRepository _produtoRepository;

        public ProdutoController(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _produtoRepository.GetAll();

            if (produtos.Any())
                return Ok(produtos);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            await _produtoRepository.Save(produto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Produto produto)
        {
            await _produtoRepository.Update(produto);
            return Ok();
        }

        [HttpPost("inativar")]
        public async Task<IActionResult> Inativar([FromBody] Produto produto)
        {
            produto.Ativo = false;
            await _produtoRepository.Update(produto);
            return Ok();
        }

        [HttpPost("ativar")]
        public async Task<IActionResult> Ativar([FromBody] Produto produto)
        {
            produto.Ativo = true;
            await _produtoRepository.Update(produto);
            return Ok();
        }
    }
}
