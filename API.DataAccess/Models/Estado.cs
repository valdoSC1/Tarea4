using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class Estado
{
    public int EstadoId { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();

    public virtual ICollection<UsuariosAdministradore> UsuariosAdministradores { get; } = new List<UsuariosAdministradore>();
}
