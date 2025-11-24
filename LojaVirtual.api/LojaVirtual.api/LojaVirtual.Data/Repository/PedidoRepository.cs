using LojaVirtual.API.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.API.Repository;

public class PedidoRepository : IPedidoRepository
{
	private readonly Context _context;

	public PedidoRepository(Context context)
	{
		_context = context;
	}

	public async Task<Pedido> CriarAsync(Pedido pedido)
	{
		try
		{
			_context.Pedidos.Add(pedido);
			await _context.SaveChangesAsync();
			return pedido;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Erro ao criar pedido: {ex.Message}", ex);
		}
	}

	public async Task<Pedido?> ObterPorIdAsync(int id)
	{
		try
		{
			return await _context.Pedidos
				.Include(p => p.Itens)
					.ThenInclude(i => i.Produto)
				.Include(p => p.Usuario)
				.FirstOrDefaultAsync(p => p.Id == id);
		}
		catch
		{
			return null;
		}
	}

	public async Task<List<Pedido>> ObterPorUsuarioIdAsync(int usuarioId)
	{
		try
		{
			return await _context.Pedidos
				.Include(p => p.Itens)
					.ThenInclude(i => i.Produto)
				.Where(p => p.UsuarioId == usuarioId)
				.OrderByDescending(p => p.DataPedido)
				.ToListAsync();
		}
		catch
		{
			return new List<Pedido>();
		}
	}
}

