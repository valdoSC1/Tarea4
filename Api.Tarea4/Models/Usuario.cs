using System;
using System.Collections.Generic;

namespace API.Tarea4.Models;

public partial class Usuario
{
    public string Identificacion { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string SegundoApellido { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int EstadoId { get; set; }

    public string CorreoElectronico { get; set; } = null!;

    public virtual ICollection<ContactoUsuario> ContactoUsuarios { get; } = new List<ContactoUsuario>();

    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<Token> Tokens { get; } = new List<Token>();
}
