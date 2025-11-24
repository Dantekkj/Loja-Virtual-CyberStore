using LojaVirtual.API.Entities;
using LojaVirtual.Business.Interfaces;
using System.Linq;

namespace LojaVirtual.Business.Services;
	public class ProdutoService : IProdutoService
	{
		private readonly IProdutoRepository _produtoRepository;

		public ProdutoService(IProdutoRepository produtoRepository)
		{
			_produtoRepository = produtoRepository;
		}

		public List<Produto> BuscarProdutos(string? nome = null)
		{
			var produtos = _produtoRepository.ObterTodos();

			if (!string.IsNullOrEmpty(nome))
			{
				produtos = produtos
					.Where(p => !string.IsNullOrEmpty(p.Nome) && p.Nome.ToLower().Contains(nome.ToLower()))
					.ToList();
			}

			return produtos;
		}

		public List<Produto> ObterTodos()
		{
			return _produtoRepository.ObterTodos();
		}

		public Produto? ObterPorId(int id)
		{
			return _produtoRepository.ObterPorId(id);
		}

		public void Adicionar(Produto produto)
		{
			_produtoRepository.Adicionar(produto);
		}

		public bool Atualizar(Produto produto)
		{
			return _produtoRepository.Atualizar(produto);
		}

		public bool Remover(int id)
		{
			return _produtoRepository.Remover(id);
		}

		public bool DiminuirEstoque(int id, int quantidade)
		{
			var produto = _produtoRepository.ObterPorId(id);
			if (produto == null) return false;

			if (produto.Estoque < quantidade) return false;

			produto.Estoque -= quantidade;
			return _produtoRepository.Atualizar(produto);
		}
	}
