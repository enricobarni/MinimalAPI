using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
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

var key = builder.Configuration.GetSection("Jwt").ToString();

if (string.IsNullOrEmpty(key))
    key = "123456";

builder
    .Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira somente o token JWT",
        }
    );

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
    });
});

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServer"));
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region PARTE_Administradores
{
    #region TokenJwt
    string GerarTokenJwt(Administrador administrador)
    {
        if (!string.IsNullOrEmpty(key))
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Email", administrador.Email),
                new Claim("Perfil", administrador.Perfil),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        else
        {
            return string.Empty;
        }
    }
    #endregion

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
        .RequireAuthorization()
        .WithTags("Administradores");
    #endregion

    #region RouteLogin
    app.MapPost(
            "/administradores/login",
            ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
            {
                var admin = administradorServico.Login(loginDTO);
                if (admin != null)
                {
                    string token = GerarTokenJwt(admin);
                    return Results.Ok(
                        new AdminLogado
                        {
                            Email = admin.Email,
                            Perfil = admin.Perfil,
                            Token = token,
                        }
                    );
                }
                else
                {
                    return Results.Unauthorized();
                }
            }
        )
        .AllowAnonymous()
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
        .RequireAuthorization()
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
        .RequireAuthorization()
        .WithTags("Administradores");
    #endregion
}
#endregion

#region PARTE_Veículos
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
        .RequireAuthorization()
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
        .RequireAuthorization()
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
        .RequireAuthorization()
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
        .RequireAuthorization()
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
        .RequireAuthorization()
        .WithTags("Veiculos");
    #endregion
}
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion
