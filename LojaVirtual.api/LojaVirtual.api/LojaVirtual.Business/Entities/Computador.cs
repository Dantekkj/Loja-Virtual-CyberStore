namespace LojaVirtual.API.Entities;

public class Computador
{
	public int Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Marca { get; set; } = string.Empty;
	public string Processador { get; set; } = string.Empty;
	public decimal Preco { get; set; }
	public string Armazenamento { get; set; } = string.Empty;
	public string RAM { get; set; } = string.Empty;
	public string PlacaVideo { get; set; } = string.Empty;
	public string Perifericos { get; set; } = string.Empty;
	public string Descricao { get; set; } = string.Empty;
	public int Estoque { get; set; }
}

