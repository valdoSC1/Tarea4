using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class Token
{
    public int IdToken { get; set; }

    public int Token1 { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();

    public virtual ICollection<UsuariosAdministradore> UsuariosAdministradores { get; } = new List<UsuariosAdministradore>();
}
