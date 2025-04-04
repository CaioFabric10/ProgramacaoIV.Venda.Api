using Microsoft.EntityFrameworkCore;
using ProgramacaoIV.Venda.Api.Entidades;
using ProgramacaoIV.Venda.Api.Map;

namespace ProgramacaoIV.Venda.Api.Context;

public sealed class VendaContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Vendedor> Vendedores { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }
    public DbSet<ItemTransacao> ItensTransacoes { get; set; }

    public VendaContext(DbContextOptions<VendaContext> options) : base(options) 
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();

        VendaInitilizer.Initialize(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ClienteMap());
        modelBuilder.ApplyConfiguration(new VendedorMap());
        modelBuilder.ApplyConfiguration(new ProdutoMap());
        modelBuilder.ApplyConfiguration(new ItemTransacaoMap());
        modelBuilder.ApplyConfiguration(new TransacaoMap());
    }
}