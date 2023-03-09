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

//Verbo get para consultar servicios
app.MapGet("/servicio", async (Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        DatosSQL cadx = new DatosSQL();

        List<SqlParameter> lstParametros = new List<SqlParameter>();

        lstParametros.Add(new SqlParameter("@idUsuario", "305380744"));
        lstParametros.Add(new SqlParameter("@idContacto", 1));

        //var datos = cadx.ExecuteSPWithDT("SP_ConsultarContactos", lstParametros);

        var datosa = await context.ContactoUsuarios
                .FromSqlRaw($"SP_ConsultarContactos {"305380744"}, {1}")
                .ToListAsync();

        //ArrayList datosRespuesta = new ArrayList();

        List<ContactoUsuario> datosRespuesta = new List<ContactoUsuario>();

        //drUsuario As DataRow In dtUsuarios.Rows
        //        .CodigoUsuario = drUsuario("CodigoUsuario"),
        //.Nombre = drUsuario("Nombre"),
        //.Apellidos = drUsuario("Apellidos"),
        //.Correo = drUsuario("Correo"),
        //.RolSistema = drUsuario("Rol"),
        //.Password = drUsuario("Contraseña")

        /*
        foreach (DataRow drContacto in datos.Rows)
        {
            datosRespuesta.Add(new ContactoUsuario
            {
                ContactoId = (int)drContacto["IdContacto"],
                Nombre = (string)drContacto["Nombre"]
            });

            //ContactoUsuario iContacto = new ContactoUsuario();
            //iContacto.ContactoId = (int)drContacto["IdContacto"];
            //iContacto.Nombre = ;

            //datosRespuesta.Add(iContacto);
        }
        */
        return Results.Ok(datosa);
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