namespace LojaVirtual.Business.DTOs;
public class ComputadorDTO
{
	public int Id { get; set; }
	public string Marca { get; set; } = string.Empty;
	public string Modelo { get; set; } = string.Empty;
	public string Processador { get; set; } = string.Empty;
	public int MemoriaGB { get; set; }
	public int ArmazenamentoGB { get; set; }
	public decimal Preco { get; set; }
}
