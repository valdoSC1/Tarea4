using API.DataAccess;
using API.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
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

//Login usuarios
app.MapPost("/login", async (Usuario user, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        if (user.Contrasena == null)
        {
            user.Contrasena = "";
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

//Obtener datos de los contactos de un usuario en particular
app.MapGet("contactosUsuario/{idUsuario}", async ([FromHeader] string token, [FromHeader] string identificacion, string idUsuario, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        if (String.IsNullOrEmpty(token))
        {
            return Results.Json(new { mensaje = "El token es requerido" },
                statusCode: StatusCodes.Status500InternalServerError);
        }

        if (string.IsNullOrEmpty(identificacion))
        {
            return Results.BadRequest(new { mensaje = "La identificación es requerida" });
        }

        var validateToken = await (from tokens in context.Tokens
                                   join duracion in context.DuracionTokens
                                   on tokens.DuracionId equals duracion.DuracionId
                                   where tokens.Identificacion == identificacion && tokens.Token1 == token
                                   select new
                                   {
                                       tokens.TokenId,
                                       tokens.FechaSolicitud,
                                       duracion.Duracion
                                   }).ToListAsync();

        if (validateToken.Count > 0)
        {
            ValidarToken iValidarToken = new ValidarToken((DateTime)validateToken[0].FechaSolicitud, (int)validateToken[0].Duracion);
            bool valido = iValidarToken.validacion();

            if (valido)
            {
                Token itoken = new Token();
                itoken.TokenId = validateToken[0].TokenId;
                itoken.Token1 = token;
                itoken.FechaSolicitud = DateTime.Now;
                itoken.DuracionId = 1;
                itoken.Identificacion = identificacion;

                context.Tokens.Update(itoken);
                await context.SaveChangesAsync();

                var query = await (from ContactoUsuario in context.ContactoUsuarios
                                   join Telefono in context.Telefonos on ContactoUsuario.ContactoId equals Telefono.ContactoId
                                   join Correo in context.Correos on ContactoUsuario.ContactoId equals Correo.ContactoId
                                   where ContactoUsuario.UsuarioId == idUsuario
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
            else
            {
                return Results.Json(new { mensaje = "El token ha expirado" },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        else
        {
            return Results.Ok();
        }
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

//Obtener datos de un contacto en particular
app.MapGet("contactos/{id}", async ([FromHeader] string token, [FromHeader] string identificacion, int id, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        if (String.IsNullOrEmpty(token))
        {
            return Results.Json(new { mensaje = "El token es requerido" },
                statusCode: StatusCodes.Status500InternalServerError);
        }

        if (string.IsNullOrEmpty(identificacion))
        {
            return Results.BadRequest(new { mensaje = "La identificación es requerida" });
        }

        var validateToken = await (from tokens in context.Tokens
                                   join duracion in context.DuracionTokens
                                   on tokens.DuracionId equals duracion.DuracionId
                                   where tokens.Identificacion == identificacion && tokens.Token1 == token
                                   select new
                                   {
                                       tokens.TokenId,
                                       tokens.FechaSolicitud,
                                       duracion.Duracion
                                   }).ToListAsync();

        if (validateToken.Count > 0)
        {
            ValidarToken iValidarToken = new ValidarToken((DateTime)validateToken[0].FechaSolicitud, (int)validateToken[0].Duracion);
            bool valido = iValidarToken.validacion();

            if (valido)
            {
                Token itoken = new Token();
                itoken.TokenId = validateToken[0].TokenId;
                itoken.Token1 = token;
                itoken.FechaSolicitud = DateTime.Now;
                itoken.DuracionId = 1;
                itoken.Identificacion = identificacion;

                context.Tokens.Update(itoken);
                await context.SaveChangesAsync();

                var idContacto = await context.ContactoUsuarios.FindAsync(id);
                if (idContacto == null)
                {
                    return Results.NotFound(new { codigo = 404, mensaje = "No se encontró el contacto" });
                }

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
            else
            {
                return Results.Json(new { mensaje = "El token ha expirado" },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        else
        {
            return Results.Ok();
        }
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});


/*Contactos*/
app.MapPost("/registroContacto", async ([FromBody] ContactoUsuario Contacto, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        if (!MiniValidator.TryValidate(Contacto, out var errors))
        {
            return Results.BadRequest(new { id = 400, mensaje = "Datos incorrectos", errores = errors });
        }

        if (await context.ContactoUsuarios.AnyAsync(c => c.Facebook == Contacto.Facebook || c.Twitter == Contacto.Twitter && c.Instagram == Contacto.Instagram))
        {
            return Results.Conflict(new { codigo = 409, mensaje = "Ya existe el contacto" });
        }
        else
        {
            await context.ContactoUsuarios.AddAsync(Contacto);
            await context.SaveChangesAsync();

            return Results.Created($"/registroContacto/ {Contacto.ContactoId}", new
            {
                mensaje = "Creación exitosa",
                contacto = Contacto
            });
        }
    }
    catch (System.Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

// Eliminar un contacto
app.MapDelete("/contacto/{idContacto}", async ([FromHeader] string token, [FromHeader] string identificacion, int idContacto, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        if (String.IsNullOrEmpty(token))
        {
            return Results.Json(new { mensaje = "El token es requerido" },
                statusCode: StatusCodes.Status500InternalServerError);
        }

        if (string.IsNullOrEmpty(identificacion))
        {
            return Results.BadRequest(new { mensaje = "La identificación es requerida" });
        }

        var validateToken = await (from tokens in context.Tokens
                                   join duracion in context.DuracionTokens
                                   on tokens.DuracionId equals duracion.DuracionId
                                   where tokens.Identificacion == identificacion && tokens.Token1 == token
                                   select new
                                   {
                                       tokens.TokenId,
                                       tokens.FechaSolicitud,
                                       duracion.Duracion
                                   }).ToListAsync();

        if (validateToken.Count > 0)
        {
            ValidarToken iValidarToken = new ValidarToken((DateTime)validateToken[0].FechaSolicitud, (int)validateToken[0].Duracion);
            bool valido = iValidarToken.validacion();

            if (valido)
            {
                Token itoken = new Token();
                itoken.TokenId = validateToken[0].TokenId;
                itoken.Token1 = token;
                itoken.FechaSolicitud = DateTime.Now;
                itoken.DuracionId = 1;
                itoken.Identificacion = identificacion;

                context.Tokens.Update(itoken);
                await context.SaveChangesAsync();

                var contactoID = await context.ContactoUsuarios.FindAsync(idContacto);
                if (contactoID == null)
                {
                    return Results.NotFound(new { codigo = 404, mensaje = "No se encontró el contacto" });
                }

                var query = await (from Telefono in context.Telefonos
                                   where Telefono.ContactoId == idContacto
                                   select new
                                   {
                                       Telefono.TelefonoId
                                   }).ToListAsync();

                var query2 = await (from Correo in context.Correos
                                    where Correo.ContactoId == idContacto
                                    select new
                                    {
                                        Correo.CorreoId
                                    }).ToListAsync();


                for (int i = 0; i < query.Count(); i++)
                {
                    var telefono = await context.Telefonos.FindAsync(query[i].TelefonoId);
                    context.Remove(telefono);
                    await context.SaveChangesAsync();
                }

                for (int i = 0; i < query2.Count(); i++)
                {
                    var correo = await context.Correos.FindAsync(query2[i].CorreoId);
                    context.Remove(correo);
                    await context.SaveChangesAsync();
                }

                context.Remove(contactoID);
                await context.SaveChangesAsync();
                return Results.Ok();
            }
            else
            {
                return Results.Json(new { mensaje = "El token ha expirado" },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        else
        {
            return Results.Ok();
        }
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
                    statusCode: StatusCodes.Status500InternalServerError);
    }
});

app.MapPost("/correo", async (Correo Correo, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        ArrayList mensajeError = new ArrayList();
        if (!MiniValidator.TryValidate(Correo, out var errors))
        {
            return Results.BadRequest(new { id = 400, mensaje = "Datos incorrectos", errores = errors });
        }


        if (await context.Correos.AnyAsync(c => c.CorreoElectronico == Correo.CorreoElectronico))
        {
            return Results.Conflict(new { codigo = 409, mensaje = "Ya existe el correo." });
        }
        else
        {
            await context.Correos.AddAsync(Correo);
            await context.SaveChangesAsync();
            //var miContacto = await context.Correos.FirstAsync(c => c.CorreoElectronico == Correo.CorreoElectronico);
            return Results.NoContent();
        }
    }
    catch (System.Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
});

app.MapPost("/telefono", async (Telefono Telef, Tiusr4plMohisatarea4Context context) =>
{
    try
    {
        ArrayList mensajeError = new ArrayList();
        if (!MiniValidator.TryValidate(Telef, out var errors))
        {
            return Results.BadRequest(new { id = 400, mensaje = "Datos incorrectos", errores = errors });
        }

        if (await context.Telefonos.AnyAsync(c => c.NumeroTelefono == Telef.NumeroTelefono))// Se puede agregar el id para hacerlo mas unico
        {
            return Results.Conflict(new { codigo = 409, mensaje = "Ya existe el telefono." });
        }
        else
        {
            await context.Telefonos.AddAsync(Telef);
            await context.SaveChangesAsync();
            //var miContacto = await context.Telefonos.FirstAsync(c => c.NumeroTelefono == Telef.NumeroTelefono);
            return Results.NoContent();
        }
    }
    catch (System.Exception exc)
    {
        return Results.Json(new { codigo = 500, mensaje = exc.Message },
            statusCode: StatusCodes.Status500InternalServerError);
    }
}); ;

app.Run();

internal record ValidarToken(DateTime fechaSolicitud, int duracionToken)
{
    public bool validacion()
    {
        DateTime tokenLifetime = fechaSolicitud.AddMinutes(duracionToken);

        if (tokenLifetime > DateTime.Now)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}