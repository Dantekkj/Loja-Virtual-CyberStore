namespace LojaVirtual.Business.DTOs;

public class UsuarioDTO
{
	public int Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string FotoUrl { get; set; } = string.Empty;
}

