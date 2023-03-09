using System;
using System.Collections.Generic;

namespace API.DataAccess.Models;

public partial class Token
{
    public int TokenId { get; set; }

    public string Token1 { get; set; } = null!;

    public DateTime FechaSolicitud { get; set; }

    public int DuracionId { get; set; }

    public string Identificacion { get; set; } = null!;

    public virtual DuracionToken Duracion { get; set; } = null!;

    public virtual Usuario IdentificacionNavigation { get; set; } = null!;
}
