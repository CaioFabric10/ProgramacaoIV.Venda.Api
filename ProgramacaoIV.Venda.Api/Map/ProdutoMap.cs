using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramacaoIV.Venda.Api.Entidades;

namespace ProgramacaoIV.Venda.Api.Map;

public sealed class ProdutoMap : AbstractEntidadeMap<Produto>
{
    public override void Configure(EntityTypeBuilder<Produto> builder)
    {
        base.Configure(builder);

        builder.ToTable("PRODUTO");

        builder.Property(x => x.EAN).HasColumnName("NR_EAN").HasMaxLength(13).IsRequired();
        builder.Property(x => x.Descricao).HasColumnName("DS_PRODUTO").HasMaxLength(100).IsRequired();
        builder.Property(x => x.PrecoCompra).HasColumnName("VL_PRECO_COMPRA").IsRequired();
        builder.Property(x => x.PrecoVenda).HasColumnName("VL_PRECO_VENDA").IsRequired();
        builder.Property(x => x.Estoque).HasColumnName("QT_ESTOQUE").IsRequired();
    }
}