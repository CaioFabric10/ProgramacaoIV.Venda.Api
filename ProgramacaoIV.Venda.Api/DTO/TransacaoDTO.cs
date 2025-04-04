namespace ProgramacaoIV.Venda.Api.DTO;

public sealed class TransacaoDTO
{
    public sealed class TransacaoCapaRequest
    {
        public Guid IdCliente { get; set; } = Guid.Empty;
        public Guid idVendedor { get; set; } = Guid.Empty;
    }

    public sealed class TransacaoItemRequest
    {
        public Guid IdProduto { get; set; } = Guid.Empty;
        public decimal Quantidade { get; set; } = decimal.Zero;
    }
}