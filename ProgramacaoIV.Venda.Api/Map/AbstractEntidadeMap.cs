using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramacaoIV.Venda.Api.Entidades;

namespace ProgramacaoIV.Venda.Api.Map;

public abstract class AbstractEntidadeMap<T> : IEntityTypeConfiguration<T> where T : AbstractEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.IsAtivo).HasColumnName("IN_ATIVO").IsRequired();
        builder.Property(x => x.DataCriacao).HasColumnName("DT_CRIACAO").IsRequired();
        builder.Property(x => x.DataAtualizacao).HasColumnName("DT_ATUALIZACAO").IsRequired();
    }
}