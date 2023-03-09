using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class TokenAdmin
{
    public int? TokenId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El token es requerido")]
    public string? Token { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de solicitud es requerida")]
    public DateTime? FechaSolicitud { get; set; }

    [Required(ErrorMessage = "El id de la duración es requerido")]
    public int? DuracionId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El código de usuario es requerido")]
    public string? CodigoUsuario { get; set; } = null!;

    [JsonIgnore]
    public virtual UsuariosAdministradore CodigoUsuarioNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual DuracionToken Duracion { get; set; } = null!;
}
