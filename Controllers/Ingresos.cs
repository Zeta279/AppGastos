using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGastos.Data;
using AppGastos.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class Ingresos : ControllerBase
    {
        private readonly AppGastosContext _context;

        public Ingresos(AppGastosContext context)
        {
            _context = context;
        }

        public List<Ingreso> Get()
        {
            var ingreso = _context.Ingreso
                .Include(i => i.Empresa)
                .Include(i => i.Tipo)
                .OrderByDescending(i => i.Fecha)
                .Take(10)
                .ToList();

            return ingreso;
        }

        [HttpGet("page/{page}")]
        public List<Ingreso> GetByPage(int page)
        {
            var ingreso = _context.Ingreso
                .Include(i => i.Empresa)
                .Include(i => i.Tipo)
                .OrderByDescending(i => i.Fecha)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            return ingreso;
        }

        [HttpGet("{id}")]
        public Ingreso Get(int id)
        {
            var ingreso = _context.Ingreso
                .Include(i => i.Empresa)
                .Include(i => i.Tipo)
                .FirstOrDefault(i => i.Id_empresa == id);

            return ingreso;
        }

        [HttpGet("filter")]
        public List<Ingreso> GetByFilter(string filtro, string? busqueda1, string? busqueda2)
        {
            IQueryable<Ingreso> ingreso = _context.Ingreso
                .Include(i => i.Empresa)
                .Include(i => i.Tipo);

            if (!string.IsNullOrEmpty(filtro) && !string.IsNullOrEmpty(busqueda1))
            {
                if (filtro == "Fecha")
                {
                    ingreso = ingreso.Where(i => i.Fecha == DateTime.Parse(busqueda1));
                }
                if (filtro == "FechaDespues")
                {
                    ingreso = ingreso.Where(i => i.Fecha >= DateTime.Parse(busqueda1));
                }
                else if (filtro == "FechaAntes")
                {
                    ingreso = ingreso.Where(i => i.Fecha <= DateTime.Parse(busqueda2));
                }
                else if (filtro == "FechaEntre")
                {
                    ingreso = ingreso.Where(i => i.Fecha >= DateTime.Parse(busqueda1) && i.Fecha <= DateTime.Parse(busqueda2));
                }
                else if (filtro == "MontoMayor")
                {
                    ingreso = ingreso.Where(i => i.Monto >= float.Parse(busqueda1));
                }
                else if (filtro == "MontoMenor")
                {
                    ingreso = ingreso.Where(i => i.Monto <= float.Parse(busqueda2));
                }
                if (filtro == "Empresa")
                {
                    ingreso = ingreso.Where(i => i.Empresa.Nombre.Contains(busqueda1));
                }
                else if (filtro == "Tipo")
                {
                    ingreso = ingreso.Where(i => i.Tipo.Nombre.Contains(busqueda1));
                }
                else if (filtro == "Monto")
                {
                    ingreso = ingreso.Where(i => i.Monto == float.Parse(busqueda1));
                }
                else
                {
                    ingreso = ingreso.Where(i => i.Descripcion.Contains(busqueda1));
                }
            }

            return ingreso.Take(10).ToList();
        }
    }
}
