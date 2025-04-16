namespace ParqueaderoAPI.Models
{
    public class Espacio
    {
        public int EspacioID { get; set; }
        public string Nomenclatura { get; set; } 
        public int Piso { get; set; }
        public string Zona { get; set; }
        public string Estado { get; set; } // Disponible, Ocupado, Reservado
    }
}
