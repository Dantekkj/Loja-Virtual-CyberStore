using LojaVirtual.API.Entities;
using System.Collections.Generic;

namespace LojaVirtual.Business.Interfaces
{
	public interface IProdutoService
	{
		List<Produto> BuscarProdutos(string? nome = null);
		List<Produto> ObterTodos();
		Produto? ObterPorId(int id);
		void Adicionar(Produto produto);
		bool Atualizar(Produto produto);
		bool Remover(int id);
		bool DiminuirEstoque(int id, int quantidade);
	}
}
