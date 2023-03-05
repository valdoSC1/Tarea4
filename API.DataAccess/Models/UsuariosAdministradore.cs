using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class UsuariosAdministradore
{
    public string CodigoUsuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public int EstadoId { get; set; }

    public int? IdToken { get; set; }

    public virtual Estado Estado { get; set; } = null!;

    public virtual Token? IdTokenNavigation { get; set; }
}
