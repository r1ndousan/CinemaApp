using CinemaConsole.Commands;
using CinemaConsole.Data;
using CinemaConsole.Data.Entities;
using CinemaConsole.Data.Repositories;
using CinemaConsole.Data.Repositories.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CinemaConsole.Models;

var builder = WebApplication.CreateBuilder(args);

// JWT‑конфиг
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
// ---- 1) EF Core ----
var cs = builder.Configuration.GetConnectionString("CinemaDb");
builder.Services.AddDbContext<CinemaDbContext>(opt =>
    opt.UseSqlServer(cs));



// ---- 2) Репозитории ----
builder.Services.AddScoped<IClientRepository, EfClientRepository>();
builder.Services.AddScoped<ISessionRepository, EfSessionRepository>();
builder.Services.AddScoped<CommandDispatcher>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IBookingRepository, EfBookingRepository>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);



var app = builder.Build();

app.UseAuthentication();



app.MapGet("/", () => "API is up and running!");
// ---- 3) Клиенты ----
//app.MapGet("/clients", async (IClientRepository repo) =>
//    Results.Ok(await repo.GetAllClientsAsync()));

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
//app.MapGet("/sessions", async (ISessionRepository repo) =>
 //   Results.Ok(await repo.GetAllSessionsAsync()));

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

// --- Users ---
//app.MapGet("/users", async (IUserRepository repo) =>
 //   Results.Ok(await repo.GetAllUsersAsync()));

app.MapGet("/users/{id:int}", async (int id, IUserRepository repo) =>
    (await repo.GetUserByIdAsync(id)) is User u
        ? Results.Ok(u)
        : Results.NotFound());

app.MapPost("/users", async (User u, IUserRepository repo, CommandDispatcher cmdDisp) =>
{
    var cmd = new AddUserCommand(repo, u);
    await cmdDisp.DispatchAsync(cmd);
    return Results.Created($"/users/{u.Id}", u);
});


// --- Bookings ---
//app.MapGet("/bookings", async (IBookingRepository repo) =>
//    Results.Ok(await repo.GetAllBookingsAsync()));

app.MapGet("/bookings/{id:int}", async (int id, IBookingRepository repo) =>
    (await repo.GetBookingByIdAsync(id)) is Booking b
        ? Results.Ok(b)
        : Results.NotFound());

app.MapPost("/bookings", async (Booking b, IBookingRepository repo, CommandDispatcher cmdDisp) =>
{
    var cmd = new AddBookingCommand(repo, b);
    await cmdDisp.DispatchAsync(cmd);
    return Results.Created($"/bookings/{b.Id}", b);
});

app.MapDelete("/bookings/{id:int}", async (int id, IBookingRepository repo) =>
{
    await repo.DeleteBookingAsync(id);
    return Results.NoContent();
})
.RequireAuthorization();
// PUT и DELETE — аналогично


// POST /auth/register
// РЕГИСТРАЦИЯ
app.MapPost("/auth/register", async (AuthRequestDto dto, IUserRepository repo) =>
{
    // хешируем пароль (рекомендуется)
    var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    var user = new User
    {
        Username = dto.Username,
        PasswordHash = hash,
        Role = "User"
    };

    await repo.AddUserAsync(user);
    return Results.Created($"/users/{user.Id}", new { user.Id, user.Username });
});

app.UseAuthorization();

// POST /auth/login
app.MapPost("/auth/login", async (LoginRequest creds, IUserRepository repo) =>
{
    var user = await repo.GetUserByUsernameAsync(creds.Username);
    if (user is null)
        return Results.Unauthorized();

    // Правильная проверка хеша:
    bool ok = BCrypt.Net.BCrypt.Verify(creds.Password, user.PasswordHash);
    if (!ok)
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

    var jwtSection = builder.Configuration.GetSection("Jwt");
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
    var token = new JwtSecurityToken(
        issuer: jwtSection["Issuer"],
        audience: jwtSection["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpireMinutes"]!)),
        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
    );

    var jwtString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = jwtString });
});

app.MapGet("/clients", async (IClientRepository repo) =>
        Results.Ok(await repo.GetAllClientsAsync())).RequireAuthorization();
app.MapGet("/sessions", async (ISessionRepository repo) =>
        Results.Ok(await repo.GetAllSessionsAsync())).RequireAuthorization();
app.MapGet("/bookings", async (IBookingRepository repo) =>
        Results.Ok(await repo.GetAllBookingsAsync())).RequireAuthorization();


app.Run("http://localhost:5000");
