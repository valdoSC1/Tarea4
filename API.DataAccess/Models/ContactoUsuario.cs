using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class ContactoUsuario
{
    public int ContactoId { get; set; }

    public string UsuarioId { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string PrimerApellido { get; set; } = null!;

    public string SegundoApellido { get; set; } = null!;

    public string Facebook { get; set; } = null!;

    public string Instagram { get; set; } = null!;

    public string Twitter { get; set; } = null!;

    public virtual ICollection<Correo> Correos { get; } = new List<Correo>();

    public virtual ICollection<Telefono> Telefonos { get; } = new List<Telefono>();

    public virtual Usuario Usuario { get; set; } = null!;
}
