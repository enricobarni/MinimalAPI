using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.DTOs;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Interfaces;
using MinimalAPI.Dominio.ModelViews;
using MinimalAPI.Dominio.Servicos;
using MinimalAPI.Infraestrutura.Db;
using MinimalAPI.Infraestrutura.Interfaces;
using ProjetoAPI.Dominio.DTOs;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer"));
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost(
        "administradores/login",
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
    )
    .WithTags("Administradores");
#endregion

#region Veículos
app.MapPost(
        "/veiculos",
        ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
        {
            var veiculo = new Veiculo
            {
                Nome = veiculoDTO.Nome,
                Marca = veiculoDTO.Marca,
                Ano = veiculoDTO.Ano,
            };
            veiculoServico.Adicionar(veiculo);
            return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
        }
    )
    .WithTags("Veiculos");

app.MapGet(
        "/veiculos",
        ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>
        {
            var veiculo = veiculoServico.Todos(pagina);
            return Results.Ok(veiculo);
        }
    )
    .WithTags("Veiculos");

app.MapGet(
        "/veiculos/{Id}",
        ([FromRoute] int id, IVeiculoServico veiculoServico) =>
        {
            var veiculo = veiculoServico.BuscarPorId(id);
            if (veiculo != null)
            {
                return Results.Ok(veiculo);
            }
            else
            {
                return Results.NotFound();
            }
        }
    )
    .WithTags("Veiculos");

app.MapPut(
        "/veiculos/{Id}",
        ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
        {
            var veiculo = veiculoServico.BuscarPorId(id);
            if (veiculo != null)
            {
                veiculo.Nome = veiculoDTO.Nome;
                veiculo.Marca = veiculoDTO.Marca;
                veiculo.Ano = veiculoDTO.Ano;

                veiculoServico.Atualizar(veiculo);
                return Results.Ok(veiculo);
            }
            else
            {
                return Results.NotFound();
            }
        }
    )
    .WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
