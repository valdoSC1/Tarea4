using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DataAccess.Models;

public partial class DuracionToken
{
    [Required(ErrorMessage = "La el ID de la duracuión es requerido")]
    public int? DuracionId { get; set; }

    [Required(ErrorMessage = "La duración es requerida")]
    public int? Duracion { get; set; }

    [JsonIgnore]
    public virtual ICollection<TokenAdmin> TokenAdmins { get; } = new List<TokenAdmin>();

    [JsonIgnore]
    public virtual ICollection<Token> Tokens { get; } = new List<Token>();
}
