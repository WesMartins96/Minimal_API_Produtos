using Microsoft.EntityFrameworkCore;
using Minimal_API_Produtos.Data;
using Minimal_API_Produtos.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o serviço do DbContext com a configuração do PostgreSQL
builder.Services.AddDbContext<Context>(op =>
op.UseNpgsql(builder.Configuration.GetConnectionString("MinimalAPIDatabase")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints da API
app.MapGet("/produtos", async (Context db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id}", async (int id, Context db) =>
    await db.Produtos.FindAsync(id)
    is Produto produto
    ? Results.Ok(produto)
    : Results.NotFound());

app.MapPost("/produtos/{id}", async (Produto produto, Context db) =>
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();
    return Results.Created($"/produtos/{produto.Id}", produto);
});

app.MapPut("/produtos/{id}", async (int id, Produto produtoAtualizado, Context db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is null)
    {
        return Results.NotFound();
    }

    produto.Nome = produtoAtualizado.Nome;
    produto.Descricao = produtoAtualizado.Descricao;
    produto.Preco = produtoAtualizado.Preco;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/produtos/{id}", async (int id, Context db) =>
{
    var produto = await db.Produtos.FindAsync(id);
    if (produto is not null)
    {
        db.Produtos.Remove(produto);
        await db.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
