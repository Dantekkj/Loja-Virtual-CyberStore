namespace LojaVirtual.Business.DTOs;
public class ProdutoDTO
{
	public int Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public decimal Preco { get; set; }
	public string Descricao { get; set; } = string.Empty;
	public int Estoque { get; set; }
	public string ImagemUrl { get; set; } = string.Empty;
	public int Quantidade { get; set; } = 1;
}
