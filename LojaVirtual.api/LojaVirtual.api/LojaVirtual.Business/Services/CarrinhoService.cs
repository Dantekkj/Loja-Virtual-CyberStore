using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;
using System.Linq;

namespace LojaVirtual.Business.Services;

public class CarrinhoService : ICarrinhoService
{

	private static readonly List<ProdutoDTO> _carrinho = new List<ProdutoDTO>();

	public CarrinhoService()
	{
	}

	public List<ProdutoDTO> ObterItensCarrinho()
	{
		return _carrinho;
	}

	public void AdicionarItem(ProdutoDTO produto)
	{
		var itemExistente = _carrinho.FirstOrDefault(p => p.Id == produto.Id);
		if (itemExistente != null)
		{
			itemExistente.Quantidade += produto.Quantidade;
		}
		else
		{
			if (produto.Quantidade <= 0)
				produto.Quantidade = 1;
			_carrinho.Add(produto);
		}
	}

	public bool RemoverItem(int id)
	{
		var item = _carrinho.FirstOrDefault(p => p.Id == id);
		if (item == null) return false;

		_carrinho.Remove(item);
		return true;
	}

	public decimal CalcularTotal()
	{
		return _carrinho.Sum(p => p.Preco * p.Quantidade);
	}

	public bool AtualizarQuantidade(int id, int quantidade)
	{
		var item = _carrinho.FirstOrDefault(p => p.Id == id);
		if (item == null) return false;

		if (quantidade <= 0)
		{
			return RemoverItem(id);
		}

		item.Quantidade = quantidade;
		return true;
	}
}