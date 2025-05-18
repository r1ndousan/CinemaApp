using CinemaConsole.Data;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;
using CinemaConsole.Data.Repositories.Ef;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;   // для GetConnectionString

var builder = WebApplication.CreateBuilder(args);

// ---- 1) EF Core ----
var cs = builder.Configuration.GetConnectionString("CinemaDb");
builder.Services.AddDbContext<CinemaDbContext>(opt =>
    opt.UseSqlServer(cs));

// ---- 2) Репозитории ----
builder.Services.AddScoped<IClientRepository, EfClientRepository>();
builder.Services.AddScoped<ISessionRepository, EfSessionRepository>();

var app = builder.Build();

// ---- 3) Клиенты ----
app.MapGet("/clients", async (IClientRepository repo) =>
    Results.Ok(await repo.GetAllClientsAsync()));

app.MapGet("/clients/{id:int}", async (int id, IClientRepository repo) =>
    (await repo.GetClientByIdAsync(id)) is Client c
        ? Results.Ok(c)
        : Results.NotFound());

app.MapPost("/clients", async (Client c, IClientRepository repo) =>
{
    await repo.AddClientAsync(c);
    return Results.Created($"/clients/{c.Id}", c);
});

app.MapPut("/clients/{id:int}", async (int id, Client c, IClientRepository repo) =>
{
    c.Id = id;
    await repo.UpdateClientAsync(c);
    return Results.NoContent();
});

app.MapDelete("/clients/{id:int}", async (int id, IClientRepository repo) =>
{
    await repo.DeleteClientAsync(id);
    return Results.NoContent();
});

// ---- 4) Сеансы ----
app.MapGet("/sessions", async (ISessionRepository repo) =>
    Results.Ok(await repo.GetAllSessionsAsync()));

app.MapGet("/sessions/{id:int}", async (int id, ISessionRepository repo) =>
    (await repo.GetSessionByIdAsync(id)) is Session s
        ? Results.Ok(s)
        : Results.NotFound());

app.MapPost("/sessions", async (Session s, ISessionRepository repo) =>
{
    await repo.AddSessionAsync(s);
    return Results.Created($"/sessions/{s.Id}", s);
});

app.MapPut("/sessions/{id:int}", async (int id, Session s, ISessionRepository repo) =>
{
    s.Id = id;
    await repo.UpdateSessionAsync(s);
    return Results.NoContent();
});

app.MapDelete("/sessions/{id:int}", async (int id, ISessionRepository repo) =>
{
    await repo.DeleteSessionAsync(id);
    return Results.NoContent();
});

app.Run("http://localhost:5000");
