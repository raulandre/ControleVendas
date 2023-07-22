using Azure.Core;
using ControleVendas.Server.Repositories;
using ControleVendas.Server.Utils;
using ControleVendas.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendas.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private UsuarioRepository _usuarioRepository;

        public AuthController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var usuario = await _usuarioRepository.GetByNome(loginModel.Nome);

            if (usuario is null)
                return NotFound(
                    new
                    {
                        Success = false,
                        Message = "Usuário não encontrado!"
                    });

            if (PasswordUtils.VerifyPasswordHash(usuario, loginModel.Senha, usuario.Hash, usuario.Salt))
            {
                return Ok(
                    new
                    {
                        Success = true,
                        Message = "Login efetuado com sucesso",
                        Token = PasswordUtils.CreateToken(usuario)
                    });
            }

            return Unauthorized(
                new
                {
                    Success = false,
                    Message = "Erro ao realizar login, verifique seu nome de usuário e senha!"
                });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel loginModel)
        {
            if (await _usuarioRepository.GetByNome(loginModel.Nome) is not null)
                return BadRequest(
                    new
                    {
                        Success = false,
                        Message = "Nome de usuário em uso!"
                    });

            PasswordUtils.CreatePasswordHash(loginModel.Senha, out var hash, out var salt);
            var usuario = new Usuario(loginModel.Nome, hash, salt);

            await _usuarioRepository.Save(usuario);
            return Ok(
                new
                {
                    Success = true,
                    Message = "Usuário cadastrado com sucesso!"
                });
        }
    }
}