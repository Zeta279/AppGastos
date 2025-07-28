using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGastos.Models;

public class Ingreso
{
    [Key]
    public int Id_ingreso { get; set; }
    [Required]
    public DateTime Fecha { get; set; }
    [Required]
    public float Monto { get; set; }
    public string? Descripcion { get; set; }

    [ForeignKey("Empresa")]
    public int? Id_empresa { get; set; }
    public Empresa? Empresa { get; set; }

    [Required]
    [ForeignKey("Tipo")]
    public int Id_tipo { get; set; }
    public Tipo? Tipo { get; set; }
}