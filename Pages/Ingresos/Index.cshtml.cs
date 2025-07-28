using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AppGastos.Models;
using AppGastos.Data;
using Microsoft.EntityFrameworkCore;

namespace AppGastos.Pages
{
    public class IndexIngresoModel : PageModel
    {
        public readonly AppGastosContext _context;
        public IndexIngresoModel(AppGastosContext context)
        {
            _context = context;
        }

        
        [BindProperty(SupportsGet = true)]
        public string? Pagina { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Busqueda { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Filtro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Orden { get; set; }
        public void OnGet()
        {
            // Obtener también los objetos Empresa y Tipo relacionados
            IQueryable<Ingreso> ingreso = _context.Ingreso
            .Include(i => i.Empresa)
            .Include(i => i.Tipo);

            // Orden
            if (string.IsNullOrEmpty(Orden))
            {
                ingreso = ingreso.OrderByDescending(i => i.Fecha);
                ViewData["Orden"] = "Fecha (descendente)";
            }
            else
            {
                if (Orden == "FechaDesc")
                {
                    ingreso = ingreso.OrderByDescending(i => i.Fecha);
                    ViewData["Orden"] = "Fecha (descendente)";
                }
                else if (Orden == "FechaAsc")
                {
                    ingreso = ingreso.OrderBy(i => i.Fecha);
                    ViewData["Orden"] = "Fecha (ascendente)";
                }
                else if (Orden == "EmpresaDesc")
                {
                    ingreso = ingreso.OrderByDescending(i => i.Empresa.Nombre);
                    ViewData["Orden"] = "Empresa (descendente)";
                }
                else if (Orden == "EmpresaAsc")
                {
                    ingreso = ingreso.OrderBy(i => i.Empresa.Nombre);
                    ViewData["Orden"] = "Empresa (ascendente)";
                }
                else if (Orden == "MontoDesc")
                {
                    ingreso = ingreso.OrderByDescending(i => i.Monto);
                    ViewData["Orden"] = "Monto (descendente)";
                }
                else if (Orden == "MontoAsc")
                {
                    ingreso = ingreso.OrderBy(i => i.Monto);
                    ViewData["Orden"] = "Monto (ascendente)";
                }
                else if (Orden == "TipoDesc")
                {
                    ingreso = ingreso.OrderByDescending(i => i.Tipo.Nombre);
                    ViewData["Orden"] = "Tipo (descendente)";
                }
                else if (Orden == "TipoAsc")
                {
                    ingreso = ingreso.OrderBy(i => i.Tipo.Nombre);
                    ViewData["Orden"] = "Tipo (ascendente)";
                }
                else if (Orden == "DescripcionDesc")
                {
                    ingreso = ingreso.OrderByDescending(i => i.Descripcion);
                    ViewData["Orden"] = "Descripción (descendente)";
                }
                else if (Orden == "DescripcionAsc")
                {
                    ingreso = ingreso.OrderBy(i => i.Descripcion);
                    ViewData["Orden"] = "Descripción (ascendente)";
                }
                else
                {
                    ingreso = ingreso.OrderByDescending(i => i.Fecha);
                    ViewData["Orden"] = "Fecha (descendente)";
                }
            }

            // Búsqueda y filtros
            float monto;
            DateTime fecha;
            bool encontrado = true;

            if (!string.IsNullOrEmpty(Busqueda) && !string.IsNullOrEmpty(Filtro))
            {
                if (Filtro == "fecha" && DateTime.TryParse(Busqueda, out fecha))
                {
                    ingreso = ingreso.Where(i => i.Fecha.Date == fecha.Date);
                }
                else if (Filtro == "empresa")
                {
                    ingreso = ingreso.Where(i => i.Empresa.Nombre.ToLower().Contains(Busqueda.ToLower()));
                }
                else if (Filtro == "tipo")
                {
                    ingreso = ingreso.Where(i => i.Tipo.Nombre.ToLower().Contains(Busqueda.ToLower()));
                }
                else if (Filtro == "monto" && float.TryParse(Busqueda, out monto))
                {
                    ingreso = ingreso.Where(i => i.Monto == monto);
                }
                else if (Filtro == "descripcion")
                {
                    ingreso = ingreso.Where(i => i.Descripcion.ToLower().Contains(Busqueda.ToLower()));
                }
                else
                {
                    encontrado = false;
                }

                if (encontrado)
                {
                    ViewData["Filtro"] = Filtro;
                    ViewData["Buscando"] = Busqueda;
                }
            }

            // Página
            int pag, cantPaginas;
            cantPaginas = ingreso.Count() / 10 + 1;

            if (!String.IsNullOrEmpty(Pagina) && int.TryParse(Pagina, out pag))
            {
                ingreso = ingreso.Skip(pag * 10 - 10).Take(10);
            }
            else
            {
                ingreso = ingreso.Take(10);
            }

            // Objetos para la vista
            ViewData["CantidadPaginas"] = cantPaginas;
            ViewData["Ingresos"] = ingreso.ToList();
            ViewData["Empresas"] = _context.Empresa.ToList();
            ViewData["Tipos"] = _context.Tipo.ToList();
        }

        public IActionResult OnGetProbando()
        {
            var Message = "Probando la acción GET";
            return new JsonResult(new { message = Message });
        }

        public IActionResult OnPost()
        {
            // Método para añadir un nuevo ingreso
            var ingreso = new Ingreso
            {
                Id_ingreso = 0,
                Fecha = DateTime.Parse(Request.Form["fecha"]),
                Monto = ObtenerMonto(Request.Form["monto"]),
                Descripcion = Request.Form["descripcion"],
                Id_tipo = int.Parse(Request.Form["tipo"])
            };

            // Controlar empresa
            if (Request.Form["empresa"].Any())
            {
                ingreso.Id_empresa = int.Parse(Request.Form["empresa"]);
            }
            else
            {
                ingreso.Id_empresa = null;
            }

            _context.Ingreso.Add(ingreso);
            _context.SaveChanges();

            return RedirectToPage("/Ingresos/Index");
        }

        public IActionResult OnPostEditar()
        {
            // Editar el ingreso según su ID
            var ingreso = _context.Ingreso.Find(int.Parse(Request.Form["id"]));

            if (ingreso == null)
            {
                return NotFound();
            }
            else
            {
                ingreso.Fecha = DateTime.Parse(Request.Form["fecha"]);
                ingreso.Monto = ObtenerMonto(Request.Form["monto"]);
                ingreso.Id_tipo = int.Parse(Request.Form["tipo"]);
                ingreso.Descripcion = Request.Form["descripcion"];

                // Controlar empresa
                if (Request.Form["empresa"].Any())
                {
                    ingreso.Id_empresa = int.Parse(Request.Form["empresa"]);
                }
                else
                {
                    ingreso.Id_empresa = null;
                }
            }

            _context.Ingreso.Update(ingreso);
            _context.SaveChanges();

            return RedirectToPage("/Ingresos/Index");
        }

        public IActionResult OnPostEliminar()
        {
            // Eliminar el ingreso según su ID
            int id = int.Parse(Request.Form["id"]);
            var ingreso = _context.Ingreso.Find(id);

            if (ingreso != null)
            {
                _context.Ingreso.Remove(ingreso);
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage("/Ingresos/Index");
        }

        private float ObtenerMonto(string monto)
        {
            var numDecimal = monto.Split('.');
            float resultado = float.Parse(numDecimal[0]) + float.Parse(numDecimal[1]) / 100;

            return resultado;
        }

        public string Recortar(string texto, int longitud)
        {
            if (longitud >= texto.Length)
            {
                return texto;
            }

            string textoNuevo = "";
            int i = 0;

            for (i = 0; i < longitud; i++)
            {
                textoNuevo += texto[i];
            }

            if (i == longitud)
            {
                textoNuevo += "...";
            }

            return textoNuevo;
        }
    }
}
