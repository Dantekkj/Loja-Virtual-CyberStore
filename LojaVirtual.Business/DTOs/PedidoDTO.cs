namespace LojaVirtual.Business.DTOs;

public class PedidoDTO
{
	public int Id { get; set; }
	public DateTime DataPedido { get; set; }
	public decimal ValorTotal { get; set; }
	public string Status { get; set; } = string.Empty;
	public string MetodoPagamento { get; set; } = string.Empty;
	public string Banco { get; set; } = string.Empty;
	public List<PedidoItemDTO> Itens { get; set; } = new();
}

