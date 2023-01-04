namespace AutoCar.Entities
{
    public class Mark
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int CarId { get; set; }
        public virtual Car Car{ get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
