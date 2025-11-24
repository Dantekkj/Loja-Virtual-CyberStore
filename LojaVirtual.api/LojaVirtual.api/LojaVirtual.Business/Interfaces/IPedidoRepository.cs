using LojaVirtual.API.Entities;

namespace LojaVirtual.Business.Interfaces;

public interface IPedidoRepository
{
	Task<Pedido> CriarAsync(Pedido pedido);
	Task<Pedido?> ObterPorIdAsync(int id);
	Task<List<Pedido>> ObterPorUsuarioIdAsync(int usuarioId);
}

