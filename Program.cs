using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);


/*
No appsettings.json é definido o atributo DevJobsCs com a string de conexão com server, password...
Utilizado o seguinte comando para definir a string de conexão oculta: dotnet user-secrets set "ConnectionStrings:DevJobsCs" "Server=JONATAS;Database=DevJobs;Trusted_Concetion=True"

1. Comando para iniciar a ocultação da string de conexão: dotnet user-secrets init
2. Comando para definir a conexão: dotnet user-secrets set "ConnectionStrings:DevJobsCs" "Server=JONATAS;Database=DevJobs;Trusted_Connection=True"
3. Comando para gerar migration: dotnet ef migrations add InitialMigration -o Persistence/Migrations (A cada necessidade de manutenção de BD deve gerar a migration para mapear o novo atributo ou nova tabela, etc.)
4. Comando para ativar a migration: dotnet ef database update

*/
var connectionString = builder.Configuration.GetConnectionString("DevJobsCs");

// Add services to the container.

builder.Services.AddDbContext<DevJobsContext>(options => options.UseInMemoryDatabase("DevJobs"));

//builder.Services.AddDbContext<DevJobsContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IJobVacancyRepository, JobVacancyRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => {
    c.SwaggerDoc("v1", new OpenApiInfo{
        Title = "DevJobs.API",
        Version = "v1",
        Contact = new OpenApiContact{
            Name = "Jonatas Silva - Developer",
            Email = "jonatasbatistasilva@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/jonatas-silva-b47ab298/")
        }
    });

    var xmlFile = "DevJobs.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

});

// //Configuração de log para aplicação
// builder.Host.ConfigureAppConfiguration((hostingContext, config) => {
//     Serilog.Log.Logger = new LoggerConfiguration()
//     .Enrich.FromLogContext()
//     .WriteTo.MSSqlServer(connectionString, 
//             sinkOptions: new MSSqlServerSinkOptions(){
//                 AutoCreateSqlTable = true,
//                 TableName = "Logs"
//             })
//     .WriteTo.Console()        
//     .CreateLogger();
// }).UseSerilog();

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
