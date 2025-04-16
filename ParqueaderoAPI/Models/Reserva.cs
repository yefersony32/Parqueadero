namespace ParqueaderoAPI.Models;
public class Reserva
{
    public int ReservaID { get; set; }
    public int VehiculoID { get; set; }
    public int EspacioID { get; set; }

    public string TipoReserva { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaSalida { get; set; }
    public decimal? Monto { get; set; }

    public Vehiculo? Vehiculo { get; set; }
    public Espacio? Espacio { get; set; }
}
