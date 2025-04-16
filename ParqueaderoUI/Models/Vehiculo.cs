namespace ParqueaderoUI.Models
{
    public class Vehiculo
    {
        public int VehiculoID { get; set; }
        public string Placa { get; set; }
        public string Tipo { get; set; } // Carro o Moto
        public string Marca { get; set; }
        public string Color { get; set; }
        public int ClienteID { get; set; }
    }
}
