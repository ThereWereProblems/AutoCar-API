using Microsoft.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace AutoCar.Models.DTO
{
    public class UpdateCarDto
    {
        public string? LicensePlate { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? FuelLevel { get; set; }
        public string? FuelType { get; set; }
        public DateTime? RegistrationReview { get; set; }
        public DateTime? Insurance { get; set; }
        public DateTime? TireChange { get; set; }
        public int? UserId { get; set; }
    }
}
