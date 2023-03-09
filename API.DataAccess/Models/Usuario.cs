using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class Usuario
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "La identificación es requerida")]
    public string? Identificacion { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido")]
    public string? Nombre { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El primer apellido es requerido")]
    public string? PrimerApellido { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El segundo apellido es requerido")]
    public string? SegundoApellido { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
    public string? Contrasena { get; set; } = null!;

    [Required(ErrorMessage = "El id del estado es requerido")]
    public int? EstadoId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido")]
    public string? CorreoElectronico { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ContactoUsuario> ContactoUsuarios { get; } = new List<ContactoUsuario>();

    [JsonIgnore]
    public virtual Estado Estado { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Token> Tokens { get; } = new List<Token>();
}
