namespace ParqueaderoUI.Models
{
    public class Espacio
{
    public int EspacioID { get; set; }
    public string Nomenclatura { get; set; }
    public int Piso { get; set; }
    public string Zona { get; set; } // A o B
    public string Estado { get; set; } // Disponible, Ocupado, Reservado
}
}