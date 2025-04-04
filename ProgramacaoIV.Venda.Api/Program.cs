using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgramacaoIV.Venda.Api.Context;
using ProgramacaoIV.Venda.Api.Entidades;
using static ProgramacaoIV.Venda.Api.DTO.TransacaoDTO;

var connectionString = "Server=localhost;Port=3306;Database=umfg_venda_api;Uid=root;Pwd=root;";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VendaContext>(options => options.UseMySQL(connectionString));

#region endpoints

var app = builder.Build();

#region cliente

app.MapGet("/clientes", async (VendaContext context) => await context.Clientes.Where(x => x.IsAtivo).ToListAsync());

app.MapGet("/clientes/{id}", async (string id, VendaContext context) =>
    await context.Clientes.Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync() is Cliente cliente ? Results.Ok(cliente) : Results.NotFound());

app.MapPost("/clientes", async (Cliente cliente, VendaContext context) =>
{
    context.Clientes.Add(cliente);
    await context.SaveChangesAsync();

    return Results.Created($"/clientes/{cliente.Id}", cliente);
});

app.MapPut("/clientes/{id}", async (string id, Cliente input, VendaContext context) =>
{
    var cliente = await context.Clientes.Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync();

    if (cliente is null)
        return Results.NotFound();

    cliente.Nome = input.Nome;
    cliente.CPF = input.CPF;
    cliente.Endereco = input.Endereco;
    cliente.Telefone = input.Telefone;

    cliente.AtualizarDataAtualizacao();

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/clientes/{id}", async (string id, VendaContext context) =>
{
    var cliente = await context.Clientes.Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync();

    if (cliente is null)
        return Results.NotFound();

    cliente.Inativar();

    await context.SaveChangesAsync();

    return Results.NoContent();
});

#endregion cliente

#region produto

app.MapGet("/produtos", async (VendaContext context) => await context.Produtos.Where(x => x.IsAtivo).ToListAsync());

app.MapGet("/produtos/{id}", async (string id, VendaContext context) =>
    await context.Produtos.Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync() is Produto produto ? Results.Ok(produto) : Results.NotFound());

app.MapPost("/produtos", async (Produto produto, VendaContext context) =>
{
    context.Produtos.Add(produto);
    await context.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/{id}", async (string id, Produto input, VendaContext context) =>
{
    var produto = await context.Produtos.Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync();

    if (produto is null)
        return Results.NotFound();

    produto.EAN = input.EAN;
    produto.Descricao = input.Descricao;
    produto.PrecoCompra = input.PrecoCompra;
    produto.PrecoVenda = input.PrecoVenda;
    produto.Estoque = input.Estoque;

    produto.AtualizarDataAtualizacao();

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/produtos/{id}", async (string id, VendaContext context) =>
{
    var produto = await context.Produtos.Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync();

    if (produto is null)
        return Results.NotFound();

    produto.Inativar();

    await context.SaveChangesAsync();

    return Results.NoContent();
});

#endregion produto

#region transacao
app.MapGet("/transacoes", async (VendaContext context) 
    => await context.Transacoes
        .Include(x => x.Cliente)
        .Include(x => x.Vendedor)
        .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
        .Where(x => x.IsAtivo)
        .ToListAsync());

app.MapGet("/transacoes/{id}", async (string id, VendaContext context) 
    => await context.Transacoes
        .Include(x => x.Cliente)
        .Include(x => x.Vendedor)
        .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
        .Where(x => x.Id == Guid.Parse(id) && x.IsAtivo)
        .FirstOrDefaultAsync()
        is Transacao transacao
            ? Results.Ok(transacao)
            : Results.NotFound());

app.MapPost("/transacoes", async ([FromBody] TransacaoCapaRequest request, VendaContext context) =>
{
    var cliente = await context.Clientes.Where(x => x.Id == request.IdCliente && x.IsAtivo).FirstOrDefaultAsync();
    var vendedor = await context.Set<Vendedor>().Where(x => x.Id == request.idVendedor && x.IsAtivo).FirstOrDefaultAsync();

    if (cliente is null || vendedor is null)
        return Results.NotFound("Cliente ou Vendedor inválido");

    var transacao = new Transacao(cliente, vendedor);

    context.Transacoes.Add(transacao);
    await context.SaveChangesAsync();

    return Results.Created($"/transacoes/{transacao.Id}", transacao);
});

app.MapPost("/transacoes/{idTransacao}/itens", async (string idTransacao, [FromBody] TransacaoItemRequest request, VendaContext context) =>
{
    try
    {
        var transacao = await context.Transacoes
            .Include(x => x.Cliente)
            .Include(x => x.Itens)
                .ThenInclude(x => x.Produto)
            .Where(x => x.Id == Guid.Parse(idTransacao) && x.IsAtivo)
            .FirstOrDefaultAsync();

        if (transacao is null)
            return Results.NotFound();

        var produto = await context.Produtos.Where(x => x.Id == request.IdProduto && x.IsAtivo).FirstOrDefaultAsync();

        if (produto is null)
            return Results.NotFound();

        ArgumentNullException.ThrowIfNull(produto, nameof(produto));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(request.Quantidade, decimal.Zero, nameof(request.Quantidade));

        if (produto.Estoque < request.Quantidade)
            throw new InvalidOperationException($"Quantidade solicitada ({request.Quantidade}) excede o estoque disponível ({produto.Estoque}) para o produto {produto.EAN} - {produto.Descricao}. Ajuste a quantidade ou verifique o estoque.");

        produto.Estoque -= request.Quantidade;

        var item = new ItemTransacao(produto, request.Quantidade);

        transacao.AdicionarItem(item);

        context.ItensTransacoes.Add(item);
        context.Produtos.Update(produto);
        context.Transacoes.Update(transacao);

        await context.SaveChangesAsync();

        return Results.Created($"/transacoes/{transacao.Id}", transacao);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/transacoes/{idTransacao}/itens/{idItem}", async (string idTransacao, string idItem, VendaContext context) =>
{
    var transacao = await context.Transacoes
            .Include(x => x.Cliente)
            .Include(x => x.Itens)
                .ThenInclude(x => x.Produto)
            .Where(x => x.Id == Guid.Parse(idTransacao) && x.IsAtivo)
            .FirstOrDefaultAsync();

    if (transacao is null)
        return Results.NotFound();

    var item = transacao.Itens.FirstOrDefault(x => x.Id == Guid.Parse(idItem));

    if (item is null)
        return Results.NotFound();

    var produto = await context.Produtos.Where(x => x.Id == item.Produto.Id && x.IsAtivo).FirstOrDefaultAsync();

    if (produto is null)
        return Results.NotFound();

    produto.Estoque += item.Quantidade;
    transacao.RemoverItem(item);

    context.ItensTransacoes.Remove(item);
    context.Produtos.Update(produto);
    context.Transacoes.Update(transacao);

    await context.SaveChangesAsync();

    return Results.Created($"/transacoes/{transacao.Id}", transacao);
});

app.MapDelete("/transacoes/{id}", async (string id, VendaContext context) =>
{
    var transacao = await context.Transacoes
        .Include(x => x.Cliente)
        .Include(x => x.Itens)
            .ThenInclude(x => x.Produto)
        .Where(x => x.Id == Guid.Parse(id) && x.IsAtivo)
        .FirstOrDefaultAsync();

    if (transacao is null)
        return Results.NotFound();


    foreach (var item in transacao.Itens)
    {
        var produto = await context.Produtos.Where(x => x.Id == item.Produto.Id && x.IsAtivo).FirstOrDefaultAsync();

        if (produto is null)
            continue;

        produto.Estoque += item.Quantidade;
        
        item.Inativar();

        context.ItensTransacoes.Update(item);
        context.Produtos.Update(produto);
    }

    transacao.Inativar();
    context.Transacoes.Update(transacao);

    await context.SaveChangesAsync();

    return Results.NoContent();
});

#endregion transacao

#region vendedores

app.MapGet("/vendedores", async (VendaContext context) => await context.Set<Vendedor>().Where(x => x.IsAtivo).ToListAsync());

app.MapGet("/vendedores/{id}", async(string id, VendaContext context) =>
    await context.Set<Vendedor>().Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync()
        is Vendedor vendedor ? Results.Ok(vendedor) : Results.NotFound());

app.MapPost("/vendedores", async (Vendedor vendedor, VendaContext context) =>
{
    context.Set<Vendedor>().Add(vendedor);
    await context.SaveChangesAsync();
    return Results.Created($"/vendedores/{vendedor.Id}", vendedor);
});

app.MapPut("/vendedores/{id}", async (string id, Vendedor Input, VendaContext context) =>
{
    var vendedor = await context.Set<Vendedor>().Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync();
    
    if (vendedor is null)
        return Results.NotFound();

    vendedor.Nome = Input.Nome;
    vendedor.Email = Input.Email;
    vendedor.AtualizarDataAtualizacao();

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/vendedores/{id}", async (string id, VendaContext context) =>
{
    var vendedor = await context.Set<Vendedor>().Where(x => x.Id == Guid.Parse(id) && x.IsAtivo).FirstOrDefaultAsync();

    if (vendedor is null)
        return Results.NotFound();

    vendedor.Inativar();
    await context.SaveChangesAsync();

    return Results.NoContent();
});

#endregion vendedores
#endregion endpoints

app.UseHttpsRedirection();
app.Run();