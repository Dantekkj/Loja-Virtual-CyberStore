using Microsoft.AspNetCore.Mvc;
using LojaVirtual.API.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Services;
using LojaVirtual.Business.DTOs;
using System.Linq;

namespace LojaVirtual.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrinhoController : ControllerBase
{
	private readonly ICarrinhoService _carrinhoService;
	private readonly IProdutoService _produtoService;

	public CarrinhoController(ICarrinhoService carrinhoService, IProdutoService produtoService)
	{
		_carrinhoService = carrinhoService;
		_produtoService = produtoService;
	}

	[HttpGet]
	public IActionResult GetCarrinho()
	{
		var itens = _carrinhoService.ObterItensCarrinho();
		return Ok(itens);
	}

	[HttpPost]
	public IActionResult AdicionarItem([FromBody] ProdutoDTO produtoDTO)
	{
		if (produtoDTO == null)
			return BadRequest(new { mensagem = "Produto inválido." });

		var produto = _produtoService.ObterPorId(produtoDTO.Id);
		if (produto == null)
			return NotFound(new { mensagem = "Produto não encontrado." });

		var quantidadeSolicitada = produtoDTO.Quantidade > 0 ? produtoDTO.Quantidade : 1;
		if (produto.Estoque < quantidadeSolicitada)
			return BadRequest(new { mensagem = $"Estoque insuficiente. Disponível: {produto.Estoque} unidades." });

		_produtoService.DiminuirEstoque(produtoDTO.Id, quantidadeSolicitada);

		_carrinhoService.AdicionarItem(produtoDTO);
		return Ok(new { mensagem = "Produto adicionado ao carrinho." });
	}

	[HttpDelete("{id}")]
	public IActionResult RemoverItem(int id)
	{
		var itens = _carrinhoService.ObterItensCarrinho();
		var item = itens.FirstOrDefault(i => i.Id == id);
		
		var sucesso = _carrinhoService.RemoverItem(id);

		if (!sucesso)
			return NotFound(new { mensagem = "Item não encontrado no carrinho." });

		if (item != null)
		{
			var produto = _produtoService.ObterPorId(id);
			if (produto != null)
			{
				produto.Estoque += item.Quantidade;
				_produtoService.Atualizar(produto);
			}
		}

		return NoContent();
	}

	[HttpGet("total")]
	public IActionResult ValorTotal()
	{
		var total = _carrinhoService.CalcularTotal();
		return Ok(new { valorTotal = total });
	}

	[HttpPut("{id}/quantidade")]
	public IActionResult AtualizarQuantidade(int id, [FromBody] int quantidade)
	{
		if (quantidade <= 0)
			return BadRequest(new { mensagem = "Quantidade deve ser maior que zero." });

		var itens = _carrinhoService.ObterItensCarrinho();
		var itemAtual = itens.FirstOrDefault(i => i.Id == id);
		if (itemAtual == null)
			return NotFound(new { mensagem = "Item não encontrado no carrinho." });

		var quantidadeAtual = itemAtual.Quantidade;
		var diferenca = quantidade - quantidadeAtual;

		if (diferenca > 0)
		{
			var produto = _produtoService.ObterPorId(id);
			if (produto == null)
				return NotFound(new { mensagem = "Produto não encontrado." });

			if (produto.Estoque < diferenca)
				return BadRequest(new { mensagem = $"Estoque insuficiente. Disponível: {produto.Estoque} unidades." });

			_produtoService.DiminuirEstoque(id, diferenca);
		}
		else if (diferenca < 0)
		{
			var produto = _produtoService.ObterPorId(id);
			if (produto != null)
			{
				produto.Estoque += Math.Abs(diferenca);
				_produtoService.Atualizar(produto);
			}
		}

		var sucesso = _carrinhoService.AtualizarQuantidade(id, quantidade);

		if (!sucesso)
			return NotFound(new { mensagem = "Item não encontrado no carrinho." });

		return Ok(new { mensagem = "Quantidade atualizada com sucesso." });
	}
}
