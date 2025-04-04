namespace ProgramacaoIV.Venda.Api.Entidades;

public abstract class AbstractEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public bool IsAtivo { get; private set; } = true;
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
    public DateTime DataAtualizacao { get; private set; } = DateTime.Now;

    /// <summary>
    /// To EF Core
    /// </summary>
    protected AbstractEntity() { }

    public void AtualizarDataAtualizacao() => DataAtualizacao = DateTime.Now;

    public void Inativar()
    {
        IsAtivo = false;
        AtualizarDataAtualizacao();
    }
}