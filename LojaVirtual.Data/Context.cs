using LojaVirtual.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data;

public class Context : DbContext
{
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ProdutoItem> ProdutoItems { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Produto>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<ProdutoItem>()
            .HasKey(s => s.Id);
                modelBuilder.Entity<ProdutoItem>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<ProdutoItem>()
            .HasOne(s => s.Carrinho)
            .WithMany(c => c.Itens)
            .HasForeignKey(s => s.CarrinhoId);

        modelBuilder.Entity<ProdutoItem>()
            .HasOne(s => s.Produto)
            .WithMany(c => c.Itens)
            .HasForeignKey(s => s.ProdutoId);

        modelBuilder.Entity<Usuario>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Pedido>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioId);

        modelBuilder.Entity<PedidoItem>()
            .HasKey(pi => pi.Id);

        modelBuilder.Entity<PedidoItem>()
            .HasOne(pi => pi.Pedido)
            .WithMany(p => p.Itens)
            .HasForeignKey(pi => pi.PedidoId);

        modelBuilder.Entity<PedidoItem>()
            .HasOne(pi => pi.Produto)
            .WithMany()
            .HasForeignKey(pi => pi.ProdutoId);

        base.OnModelCreating(modelBuilder);
    }
}
