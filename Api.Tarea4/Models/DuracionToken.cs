using System;
using System.Collections.Generic;

namespace API.Tarea4.Models;

public partial class DuracionToken
{
    public int DuracionId { get; set; }

    public int Duracion { get; set; }

    public virtual ICollection<TokenAdmin> TokenAdmins { get; } = new List<TokenAdmin>();

    public virtual ICollection<Token> Tokens { get; } = new List<Token>();
}
