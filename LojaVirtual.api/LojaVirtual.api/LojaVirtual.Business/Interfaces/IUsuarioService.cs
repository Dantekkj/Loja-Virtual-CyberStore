using LojaVirtual.Business.DTOs;

namespace LojaVirtual.Business.Interfaces;

public interface IUsuarioService
{
	Task<UsuarioDTO?> LoginAsync(LoginDTO loginDTO);
	Task<UsuarioDTO?> CadastrarAsync(CadastroDTO cadastroDTO);
	Task<UsuarioDTO?> ObterUsuarioPorIdAsync(int id);
	Task<bool> VerificarEmailAsync(string token);
	Task EnviarEmailBoasVindasAsync(string email, string nome);
	Task EnviarEmailLoginAsync(string email, string nome);
}

