namespace AutoCar.Entities
{
    public class Repair
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public string ServiceNote { get; set; }
        public double? Cost { get; set; }
    }
}
