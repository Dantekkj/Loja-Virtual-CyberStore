using LojaVirtual.API.Entities;
using LojaVirtual.Business.DTOs;
using LojaVirtual.Business.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LojaVirtual.Business.Services;

public class UsuarioService : IUsuarioService
{
	private readonly IUsuarioRepository _usuarioRepository;
	private readonly IEmailService _emailService;

	public UsuarioService(IUsuarioRepository usuarioRepository, IEmailService emailService)
	{
		_usuarioRepository = usuarioRepository;
		_emailService = emailService;
	}

	public async Task<UsuarioDTO?> LoginAsync(LoginDTO loginDTO)
	{
		var usuario = await _usuarioRepository.ObterPorEmailAsync(loginDTO.Email);
		if (usuario == null)
			return null;

		if (!VerificarSenha(loginDTO.Senha, usuario.SenhaHash))
			return null;

		// Enviar e-mail de login
		try
		{
			await EnviarEmailLoginAsync(usuario.Email, usuario.Nome);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Erro ao enviar e-mail de login: {ex.Message}");
		}

		return new UsuarioDTO
		{
			Id = usuario.Id,
			Email = usuario.Email,
			Telefone = usuario.Telefone,
			Nome = usuario.Nome,
			FotoUrl = GerarGravatarUrl(usuario.Email)
		};
	}

	public async Task<UsuarioDTO?> CadastrarAsync(CadastroDTO cadastroDTO)
	{
		if (!IsValidEmail(cadastroDTO.Email))
			throw new ArgumentException("E-mail inválido.");

		if (await _usuarioRepository.EmailExisteAsync(cadastroDTO.Email))
			throw new InvalidOperationException("E-mail já cadastrado.");

		if (string.IsNullOrWhiteSpace(cadastroDTO.Senha) || cadastroDTO.Senha.Length < 6)
			throw new ArgumentException("Senha deve ter no mínimo 6 caracteres.");

		var usuario = new Usuario
		{
			Email = cadastroDTO.Email.ToLower().Trim(),
			SenhaHash = HashSenha(cadastroDTO.Senha),
			Telefone = string.IsNullOrWhiteSpace(cadastroDTO.Telefone) ? null : cadastroDTO.Telefone.Trim(),
			Nome = cadastroDTO.Nome.Trim(),
			DataCadastro = DateTime.Now,
			EmailVerificado = false,
			TokenVerificacao = GerarTokenVerificacao(),
			TokenVerificacaoExpiracao = DateTime.Now.AddDays(7)
		};

		await _usuarioRepository.CriarAsync(usuario);

		try
		{
			await EnviarEmailBoasVindasAsync(usuario.Email, usuario.Nome);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
		}

		return new UsuarioDTO
		{
			Id = usuario.Id,
			Email = usuario.Email,
			Telefone = usuario.Telefone,
			Nome = usuario.Nome,
			FotoUrl = GerarGravatarUrl(usuario.Email)
		};
	}

	public async Task<UsuarioDTO?> ObterUsuarioPorIdAsync(int id)
	{
		var usuario = await _usuarioRepository.ObterPorIdAsync(id);
		if (usuario == null)
			return null;

		return new UsuarioDTO
		{
			Id = usuario.Id,
			Email = usuario.Email,
			Telefone = usuario.Telefone,
			Nome = usuario.Nome,
			FotoUrl = GerarGravatarUrl(usuario.Email)
		};
	}

	public async Task<bool> VerificarEmailAsync(string token)
	{
		var usuario = await _usuarioRepository.ObterPorEmailAsync(token);
		if (usuario != null && !usuario.EmailVerificado)
		{
			usuario.EmailVerificado = true;
			await _usuarioRepository.AtualizarAsync(usuario);
			return true;
		}
		return false;
	}

	public async Task EnviarEmailBoasVindasAsync(string email, string nome)
	{
		await _emailService.EnviarEmailBoasVindasAsync(email, nome);
	}

	public async Task EnviarEmailLoginAsync(string email, string nome)
	{
		await _emailService.EnviarEmailLoginAsync(email, nome);
	}

	private string HashSenha(string senha)
	{
		using (var sha256 = SHA256.Create())
		{
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
			return Convert.ToBase64String(hashedBytes);
		}
	}

	private bool VerificarSenha(string senha, string senhaHash)
	{
		var hashSenha = HashSenha(senha);
		return hashSenha == senhaHash;
	}

	private string GerarGravatarUrl(string email)
	{
		using (var md5 = MD5.Create())
		{
			var emailBytes = Encoding.UTF8.GetBytes(email.ToLower().Trim());
			var hashBytes = md5.ComputeHash(emailBytes);
			var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			return $"https://www.gravatar.com/avatar/{hash}?d=identicon&s=200";
		}
	}

	private bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;

		var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
		return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
	}

	private string GerarTokenVerificacao()
	{
		return Guid.NewGuid().ToString("N");
	}
}

