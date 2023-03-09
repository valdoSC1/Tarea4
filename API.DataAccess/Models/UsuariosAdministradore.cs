using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class UsuariosAdministradore
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El código de usuario es requerido")]
    public string? CodigoUsuario { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
    public string? Contrasena { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido")]
    public string? CorreoElectronico { get; set; } = null!;

    [Required(ErrorMessage = "El id del estado es requerido")]
    public int? EstadoId { get; set; }

    [JsonIgnore]
    public virtual Estado Estado { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TokenAdmin> TokenAdmins { get; } = new List<TokenAdmin>();
}
