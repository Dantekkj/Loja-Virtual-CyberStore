using System.Collections.Generic;
using LojaVirtual.API.Entities;

namespace LojaVirtual.Business.Interfaces
{
	public interface IProdutoRepository
	{
		List<Produto> BuscarProdutos(string? nome = null);
		List<Produto> ObterTodos();
		Produto? ObterPorId(int id);
		void Adicionar(Produto produto);
		bool Atualizar(Produto produto);
		bool Remover(int id);
	}
}
