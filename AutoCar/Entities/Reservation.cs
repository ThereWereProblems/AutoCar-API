using System;

namespace AutoCar.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CarId { get; set; }
        public virtual Car Car { get; set; }
        public DateTime Dos { get; set; }
        public DateTime Doe { get; set; }
        public bool Finished { get; set; }

    }
}
