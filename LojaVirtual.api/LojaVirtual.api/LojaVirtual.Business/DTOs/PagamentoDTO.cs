namespace LojaVirtual.Business.DTOs;

public class PagamentoDTO
{
	public string MetodoPagamento { get; set; } = string.Empty;
	public string TipoCartao { get; set; } = string.Empty;
	public string Banco { get; set; } = string.Empty;
	public string NumeroCartao { get; set; } = string.Empty;
	public string NomeTitular { get; set; } = string.Empty;
	public string Validade { get; set; } = string.Empty;
	public string CVV { get; set; } = string.Empty;
}

