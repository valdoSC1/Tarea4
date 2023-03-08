using System;
using System.Collections.Generic;

namespace API.Tarea4.Models;

public partial class Telefono
{
    public int TelefonoId { get; set; }

    public int ContactoId { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public virtual ContactoUsuario Contacto { get; set; } = null!;
}
