using LojaVirtual.Business.Interfaces;
using LojaVirtual.Business.Services;
using LojaVirtual.API.Repository;
using LojaVirtual.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IComputadorRepository, ComputadorRepository>();
builder.Services.AddScoped<IComputadorService, ComputadorService>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
bool dbConfigurado = false;

void ConfigurarBancoEmMemoria()
{
	builder.Services.AddDbContext<Context>(options =>
		options.UseInMemoryDatabase("CyberStoreMemory"));
}

bool ConseguirConexaoMySql(string connectionString, out ServerVersion serverVersion)
{
	serverVersion = ServerVersion.Parse("8.0.21-mysql");

	try
	{
		serverVersion = ServerVersion.AutoDetect(connectionString);
	}
	catch
	{
		// mantém versão padrão
	}

	try
	{
		using var connection = new MySqlConnection(connectionString);
		connection.Open();
		connection.Close();
		Console.WriteLine("✅ Conexão inicial com MySQL estabelecida.");
		return true;
	}
	catch (Exception ex)
	{
		Console.WriteLine("❌ Não foi possível conectar ao MySQL.");
		Console.WriteLine($"   Detalhes: {ex.Message}");
		if (ex.InnerException != null)
		{
			Console.WriteLine($"   Interno: {ex.InnerException.Message}");
		}
		return false;
	}
}

if (!string.IsNullOrWhiteSpace(mySqlConnection) && ConseguirConexaoMySql(mySqlConnection, out var serverVersionMySql))
{
	builder.Services.AddDbContext<Context>(options =>
	{
		options.UseMySql(mySqlConnection, serverVersionMySql, mysqlOptions =>
		{
			mysqlOptions.EnableRetryOnFailure(
				maxRetryCount: 1,
				maxRetryDelay: TimeSpan.FromSeconds(2),
				errorNumbersToAdd: null);
		});
	});
	dbConfigurado = true;
	Console.WriteLine("✅ MySQL configurado com sucesso!");
}
else
{
	if (!string.IsNullOrWhiteSpace(mySqlConnection))
	{
		Console.WriteLine("⚠️ Banco MySQL indisponível. Usando banco em memória para manter o site rápido.");
	}
	else
	{
		Console.WriteLine("ℹ️ Connection string não configurada. Usando banco em memória.");
	}
	ConfigurarBancoEmMemoria();
}

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.RoutePrefix = "swagger";
	});
}

app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

app.Use(async (context, next) =>
{
	var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
	if (!string.IsNullOrEmpty(token))
	{
		context.Items["Token"] = token;
	}
	await next();
});

app.UseAuthorization();
app.MapControllers();

Console.WriteLine("ℹ️ URLs serão gerenciadas pelo Visual Studio através do launchSettings.json");

try
{
	using (var scope = app.Services.CreateScope())
	{
		var context = scope.ServiceProvider.GetRequiredService<Context>();
		context.Database.EnsureCreated();
		if (dbConfigurado)
		{
			Console.WriteLine("✅ Banco de dados MySQL verificado/criado!");
		}
		else
		{
			Console.WriteLine("✅ Banco de dados em memória inicializado!");
		}
	}
}
catch (Exception ex)
{
	Console.WriteLine($"⚠️ Aviso ao criar banco de dados: {ex.Message}");
	Console.WriteLine($"   A aplicação continuará funcionando, mas os dados não serão persistidos.");
}

Console.WriteLine("🚀 API iniciando...");
Console.WriteLine($"📡 URLs disponíveis:");
if (app.Urls.Any())
{
	var firstUrl = "";
	foreach (var url in app.Urls)
	{
		Console.WriteLine($"   - {url}");
		if (string.IsNullOrEmpty(firstUrl))
			firstUrl = url;
	}
	if (!string.IsNullOrEmpty(firstUrl))
	{
		Console.WriteLine($"📚 Swagger: {firstUrl}/swagger");
		Console.WriteLine($"🌐 Frontend: {firstUrl}/");
	}
}
else
{
	Console.WriteLine("   (URLs serão configuradas pelo Visual Studio)");
}

try
{
	app.Run();
}
catch (System.Net.Sockets.SocketException ex) when (ex.Message.Contains("address already in use") || ex.Message.Contains("já está em uso"))
{
	Console.WriteLine($"❌ Erro: A porta está em uso!");
	Console.WriteLine($"   Tente fechar outras instâncias da aplicação ou altere a porta no launchSettings.json");
	Console.WriteLine($"   Detalhes: {ex.Message}");
	throw;
}
catch (Exception ex)
{
	Console.WriteLine($"❌ Erro fatal ao iniciar aplicação: {ex.Message}");
	Console.WriteLine($"   Tipo: {ex.GetType().Name}");
	if (ex.InnerException != null)
	{
		Console.WriteLine($"   Erro interno: {ex.InnerException.Message}");
	}
	throw;
}