using API.DataAccess;
using API.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Tiusr4plMohisatarea4Context>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

//Login usuarios
app.MapPost("/login", async (Usuario user, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        if (String.IsNullOrEmpty(user.Identificacion) || String.IsNullOrEmpty(user.Contrasena))
        {
            return Results.NotFound(new { id = 404, mensaje = "Vacío" });
        }

        string result = string.Empty;
        byte[] OcultarString = System.Text.Encoding.Unicode.GetBytes(user.Contrasena);
        user.Contrasena = Convert.ToBase64String(OcultarString);

        if (await context.Usuarios.AnyAsync<Usuario>(x => x.Identificacion == user.Identificacion && x.Contrasena == user.Contrasena))
        {
            var query = await (from usuarios in context.Usuarios
                               where usuarios.Identificacion == user.Identificacion
                               select new
                               {
                                   usuarios.CorreoElectronico
                               }).ToListAsync();

            string token = "";

            Random random = new Random();
            string numeros = "0123456789";

            int index;
            for (int i = 0; i < 6; i++)
            {
                index = random.Next(numeros.Length);
                token += numeros.Substring(index, 1);
            }

            var query2 = await (from tokens in context.Tokens
                                where tokens.Identificacion == user.Identificacion
                                select new
                                {
                                    tokens.TokenId
                                }).ToListAsync();

            Token itoken = new Token();
            if (query2.Count > 0)
            {
                itoken.TokenId = query2[0].TokenId;
                itoken.Token1 = token;
                itoken.FechaSolicitud = DateTime.Now;
                itoken.DuracionId = 1;
                itoken.Identificacion = user.Identificacion;

                context.Tokens.Update(itoken);
                await context.SaveChangesAsync();
            }
            else
            {
                itoken.Token1 = token;
                itoken.FechaSolicitud = DateTime.Now;
                itoken.DuracionId = 1;
                itoken.Identificacion = user.Identificacion;

                await context.Tokens.AddAsync(itoken);
                await context.SaveChangesAsync();
            }

            return Results.Ok(new { user.Identificacion, query[0].CorreoElectronico, token = token });
        }
        return Results.NotFound(new { codigo = 404, mensaje = "Usuario y/o contraseña incorrectos" });
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

// Consulta de servidores por nombre
app.MapGet("/prueba", async (Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        var gg = await context.Tokens.FindAsync(4);

        DateTime prueba = (DateTime)gg.FechaSolicitud;        
        DateTime tokenLimite = prueba.AddMinutes(2);

        string mensaje = "siuuu";

        if (tokenLimite > DateTime.Now)
        {
            mensaje = "siuuu";
        }
        else
        {
            mensaje = "nooo";
        }

        return Results.Ok(new { mensaje });
    }
    catch (System.Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}