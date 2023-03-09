using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class ContactoUsuario
{
    public int? ContactoId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El id del usuario es requerido")]
    public string? UsuarioId { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido")]
    public string? Nombre { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El primer apellido es requerido")]
    public string? PrimerApellido { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El segundo apellido es requerido")]
    public string? SegundoApellido { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El Facebook es requerido")]
    public string? Facebook { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El Instagram es requerido")]
    public string? Instagram { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El Twitter es requerido")]
    public string? Twitter { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Correo> Correos { get; } = new List<Correo>();

    [JsonIgnore]
    public virtual ICollection<Telefono> Telefonos { get; } = new List<Telefono>();

    [JsonIgnore]
    public virtual Usuario Usuario { get; set; } = null!;
}
