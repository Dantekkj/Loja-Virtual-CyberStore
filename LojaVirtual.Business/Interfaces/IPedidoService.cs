using LojaVirtual.Business.DTOs;

namespace LojaVirtual.Business.Interfaces;

public interface IPedidoService
{
	Task<PedidoDTO> CriarPedidoAsync(int usuarioId, PagamentoDTO pagamentoDTO);
	Task<PedidoDTO?> ObterPedidoPorIdAsync(int pedidoId);
	Task<List<PedidoDTO>> ObterPedidosPorUsuarioAsync(int usuarioId);
}

