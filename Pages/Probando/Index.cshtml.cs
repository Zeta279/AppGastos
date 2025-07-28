using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using AppGastos.Data;
using AppGastos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyApp.Namespace
{
    public class IndexProbandoModel : PageModel
    {

        public readonly AppGastosContext _context;
        public IndexProbandoModel(AppGastosContext context)
        {
            _context = context;
        }

        public List<Ingreso> OnGet()
        {
            IQueryable<Ingreso> ingreso = _context.Ingreso
                .Include(i => i.Empresa)
                .Include(i => i.Tipo);
            
            var json = JsonSerializer.Serialize(ingreso.ToList());

            return ingreso.ToList();
        }
    }
}
