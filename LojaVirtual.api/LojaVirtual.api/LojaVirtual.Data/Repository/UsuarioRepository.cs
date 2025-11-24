using LojaVirtual.API.Entities;
using LojaVirtual.Business.Interfaces;
using LojaVirtual.Data;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.API.Repository;

public class UsuarioRepository : IUsuarioRepository
{
	private readonly Context _context;

	public UsuarioRepository(Context context)
	{
		_context = context;
	}

	public async Task<Usuario?> ObterPorEmailAsync(string email)
	{
		try
		{
			return await _context.Usuarios
				.FirstOrDefaultAsync(u => u.Email == email);
		}
		catch
		{
			return null;
		}
	}

	public async Task<Usuario?> ObterPorIdAsync(int id)
	{
		try
		{
			return await _context.Usuarios
				.FirstOrDefaultAsync(u => u.Id == id);
		}
		catch
		{
			return null;
		}
	}

	public async Task<Usuario> CriarAsync(Usuario usuario)
	{
		try
		{
			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();
			return usuario;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Erro ao criar usuário: {ex.Message}", ex);
		}
	}

	public async Task<bool> EmailExisteAsync(string email)
	{
		try
		{
			return await _context.Usuarios
				.AnyAsync(u => u.Email == email);
		}
		catch
		{
			return false;
		}
	}

	public async Task AtualizarAsync(Usuario usuario)
	{
		try
		{
			_context.Usuarios.Update(usuario);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Erro ao atualizar usuário: {ex.Message}", ex);
		}
	}
}

