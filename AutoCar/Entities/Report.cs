using System.Runtime.InteropServices;
using System;

namespace AutoCar.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool Ready { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
