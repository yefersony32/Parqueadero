using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueaderoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly ParqueaderoContext _context;

        public ReportesController(ParqueaderoContext context)
        {
            _context = context;
        }

        // 1️⃣ Ranking de clientes frecuentes por eventos
        [HttpGet("RankingClientesPorEventos")]
        public async Task<IActionResult> GetRankingClientesPorEventos()
        {
            var ranking = await _context.Reservas
                .Where(r => r.TipoReserva == "Tiempo")
                .GroupBy(r => r.Vehiculo.ClienteID)
                .Select(g => new
                {
                    ClienteID = g.Key,
                    NombreCliente = _context.Clientes.Where(c => c.ClienteID == g.Key).Select(c => c.Nombre).FirstOrDefault(),
                    NumeroEventos = g.Count()
                })
                .OrderByDescending(x => x.NumeroEventos)
                .ToListAsync();

            return Ok(ranking);
        }

        // 2️⃣ Ranking de clientes frecuentes por tiempo
        [HttpGet("RankingClientesPorTiempo")]
        public async Task<IActionResult> GetRankingClientesPorTiempo()
        {
            var ranking = await _context.Reservas
                .Where(r => r.TipoReserva == "Tiempo" && r.FechaSalida != null)
                .GroupBy(r => r.Vehiculo.ClienteID)
                .Select(g => new
                {
                    ClienteID = g.Key,
                    NombreCliente = _context.Clientes.Where(c => c.ClienteID == g.Key).Select(c => c.Nombre).FirstOrDefault(),
                    TiempoTotal = g.Sum(r => EF.Functions.DateDiffMinute(r.FechaIngreso, r.FechaSalida.Value))
                })
                .OrderByDescending(x => x.TiempoTotal)
                .ToListAsync();

            return Ok(ranking);
        }

        // 3️⃣ Lista de vehículos parqueados en un rango de fechas
        [HttpGet("VehiculosParqueados")]
        public async Task<IActionResult> GetVehiculosParqueados([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var vehiculos = await _context.Reservas
                .Where(r => r.TipoReserva == "Tiempo" && r.FechaIngreso >= fechaInicio && r.FechaIngreso <= fechaFin)
                .OrderByDescending(r => r.FechaIngreso)
                .Select(r => new
                {
                    r.Vehiculo.Placa,
                    r.Vehiculo.Marca,
                    r.Vehiculo.Color,
                    r.FechaIngreso,
                    r.FechaSalida
                })
                .ToListAsync();

            return Ok(vehiculos);
        }

        // 4️⃣ Lista consolidada de eventos diarios en un mes
        [HttpGet("EventosDiarios")]
        public async Task<IActionResult> GetEventosDiarios([FromQuery] int anio, [FromQuery] int mes)
        {
            var eventos = await _context.Reservas
                .Where(r => r.TipoReserva == "Tiempo" && r.FechaIngreso.Year == anio && r.FechaIngreso.Month == mes)
                .GroupBy(r => r.FechaIngreso.Date)
                .Select(g => new
                {
                    Fecha = g.Key,
                    NumeroEventos = g.Count()
                })
                .OrderBy(x => x.Fecha)
                .ToListAsync();

            return Ok(eventos);
        }

        // 5️⃣ Top 15 de espacios más usados
        [HttpGet("TopEspacios")]
        public async Task<IActionResult> GetTopEspacios()
        {
            var usosEspacios = await (from r in _context.Reservas
                                      join e in _context.Espacios on r.EspacioID equals e.EspacioID
                                      where r.EspacioID != null 
                                      group r by new { r.EspacioID, e.Nomenclatura } into g
                                      select new
                                      {
                                          EspacioID = g.Key.EspacioID, 
                                          Nomenclatura = g.Key.Nomenclatura,
                                          VecesUsado = g.Count()
                                      })
                                      .OrderByDescending(x => x.VecesUsado)
                                      .Take(15)
                                      .ToListAsync();

            return Ok(usosEspacios);
        }

        // 6️⃣ Reporte de recaudo diario en un mes (Tipo Tiempo)
        [HttpGet("RecaudoDiarioTiempo")]
        public async Task<IActionResult> GetRecaudoDiarioTiempo([FromQuery] int anio, [FromQuery] int mes)
        {
            var recaudos = await (from p in _context.Pagos
                                  join r in _context.Reservas on p.ReservaID equals r.ReservaID
                                  where p.FechaPago.Year == anio && p.FechaPago.Month == mes
                                        && r.TipoReserva == "Tiempo"
                                  group p by p.FechaPago.Date into g
                                  select new
                                  {
                                      Fecha = g.Key,
                                      TotalRecaudado = g.Sum(p => p.Monto)
                                  })
                                  .OrderBy(x => x.Fecha)
                                  .ToListAsync();

            return Ok(recaudos);
        }

        [HttpGet("RecaudoDiarioArriendo")]
        public async Task<IActionResult> GetRecaudoDiarioArriendo([FromQuery] int anio, [FromQuery] int mes)
        {
            var recaudos = await (from p in _context.Pagos
                                  join r in _context.Reservas on p.ReservaID equals r.ReservaID
                                  where p.FechaPago.Year == anio && p.FechaPago.Month == mes
                                        && r.TipoReserva == "Reserva"
                                  group p by p.FechaPago.Date into g
                                  select new
                                  {
                                      Fecha = g.Key,
                                      TotalRecaudado = g.Sum(p => p.Monto)
                                  })
                                  .OrderBy(x => x.Fecha)
                                  .ToListAsync();

            return Ok(recaudos);
        }

        [HttpGet("ArriendosMorosos")]
        public async Task<IActionResult> GetArriendosMorosos()
        {
            var hoy = DateTime.Today;

            var morosos = await _context.Reservas
                .Where(r => r.TipoReserva == "Reserva") 
                .GroupJoin(_context.Pagos, r => r.ReservaID, p => p.ReservaID, (r, pagos) => new { Reserva = r, Pagos = pagos })
                .Select(t => new
                {
                    Cliente = _context.Vehiculos
                                .Where(v => v.VehiculoID == t.Reserva.VehiculoID)
                                .Join(_context.Clientes, v => v.ClienteID, c => c.ClienteID, (v, c) => c.Nombre)
                                .FirstOrDefault(),
                    Placa = _context.Vehiculos
                                .Where(v => v.VehiculoID == t.Reserva.VehiculoID)
                                .Select(v => v.Placa)
                                .FirstOrDefault(),
                    Espacio = t.Reserva.EspacioID,
                    FechaIngreso = t.Reserva.FechaIngreso, 
                    FechaUltimoPago = t.Pagos
                                .OrderByDescending(p => p.FechaPago)
                                .Select(p => (DateTime?)p.FechaPago)
                                .FirstOrDefault(), 
                    MetodoPago = t.Pagos
                                .OrderByDescending(p => p.FechaPago)
                                .Select(p => p.MetodoPago)
                                .FirstOrDefault() ?? "No registrado", 
                    MontoAdeudado = t.Reserva.Monto ?? 0
                })
                .ToListAsync();

            var resultado = morosos.Select(m => new
            {
                m.Cliente,
                m.Placa,
                m.Espacio,
                FechaUltimoPago = m.FechaUltimoPago.HasValue ? m.FechaUltimoPago.Value.ToString("yyyy-MM-dd") : "",
                DiasMora = m.FechaUltimoPago.HasValue
                    ? (hoy - m.FechaUltimoPago.Value).Days  
                    : (hoy - m.FechaIngreso).Days,          
                MetodoPago = m.MetodoPago, 
                MontoAdeudado = m.MontoAdeudado
            }).OrderByDescending(x => x.DiasMora)
            .ToList();

            return Ok(resultado);
        }

    }
}
