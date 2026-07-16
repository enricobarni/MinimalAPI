using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Infraestrutura.Db;
using ProjetoAPI.Dominio.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost(
    "/login",
    (LoginDTO loginDTO) =>
    {
        if (loginDTO.Email == "admin@teste.com" && loginDTO.Password == "123456")
        {
            return Results.Ok("Login realizado com sucesso! ");
        }
        else
        {
            return Results.Unauthorized();
        }
    }
);

app.Run();
