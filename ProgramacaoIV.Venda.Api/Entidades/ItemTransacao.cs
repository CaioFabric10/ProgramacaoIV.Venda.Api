using System.Linq.Expressions;

namespace ProgramacaoIV.Venda.Api.Entidades;

public sealed class ItemTransacao : AbstractEntity
{
    public Produto Produto { get; private set; }
    public decimal Valor { get; private set; } = decimal.Zero;
    public decimal Quantidade { get; private set; } = decimal.Zero;
    public decimal Total => Valor * Quantidade;

    /// <summary>
    /// To EF Core
    /// </summary>
    private ItemTransacao() : base() { }

    public ItemTransacao(Produto produto, decimal quantidade)
    {
        Produto = produto ?? throw new ArgumentNullException(nameof(produto));
        Valor = produto.PrecoVenda;
        Quantidade = quantidade;
    }
}