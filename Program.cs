using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.Infraestrutura.Db;
using MinimalAPI.Infraestrutura.Interfaces;
using ProjetoAPI.Dominio.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost(
    "/login",
    ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
    {
        if (administradorServico.Login(loginDTO) != null)
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
