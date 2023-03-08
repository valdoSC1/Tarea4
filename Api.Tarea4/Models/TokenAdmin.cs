using System;
using System.Collections.Generic;

namespace API.Tarea4.Models;

public partial class TokenAdmin
{
    public int TokenId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime FechaSolicitud { get; set; }

    public int DuracionId { get; set; }

    public string CodigoUsuario { get; set; } = null!;

    public virtual UsuariosAdministradore CodigoUsuarioNavigation { get; set; } = null!;

    public virtual DuracionToken Duracion { get; set; } = null!;
}
