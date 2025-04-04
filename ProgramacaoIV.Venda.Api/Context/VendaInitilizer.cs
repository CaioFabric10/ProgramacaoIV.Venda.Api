

using ProgramacaoIV.Venda.Api.Entidades;

namespace ProgramacaoIV.Venda.Api.Context;

public sealed class VendaInitilizer
{
    public static void Initialize(VendaContext context)
    {
        context.Database.EnsureCreated();

        PopularTabelas(context);
    }

    private static void PopularTabelas(VendaContext context)
    {
        PopularCliente(context);
        PopularProduto(context);
    }

    private static void PopularCliente(VendaContext context)
    {
        if (context.Clientes.Any())
            return;

        context.Clientes.Add(new Cliente("CONSUMIDOR FINAL", "00000000000", "RUA TESTE, 999, CIANORTE-PR", "44999999999"));
        context.SaveChanges();
    }

    private static void PopularProduto(VendaContext context)
    {
        if (context.Produtos.Any())
            return;

        context.Produtos.Add(new Produto("123456789012", "PRODUTO TESTE", 15.90m, 79.90m, 999m));
        context.SaveChanges();
    }
}