using API.DataAccess;
using API.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
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

//Login
app.MapPost("/login", async (Usuario user, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        /*
        string result = string.Empty;
        byte[] OcultarString = System.Text.Encoding.Unicode.GetBytes(user.Contrasena);
        user.Contrasena = Convert.ToBase64String(OcultarString);

        if (String.IsNullOrEmpty(user.NombreUsuario) || String.IsNullOrEmpty(user.Contrasena))
        {
            return Results.BadRequest(new { id = 400, mensaje = "Datos incorrectos" });
        }
        if (await context.Usuarios.AnyAsync<Usuario>(x => x.NombreUsuario == user.NombreUsuario && x.Contrasena == user.Contrasena))
        {
            return Results.Ok(new { nombre = user.NombreUsuario });
        }
        return Results.NotFound(new { codigo = 404, mensaje = "No se encontró el usuario." });
        */
        return Results.Ok(await context.ContactoUsuarios.ToListAsync());
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

//Verbo get para consultar los contactos de un usuario especifico
app.MapGet("contactoUsuarios/{usuarioID}", async (string usuarioID, Tiusr4plMohisatarea4Context context) =>
{
    var idUsuario = await context.Usuarios.FindAsync(usuarioID);
    if (idUsuario == null)
    {
        return Results.NotFound(new { codigo = 404, mensaje = "No se encontró el id del usuario." });
    }
    try
    {
        var query = await (from ContactoUsuario in context.ContactoUsuarios
                           join Usuarios in context.Usuarios on ContactoUsuario.UsuarioId equals Usuarios.Identificacion
                           join Telefono in context.Telefonos on ContactoUsuario.ContactoId equals Telefono.ContactoId
                           join Correo in context.Correos on ContactoUsuario.ContactoId equals Correo.ContactoId
                           where ContactoUsuario.UsuarioId == usuarioID
                           select new
                           {
                               ContactoUsuario.Nombre,
                               ContactoUsuario.PrimerApellido,
                               ContactoUsuario.SegundoApellido,
                               ContactoUsuario.Facebook,
                               ContactoUsuario.Instagram,
                               ContactoUsuario.Twitter,
                               Telefono.NumeroTelefono,
                               Correo.CorreoElectronico
                           }).ToListAsync();
        return Results.Ok(query);
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

//Verbo get para obtener datos de un contacto en particular
app.MapGet("contactos/{codigo}", async (int id, Tiusr4plMohisatarea4Context context) =>
{
    var idContacto = await context.ContactoUsuarios.FindAsync(id);
    if (idContacto == null)
    {
        return Results.NotFound(new { codigo = 404, mensaje = "No se encontró el id del código." });
    }
    try
    {
        var query = await (from ContactoUsuario in context.ContactoUsuarios
                           join Telefono in context.Telefonos on ContactoUsuario.ContactoId equals Telefono.ContactoId 
                           join Correo in context.Correos on ContactoUsuario.ContactoId equals Correo.ContactoId
                           where ContactoUsuario.ContactoId == id
                           select new
                           {
                               ContactoUsuario.Nombre,
                               ContactoUsuario.PrimerApellido,
                               ContactoUsuario.SegundoApellido,
                               ContactoUsuario.Facebook,
                               ContactoUsuario.Instagram,
                               ContactoUsuario.Twitter,
                               Telefono.NumeroTelefono,
                               Correo.CorreoElectronico
                           }).ToListAsync();
        return Results.Ok(query);
    }
    catch (Exception exc)
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