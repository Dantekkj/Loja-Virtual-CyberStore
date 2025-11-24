using LojaVirtual.API.Entities;

namespace LojaVirtual.Business.Interfaces;

public interface IUsuarioRepository
{
	Task<Usuario?> ObterPorEmailAsync(string email);
	Task<Usuario?> ObterPorIdAsync(int id);
	Task<Usuario> CriarAsync(Usuario usuario);
	Task<bool> EmailExisteAsync(string email);
	Task AtualizarAsync(Usuario usuario);
}

