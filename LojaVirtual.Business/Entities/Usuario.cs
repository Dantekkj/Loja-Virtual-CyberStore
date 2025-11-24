namespace LojaVirtual.API.Entities;

public class Usuario
{
	public int Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string SenhaHash { get; set; } = string.Empty;
	public string? Telefone { get; set; }
	public string Nome { get; set; } = string.Empty;
	public DateTime DataCadastro { get; set; } = DateTime.Now;
	public bool EmailVerificado { get; set; } = false;
	public string? TokenVerificacao { get; set; }
	public DateTime? TokenVerificacaoExpiracao { get; set; }
}

