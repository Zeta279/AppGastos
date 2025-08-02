using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppGastos.Models;

public class Empresa
{
    [Key]
    public int Id_empresa { get; set; }
    [Required]
    public string Nombre { get; set; }

    [JsonIgnore]
    public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();

    public override string ToString()
    {
        return Nombre;
    }
}