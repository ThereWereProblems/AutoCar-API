using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace AutoCar.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int FuelLevel { get; set; }
        public string FuelType { get; set; }
        public DateTime RegistrationReview { get; set; }
        public DateTime Insurance { get; set; }
        public DateTime TireChange { get; set; }
        public int NotificationsYellow;
        public int NotificationsRed;
        public int? UserId;
        public User User;
        public List<Reservation> Reservations;
        public bool Opened;
    }
}
