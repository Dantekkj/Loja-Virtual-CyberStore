namespace LojaVirtual.API.Entities;

public class Pedido
{
	public int Id { get; set; }
	public int UsuarioId { get; set; }
	public Usuario Usuario { get; set; } = null!;
	public DateTime DataPedido { get; set; } = DateTime.Now;
	public decimal ValorTotal { get; set; }
	public string Status { get; set; } = "Pendente";
	public string MetodoPagamento { get; set; } = string.Empty;
	public string Banco { get; set; } = string.Empty;
	public string? NumeroCartao { get; set; }
	public virtual List<PedidoItem> Itens { get; set; } = new();
}

