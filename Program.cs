using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjetoAPI.Dominio.DTOs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost(
    "/login",
    (LoginDTO loginDTO) =>
    {
        if (loginDTO.Username == "admin@teste.com" && loginDTO.Password == "123456")
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
