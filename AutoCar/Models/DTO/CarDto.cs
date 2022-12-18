using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AutoCar.Models.DTO
{
    public class CarDto
    {
        [Required]
        public string LicensePlate { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int FuelLevel { get; set; }
        [Required]
        public string FuelType { get; set; }
        [Required]
        public DateTime RegistrationReview { get; set; }
        [Required]
        public DateTime Insurance { get; set; }
        [Required]
        public DateTime TireChange { get; set; }
        [Required]
        public int  UserId { get; set; }
    }
}
