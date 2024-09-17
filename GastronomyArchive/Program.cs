using AlimentosAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Añadir el contexto de base de datos con PostgreSQL
builder.Services.AddDbContext<AlimentosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Añadir Swagger para la documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger en el entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Especificar el endpoint para Swagger JSON
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alimentos API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
