using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class Correo
{
    public int CorreoId { get; set; }

    public int ContactoId { get; set; }

    public string CorreoElectronico { get; set; } = null!;

    public virtual ContactoUsuario Contacto { get; set; } = null!;
}
