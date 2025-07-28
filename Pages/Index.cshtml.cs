using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppGastos.Models;
using AppGastos.Data;
using Microsoft.EntityFrameworkCore;

namespace AppGastos.Pages;

public class IndexModel : PageModel
{
    public readonly AppGastosContext _context;

    public IndexModel(AppGastosContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        float ingresos = 0, egresos = 0;
        var ingreso = _context.Ingreso
            .Include(i => i.Empresa)
            .Include(i => i.Tipo)
            .OrderByDescending(i => i.Fecha);

        foreach (var i in ingreso)
        {
            if (i.Monto > 0)
            {
                ingresos += i.Monto;
            }
            else
            {
                egresos += i.Monto;
            }
        }

        ViewData["Ingresos"] = ingreso.Take(3).ToList();
        ViewData["CantIngresos"] = ingresos.ToString("C");
        ViewData["CantEgresos"] = egresos.ToString("C");
        ViewData["Balance"] = (ingresos + egresos).ToString("C");
    }

    public IActionResult OnPost()
    {
        if (!_context.Ingreso.Any())
        {
            _context.Ingreso.Add(new Ingreso
            {
                Id_ingreso = 1,
                Fecha = DateTime.Now,
                Monto = 500f,
                Descripcion = "Retiro de paquete",
                Id_empresa = 1,
                Id_tipo = 1
            });

            _context.Ingreso.Add(new Ingreso
            {
                Id_ingreso = 2,
                Fecha = DateTime.Now,
                Monto = 1200f,
                Descripcion = "Retiro de paquete",
                Id_empresa = 1,
                Id_tipo = 1
            });

            _context.Ingreso.Add(new Ingreso
            {
                Id_ingreso = 3,
                Fecha = DateTime.Now,
                Monto = 500f,
                Descripcion = "Retiro de paquete",
                Id_empresa = 2,
                Id_tipo = 1
            });

            _context.Ingreso.Add(new Ingreso
            {
                Id_ingreso = 4,
                Fecha = DateTime.Now,
                Monto = 800f,
                Descripcion = "Fotocopiado",
                Id_tipo = 2
            });
        }

        _context.SaveChanges();

        return RedirectToPage("Index");
    }
}
