using LojaVirtual.Business.DTOs;

namespace LojaVirtual.Business.Interfaces;
public interface IComputadorRepository
{
	List<ComputadorDTO> ObterTodos(ComputadorFiltroDTO filtro);
	ComputadorDTO? ObterPorId(int id);
	void Adicionar(ComputadorDTO computador);
	bool Atualizar(ComputadorDTO computador);
	bool Remover(int id);
}
