using WebAPI1.Dtos.Cliente;
using WebAPI1.Interfaces.Cliente;
using WebAPI1.Interfaces.Livro;
using WebAPI1.Repositorios.Cliente;
using WebAPI1.Repositorios.Cliente_Livro;
using WebAPI1.Repositorios.Livro;
using WebAPI1.Services.Cliente_Livro;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registros atuais
builder.Services.AddSingleton<ClienteRepositorio>(); // Necessário se algum serviço injeta o concreto
builder.Services.AddSingleton<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddSingleton<ILivroRepositorio, LivroRepositorio>();
builder.Services.AddSingleton<IClienteLivroRepositorio, ClienteLivroService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
