using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Data;
using ParqueaderoAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueaderoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspaciosController : ControllerBase
    {
        private readonly ParqueaderoContext _context;

        public EspaciosController(ParqueaderoContext context)
        {
            _context = context;
        }

        // GET: api/Espacios/Disponible
        [HttpGet("Disponible")]
        public async Task<ActionResult<Espacio>> ObtenerEspacioDisponible()
        {
            var espacio = await _context.Espacios
                .Where(e => e.Estado == "Disponible")
                .FirstOrDefaultAsync();

            if (espacio == null)
            {
                return NotFound("No hay espacios disponibles.");
            }

            return Ok(espacio);
        }



        [HttpPut("Asignar/{id}")]
        public async Task<IActionResult> AsignarEspacio(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null)
            {
                return NotFound("Espacio no encontrado.");
            }

            if (espacio.Estado != "Disponible")
            {
                return BadRequest("El espacio ya está ocupado.");
            }

            espacio.Estado = "Ocupado";
            await _context.SaveChangesAsync();

            return Ok($"Espacio {espacio.Nomenclatura} asignado correctamente.");
        }

        // GET: api/Espacios/PorPisoZona?piso=1&zona=A
        [HttpGet("PorPisoZona")]
        public async Task<ActionResult<IEnumerable<Espacio>>> ObtenerEspaciosPorPisoZona(int piso, string zona)
        {
            var espacios = await _context.Espacios
                .Where(e => e.Piso == piso && e.Zona == zona)
                .ToListAsync();

            if (!espacios.Any())
            {
                return NotFound("No hay espacios disponibles para el piso y zona seleccionados.");
            }

            return Ok(espacios);
        }

    }


}
