using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramacaoIV.Venda.Api.Entidades;

namespace ProgramacaoIV.Venda.Api.Map;

public sealed class ItemTransacaoMap : AbstractEntidadeMap<ItemTransacao>
{
    public override void Configure(EntityTypeBuilder<ItemTransacao> builder)
    {
        base.Configure(builder);

        builder.ToTable("TRANSACAO_ITEM");
        
        builder.Property(x => x.Valor).HasColumnName("VL_ITEM").HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Quantidade).HasColumnName("QT_ITEM").HasPrecision(18, 2).IsRequired();

        builder.HasOne(x => x.Produto)
               .WithMany()
               .HasForeignKey("ID_PRODUTO")
               .OnDelete(DeleteBehavior.Restrict) // Evita que entidades filhas sejam excluidas quando a principal e excluida
               .IsRequired();

        builder.Ignore(x => x.Total);
    }
}