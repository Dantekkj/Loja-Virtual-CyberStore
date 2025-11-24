namespace LojaVirtual.API.Entities;
public class ProdutoItem
{
	public int Id { get; set; }
	public int ProdutoId { get; set; }
	public Produto Produto { get; set; }
    public int CarrinhoId { get; set; }
    public Carrinho Carrinho { get; set; }
    public int Quantitdade { get; set; }
}
