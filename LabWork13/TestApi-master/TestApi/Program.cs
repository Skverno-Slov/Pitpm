using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using TestApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TestApi",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton<AnimeService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/token", (string username, HttpContext context) =>
{
    var jwtSettings = context.RequestServices.GetRequiredService<IConfiguration>()
        .GetSection("JwtSettings").Get<JwtSettings>()!;

    var tokenHandler = new JsonWebTokenHandler();
    var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, username)
        ]),
        Expires = DateTime.UtcNow.AddDays(jwtSettings.AccessTokenExpirationMinutes),
        Issuer = jwtSettings.Issuer,
        Audience = jwtSettings.Audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return Results.Ok(new { Token = token });

})
.WithName("Login")
.WithOpenApi();


app.MapGet("/hello", [Authorize] (HttpContext context) =>
{
    return Results.Ok($"Привет, {context.User.Identity?.Name}");
})
.WithName("Hello")
.WithOpenApi();

app.MapGet("/anime", (AnimeService service) =>
{
    return Results.Ok(service.GetAnimes());
})
.WithName("GetAnimes")
.WithOpenApi();

app.MapGet("/anime/{id:int}", (int id, AnimeService service) =>
{
    try
    {
        var anime = service.GetAnime(id);
        return Results.Ok(anime);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { Message = $"Аниме с id={id} не найдено" });
    }
})
.WithName("GetAnime")
.WithOpenApi();

app.MapPost("/anime", [Authorize] (Anime anime, AnimeService service) =>
{
    anime.Id = service.GetAnimes().Max(a => a.Id) + 1;
    service.AddAnime(anime);
    return Results.Created($"/anime/{anime.Id}", anime);
})
.WithName("AddAnime")
.WithOpenApi();

app.MapDelete("/anime/{id:int}", [Authorize] (int id, AnimeService service) =>
{
    try
    {
        service.RemoveAnime(id);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { Message = $"Аниме с id={id} не найдено" });
    }
})
.WithName("RemoveAnime")
.WithOpenApi();

app.Run();
