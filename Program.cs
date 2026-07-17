using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Dominio.DTOs;
using MinimalAPI.Dominio.Entidades;
using MinimalAPI.Dominio.Enuns;
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

#region PARTE_Administradores
{
    #region ValidaçãoAdm
    ErrosDeValidacao validaAdministradorDTO(AdministradorDTO administradorDTO)
    {
        var validacao = new ErrosDeValidacao() { Mensagens = new List<string>() };

        if (string.IsNullOrEmpty(administradorDTO.Email))
        {
            validacao.Mensagens.Add("o E-mail não pode ser vazio");
        }
        if (string.IsNullOrEmpty(administradorDTO.Password))
        {
            validacao.Mensagens.Add("A Senha não pode ser vazia");
        }
        if (administradorDTO.Perfil == null)
        {
            validacao.Mensagens.Add("o Perfil não pode ser vazio");
        }

        return validacao;
    }
    #endregion

    #region RouteBuscarPorIdAdm
    app.MapGet(
            "/administradores/{id}",
            ([FromRoute] int id, IAdministradorServico administradorServico) =>
            {
                var administrador = administradorServico.BuscarPorId(id);
                if (administrador != null)
                {
                    return Results.Ok(
                        new AdministradorModelView
                        {
                            Email = administrador.Email,
                            Id = administrador.Id,
                            Perfil = administrador.Perfil,
                        }
                    );
                }
                else
                {
                    return Results.NotFound();
                }
            }
        )
        .WithTags("Administradores");
    #endregion

    #region RouteLogin
    app.MapPost(
            "/administradores/login",
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

    #region RouteGetAllAdm
    app.MapGet(
            "/administradores/all",
            ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
            {
                var admins = new List<AdministradorModelView>();
                var administradores = administradorServico.Todos(pagina);
                foreach (var adm in administradores)
                {
                    admins.Add(
                        new AdministradorModelView
                        {
                            Email = adm.Email,
                            Id = adm.Id,
                            Perfil = adm.Perfil,
                        }
                    );
                }
                return Results.Ok(admins);
            }
        )
        .WithTags("Administradores");
    #endregion

    #region RouteCreateAdm
    app.MapPost(
            "/administradores/create",
            (
                [FromBody] AdministradorDTO administradorDTO,
                IAdministradorServico administradorServico
            ) =>
            {
                var validacao = validaAdministradorDTO(administradorDTO);
                if (validacao.Mensagens.Count > 0)
                {
                    return Results.BadRequest(validacao);
                }
                var administrador = new Administrador
                {
                    Email = administradorDTO.Email,
                    Password = administradorDTO.Password,
                    Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString(),
                };

                administradorServico.Adicionar(administrador);
                return Results.Created(
                    $"/administradores/{administrador.Id}",
                    new AdministradorModelView
                    {
                        Email = administrador.Email,
                        Id = administrador.Id,
                        Perfil = administrador.Perfil,
                    }
                );
            }
        )
        .WithTags("Administradores");
    #endregion
}
#endregion

#region Veículos
{
    #region ValidaçãoVeiculos
    ErrosDeValidacao validaVeiculoDTO(VeiculoDTO veiculoDTO)
    {
        var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };

        if (string.IsNullOrEmpty(veiculoDTO.Nome))
        {
            validacao.Mensagens.Add("O nome não pode ser vazio");
        }
        if (string.IsNullOrEmpty(veiculoDTO.Marca))
        {
            validacao.Mensagens.Add("A marca não pode ser vazia");
        }
        // 1885 foi o ano em que inventaram o primero carro a gasolina
        if (veiculoDTO.Ano < 1885)
        {
            validacao.Mensagens.Add("Ano inválido");
        }

        return validacao;
    }
    #endregion


    #region RouteCreateVeiculo
    app.MapPost(
            "/veiculos/create",
            ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
            {
                var validacao = validaVeiculoDTO(veiculoDTO);
                if (validacao.Mensagens.Count > 0)
                {
                    return Results.BadRequest(validacao);
                }

                var veiculo = new Veiculo
                {
                    Nome = veiculoDTO.Nome,
                    Marca = veiculoDTO.Marca,
                    Ano = veiculoDTO.Ano,
                };
                veiculoServico.Adicionar(veiculo);
                return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
            }
        )
        .WithTags("Veiculos");
    #endregion


    #region RouteGetAllVeiculos
    app.MapGet(
            "/veiculos/all",
            ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>
            {
                var veiculo = veiculoServico.Todos(pagina);
                return Results.Ok(veiculo);
            }
        )
        .WithTags("Veiculos");
    #endregion


    #region RouteBuscarPorIdVeiculo
    app.MapGet(
            "/veiculos/{id}",
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
    #endregion


    #region RouteAtualizarVeiculo
    app.MapPut(
            "/veiculos/{id}",
            ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
            {
                var veiculo = veiculoServico.BuscarPorId(id);
                if (veiculo != null)
                {
                    var validacao = validaVeiculoDTO(veiculoDTO);
                    if (validacao.Mensagens.Count > 0)
                    {
                        return Results.BadRequest(validacao);
                    }
                    else
                    {
                        veiculo.Nome = veiculoDTO.Nome;
                        veiculo.Marca = veiculoDTO.Marca;
                        veiculo.Ano = veiculoDTO.Ano;
                    }

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


    #region RouteDeletarVeiculo
    app.MapDelete(
            "/veiculos/{Id}",
            ([FromRoute] int id, IVeiculoServico veiculoServico) =>
            {
                var veiculo = veiculoServico.BuscarPorId(id);
                if (veiculo != null)
                {
                    veiculoServico.Deletar(veiculo);
                    return Results.NoContent();
                }
                else
                {
                    return Results.NotFound();
                }
            }
        )
        .WithTags("Veiculos");
    #endregion
}
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
