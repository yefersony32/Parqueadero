using System;

namespace ParqueaderoUI.Models
{
    public class Pago
    {
        public int PagoID { get; set; }
        public int ReservaID { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
