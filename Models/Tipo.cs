using System.ComponentModel.DataAnnotations;

namespace AppGastos.Models;

public class Tipo
{
    [Key]
    public int Id_tipo { get; set; }
    [Required]
    public string Nombre { get; set; }

    public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();

    public override string ToString()
    {
        return Nombre;
    }
}