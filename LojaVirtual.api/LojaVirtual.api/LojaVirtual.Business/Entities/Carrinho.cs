namespace LojaVirtual.API.Entities;

public class Carrinho
{
	public int Id { get; set; }
	public virtual List<ProdutoItem> Itens { get; set; }
    public decimal ValorTotal { get; set; }
}
