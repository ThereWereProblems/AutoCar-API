using System.ComponentModel.DataAnnotations;

namespace AutoCar.Models.DTO
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
