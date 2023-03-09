using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class Estado
{
    public int? EstadoId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del estado es requerido")]
    public string? NombreEstado { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();

    [JsonIgnore]
    public virtual ICollection<UsuariosAdministradore> UsuariosAdministradores { get; } = new List<UsuariosAdministradore>();
}
