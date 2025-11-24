using LojaVirtual.Business.DTOs;

namespace LojaVirtual.Business.Interfaces;

public interface IComputadorService
{
	List<ComputadorDTO> ObterTodos();
	ComputadorDTO? ObterPorId(int id);
	void Adicionar(ComputadorDTO computador);
	bool Atualizar(ComputadorDTO computador);
	bool Remover(int id);
}
