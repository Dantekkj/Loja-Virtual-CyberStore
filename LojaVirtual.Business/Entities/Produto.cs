namespace LojaVirtual.API.Entities;
public class Produto
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public decimal Preco { get; set; }
	public string Descricao { get; set; }
	public int Estoque { get; set; }
	public string ImagemUrl { get; set; } = string.Empty;

	public virtual List<ProdutoItem> Itens { get; set; }
}
