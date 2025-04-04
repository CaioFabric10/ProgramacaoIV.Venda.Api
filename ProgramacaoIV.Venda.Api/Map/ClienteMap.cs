using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramacaoIV.Venda.Api.Entidades;

namespace ProgramacaoIV.Venda.Api.Map;

public sealed class ClienteMap : AbstractEntidadeMap<Cliente>
{
    public override void Configure(EntityTypeBuilder<Cliente> builder)
    {
        base.Configure(builder);

        builder.ToTable("CLIENTE");

        builder.Property(x => x.Nome).HasColumnName("NM_CLIENTE").HasMaxLength(100).IsRequired();
        builder.Property(x => x.CPF).HasColumnName("NR_CPF").HasMaxLength(11).IsRequired();
        builder.Property(x => x.Endereco).HasColumnName("DS_ENDERECO").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Telefone).HasColumnName("NR_TELEFONE").HasMaxLength(11).IsRequired();
    }
}