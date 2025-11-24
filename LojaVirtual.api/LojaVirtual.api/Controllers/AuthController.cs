using Microsoft.AspNetCore.Mvc;
using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;
using MySqlConnector;
using System.Security.Cryptography;
using System.Text;

namespace LojaVirtual.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IUsuarioService _usuarioService;
	private readonly IConfiguration _configuration;

	public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
	{
		_usuarioService = usuarioService;
		_configuration = configuration;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
	{
		if (string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Senha))
			return BadRequest(new { mensagem = "E-mail e senha são obrigatórios." });

		try
		{
			var usuario = await _usuarioService.LoginAsync(loginDTO);
			if (usuario == null)
				return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });

			var token = GerarToken(usuario.Id, usuario.Email);
			return Ok(new
			{
				token,
				usuario
			});
		}
		catch (Exception)
		{
			return StatusCode(500, new { mensagem = "Erro ao fazer login. Tente novamente em instantes." });
		}
	}

	[HttpPost("cadastro")]
	public async Task<IActionResult> Cadastro([FromBody] CadastroDTO cadastroDTO)
	{
		if (string.IsNullOrWhiteSpace(cadastroDTO.Email) || string.IsNullOrWhiteSpace(cadastroDTO.Senha))
			return BadRequest(new { mensagem = "E-mail e senha são obrigatórios." });

		if (string.IsNullOrWhiteSpace(cadastroDTO.Nome))
			return BadRequest(new { mensagem = "Nome é obrigatório." });

		try
		{
			var usuario = await _usuarioService.CadastrarAsync(cadastroDTO);
			var token = GerarToken(usuario!.Id, usuario.Email);
			return Ok(new
			{
				token,
				usuario
			});
		}
		catch (InvalidOperationException ex)
		{
			if (ex.Message.Contains("já cadastrado", StringComparison.OrdinalIgnoreCase))
				return Conflict(new { mensagem = ex.Message });

			return StatusCode(503, new { mensagem = "Não conseguimos salvar seu cadastro agora. Verifique o banco de dados e tente novamente." });
		}
		catch (MySqlException)
		{
			return StatusCode(503, new { mensagem = "Banco de dados indisponível no momento. Tente novamente em alguns segundos." });
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { mensagem = ex.Message });
		}
		catch (Exception)
		{
			return StatusCode(500, new { mensagem = "Erro inesperado ao cadastrar. Tente novamente." });
		}
	}

	[HttpGet("usuario")]
	public async Task<IActionResult> ObterUsuario()
	{
		var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		if (string.IsNullOrEmpty(token))
			return Unauthorized();

		var parts = token.Split(':');
		if (parts.Length < 1 || !int.TryParse(parts[0], out var userId))
			return Unauthorized();

		var usuario = await _usuarioService.ObterUsuarioPorIdAsync(userId);
		if (usuario == null)
			return NotFound();

		return Ok(usuario);
	}

	private string GerarToken(int userId, string email)
	{
		var secret = _configuration["Jwt:Secret"] ?? "CyberStoreSecretKey2025!@#$%";
		var data = $"{userId}:{email}:{DateTime.UtcNow:yyyyMMdd}";
		using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
		{
			var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
			var token = Convert.ToBase64String(hash);
			return $"{userId}:{token}";
		}
	}
}

