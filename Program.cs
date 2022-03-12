using DevJobs.API.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
No appsettings.json é definido o atributo DevJobsCs com a string de conexão com server, password...
Utilizado oseguinte comando para definir a string de conexão oculta: dotnet user-secrets set "ConnectionStrings:DevJobsCs" "Server=JONATAS;Database=DevJobs;Trusted_Concetion=True"

Comando para iniciar a ocultação da string de conexão: dotnet user-secrets init
Comando para definir a conexão: dotnet user-secrets set "ConnectionStrings:DevJobsCs" "Server=JONATAS;Database=DevJobs;Trusted_Connection=True"
Comando para gerar migration: dotnet ef migrations add InitialMigration -o Persistence/Migrations
Comando para ativar a migration: dotnet ef database update

*/
var connectionString = builder.Configuration.GetConnectionString("DevJobsCs");

builder.Services.AddDbContext<DevJobsContext>(options => options.UseSqlServer(connectionString));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
