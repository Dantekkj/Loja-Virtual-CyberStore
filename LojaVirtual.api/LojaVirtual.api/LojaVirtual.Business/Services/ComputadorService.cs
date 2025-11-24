using LojaVirtual.API.Repository;
using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;

namespace LojaVirtual.Business.Services;

public class ComputadorService : IComputadorService
{
	private readonly ComputadorRepository _computadorRepository;

	public ComputadorService()
	{
		_computadorRepository = new ComputadorRepository();
	}

	public List<ComputadorDTO> ObterTodos()
	{
		var filtro = new ComputadorFiltroDTO();
		return _computadorRepository.ObterTodos(filtro);
	}

	public ComputadorDTO? ObterPorId(int id)
	{
		return _computadorRepository.ObterPorId(id);
	}

	public void Adicionar(ComputadorDTO computador)
	{
		_computadorRepository.Adicionar(computador);
	}

	public bool Atualizar(ComputadorDTO computador)
	{
		return _computadorRepository.Atualizar(computador);
	}

	public bool Remover(int id)
	{
		return _computadorRepository.Remover(id);
	}
}