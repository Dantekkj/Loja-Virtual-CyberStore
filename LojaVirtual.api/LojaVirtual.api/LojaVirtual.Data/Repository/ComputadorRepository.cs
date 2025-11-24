using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;

namespace LojaVirtual.API.Repository;

public class ComputadorRepository : IComputadorRepository
{
	private static readonly List<ComputadorDTO> _computadores = new();

	public List<ComputadorDTO> ObterTodos(ComputadorFiltroDTO filtro)
	{
		IQueryable<ComputadorDTO> query = _computadores.AsQueryable();

		if (filtro.Processador != null)
		{
			query = query.Where(comp => comp.Processador.Contains(filtro.Processador, StringComparison.OrdinalIgnoreCase));
		}

		if (filtro.Marca != null)
		{
			query = query.Where(comp => comp.Marca.Contains(filtro.Marca, StringComparison.OrdinalIgnoreCase));
		}

		return query.ToList();
	}

	public ComputadorDTO? ObterPorId(int id) => _computadores.FirstOrDefault(c => c.Id == id);

	public void Adicionar(ComputadorDTO computador)
	{
		computador.Id = _computadores.Any() ? _computadores.Max(c => c.Id) + 1 : 1;
		_computadores.Add(computador);
	}

	public bool Atualizar(ComputadorDTO computador)
	{
		var existente = _computadores.FirstOrDefault(c => c.Id == computador.Id);
		if (existente == null) return false;

		existente.Marca = computador.Marca;
		existente.Modelo = computador.Modelo;
		existente.Processador = computador.Processador;
		existente.MemoriaGB = computador.MemoriaGB;
		existente.ArmazenamentoGB = computador.ArmazenamentoGB;
		existente.Preco = computador.Preco;

		return true;
	}

	public bool Remover(int id)
	{
		var comp = _computadores.FirstOrDefault(c => c.Id == id);
		if (comp == null) return false;

		_computadores.Remove(comp);
		return true;
	}
}