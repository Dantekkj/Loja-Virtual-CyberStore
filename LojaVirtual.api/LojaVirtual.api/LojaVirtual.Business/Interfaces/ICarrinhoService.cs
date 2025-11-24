using LojaVirtual.Business.DTOs;

namespace LojaVirtual.Business.Interfaces;

public interface ICarrinhoService
{
	List<ProdutoDTO> ObterItensCarrinho();
	void AdicionarItem(ProdutoDTO produto);
	bool RemoverItem(int id);
	bool AtualizarQuantidade(int id, int quantidade);
	decimal CalcularTotal();
}
