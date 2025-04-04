namespace ProgramacaoIV.Venda.Api.Entidades;

public sealed class Cliente : AbstractEntity
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    /// <summary>
    /// To EF Core
    /// </summary>
    private Cliente() : base() { }

    public Cliente(string nome, string cpf, string endereco, string telefone)
    {
        Nome = nome.ToUpper() ?? throw new ArgumentNullException(nameof(nome));
        CPF = cpf ?? throw new ArgumentNullException(nameof(nome));
        Endereco = endereco.ToUpper() ?? throw new ArgumentNullException(nameof(nome));
        Telefone = telefone ?? throw new ArgumentNullException(nameof(nome));
    }
}