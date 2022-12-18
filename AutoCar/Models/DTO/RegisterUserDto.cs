namespace AutoCar.Models.DTO
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }
    }
}
