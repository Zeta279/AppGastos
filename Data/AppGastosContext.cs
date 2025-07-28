using Microsoft.EntityFrameworkCore;
using AppGastos.Models;

namespace AppGastos.Data
{
    public class AppGastosContext : DbContext
    {
        public AppGastosContext(DbContextOptions<AppGastosContext> options) : base(options) { }

        public DbSet<Ingreso> Ingreso { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
    }
}