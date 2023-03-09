using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class Correo
{
    public int? CorreoId { get; set; }

    [Required(ErrorMessage = "El id del contacto es requerido")]
    public int? ContactoId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido")]
    public string? CorreoElectronico { get; set; } = null!;

    [JsonIgnore]
    public virtual ContactoUsuario Contacto { get; set; } = null!;
}
