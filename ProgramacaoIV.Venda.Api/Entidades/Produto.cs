namespace ProgramacaoIV.Venda.Api.Entidades;

public sealed class Produto : AbstractEntity
{
    public string EAN { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal PrecoCompra { get; set; } = decimal.Zero;
    public decimal PrecoVenda { get; set; } = decimal.Zero;
    public decimal Estoque { get; set; } = decimal.Zero;

    /// <summary>
    /// To EF Core
    /// </summary>
    private Produto() : base() { }

    public Produto(string ean, string descricao, decimal precoCompra, decimal precoVenda, decimal estoque)
    {
        EAN = ean ?? throw new ArgumentNullException(nameof(ean));
        Descricao = descricao.ToUpper() ?? throw new ArgumentNullException(nameof(Descricao));
        PrecoCompra = precoCompra;
        PrecoVenda = precoVenda;
        Estoque = estoque;
    }
}