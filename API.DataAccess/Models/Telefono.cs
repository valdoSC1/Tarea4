using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class Telefono
{
    public int TelefonoId { get; set; }

    public int ContactoId { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public virtual ContactoUsuario Contacto { get; set; } = null!;
}
