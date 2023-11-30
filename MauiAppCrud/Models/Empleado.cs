using System.ComponentModel.DataAnnotations;

namespace MauiAppCrud.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public decimal Sueldo { get; set; }

        public DateTime FechaContrato { get; set; }
    }
}
