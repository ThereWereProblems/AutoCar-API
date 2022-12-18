using System.ComponentModel.DataAnnotations;

namespace AutoCar.Models.DTO
{
    public class NewPasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
