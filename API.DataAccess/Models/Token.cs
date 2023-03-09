using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class Token
{
    [JsonIgnore]
    public int? TokenId { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "El token es requerido")]
    public string? Token1 { get; set; } = null!;

    [JsonIgnore]
    public DateTime? FechaSolicitud { get; set; }

    [JsonIgnore]
    public int? DuracionId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "El identificación es requerida")]
    public string? Identificacion { get; set; } = null!;

    [JsonIgnore]
    public virtual DuracionToken Duracion { get; set; } = null!;

    [JsonIgnore]
    public virtual Usuario IdentificacionNavigation { get; set; } = null!;
}
