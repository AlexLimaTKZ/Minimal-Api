var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Olá, mundo!");
app.MapGet("/sobre", () => Results.Ok(new
{
    mensagem = "Meu primeiro endpoint em uma Minimal API"
}));

app.Run();
