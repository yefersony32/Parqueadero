using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParqueaderoAPI.Models
{
    public class Vehiculo
    {
        [Key]
        public int VehiculoID { get; set; }

        [Required]
        public string Placa { get; set; }

        [Required]
        public string Tipo { get; set; } // Carro o Moto

        [Required]
        public string Marca { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }

        public Cliente? Cliente { get; set; } // Permite nulo para evitar problemas en API
    }
}
