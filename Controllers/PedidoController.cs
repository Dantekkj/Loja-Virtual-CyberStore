using Microsoft.AspNetCore.Mvc;
using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;

namespace LojaVirtual.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
	private readonly IPedidoService _pedidoService;

	public PedidoController(IPedidoService pedidoService)
	{
		_pedidoService = pedidoService;
	}

	[HttpPost("finalizar")]
	public async Task<IActionResult> FinalizarCompra([FromBody] PagamentoDTO pagamentoDTO)
	{
		var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		if (string.IsNullOrEmpty(token))
			return Unauthorized(new { mensagem = "Usuário não autenticado." });

		var parts = token.Split(':');
		if (parts.Length < 1 || !int.TryParse(parts[0], out var userId))
			return Unauthorized(new { mensagem = "Token inválido." });

		try
		{
			var pedido = await _pedidoService.CriarPedidoAsync(userId, pagamentoDTO);
			return Ok(new
			{
				mensagem = "Compra realizada com sucesso!",
				pedido
			});
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { mensagem = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(new { mensagem = ex.Message });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { mensagem = $"Erro ao finalizar compra: {ex.Message}" });
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> ObterPedido(int id)
	{
		var pedido = await _pedidoService.ObterPedidoPorIdAsync(id);
		if (pedido == null)
			return NotFound();

		return Ok(pedido);
	}

	[HttpGet("usuario/meus-pedidos")]
	public async Task<IActionResult> ObterMeusPedidos()
	{
		var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		if (string.IsNullOrEmpty(token))
			return Unauthorized(new { mensagem = "Usuário não autenticado." });

		var parts = token.Split(':');
		if (parts.Length < 1 || !int.TryParse(parts[0], out var userId))
			return Unauthorized(new { mensagem = "Token inválido." });

		var pedidos = await _pedidoService.ObterPedidosPorUsuarioAsync(userId);
		return Ok(pedidos);
	}
}

