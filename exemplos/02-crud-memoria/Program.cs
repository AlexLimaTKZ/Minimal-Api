var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var veiculos = new List<Veiculo>();
var proximoId = 1;

app.MapGet("/veiculos", () => Results.Ok(veiculos));

app.MapGet("/veiculos/{id:int}", (int id) =>
{
    var veiculo = veiculos.FirstOrDefault(v => v.Id == id);
    return veiculo is null ? Results.NotFound() : Results.Ok(veiculo);
});

app.MapPost("/veiculos", (CriarVeiculoDto dto) =>
{
    var veiculo = new Veiculo
    {
        Id = proximoId++,
        Marca = dto.Marca,
        Modelo = dto.Modelo,
        Ano = dto.Ano,
        Cor = dto.Cor
    };

    veiculos.Add(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
});

app.MapPut("/veiculos/{id:int}", (int id, CriarVeiculoDto dto) =>
{
    var veiculo = veiculos.FirstOrDefault(v => v.Id == id);
    if (veiculo is null) return Results.NotFound();

    veiculo.Marca = dto.Marca;
    veiculo.Modelo = dto.Modelo;
    veiculo.Ano = dto.Ano;
    veiculo.Cor = dto.Cor;

    return Results.NoContent();
});

app.MapDelete("/veiculos/{id:int}", (int id) =>
{
    var veiculo = veiculos.FirstOrDefault(v => v.Id == id);
    if (veiculo is null) return Results.NotFound();

    veiculos.Remove(veiculo);
    return Results.NoContent();
});

app.Run();

public class Veiculo
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
}

public record CriarVeiculoDto(string Marca, string Modelo, int Ano, string Cor);
