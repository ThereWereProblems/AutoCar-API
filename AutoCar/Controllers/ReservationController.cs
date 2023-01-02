using AutoCar.Entities;
using AutoCar.Models.DTO;
using AutoCar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult GetReservations()
        {
            var list = _reservationService.GetReservations();
            return Ok(list);
        }

        [HttpGet("{userId}")]
        public ActionResult GetReservationsByUserId([FromRoute] int userId)
        {
            var list = _reservationService.GetReservationsByUserId(userId);
            return Ok(list);
        }

        [HttpGet("/car/{carId}")]
        public ActionResult GetReservationsByUserIdAndCarId([FromRoute] int carId, [FromHeader] int userId)
        {
            var list = _reservationService.GetReservationsByUserIdAndCarId(userId, carId);
            return Ok(list);
        }

        [HttpPost]
        public ActionResult AddNewReservation([FromBody] ReservationDto reservation)
        {
            int id = _reservationService.AddNewReservation(reservation);
            return Ok(id);
        }

        [HttpPut("{resvId}")]
        public ActionResult UpdateReservation([FromRoute] int resvId, [FromBody] UpdateReservationDto dto)
        {
            _reservationService.UpdateReservation(resvId, dto);
            return Ok();
        }

        [HttpDelete("{resvId}")]
        public ActionResult DeleteReservation([FromRoute] int resvId)
        {
            _reservationService.DeleteReservation(resvId);
            return Ok();
        }

        [HttpPut("finish/{resvId}")]
        public ActionResult ChangeFinished([FromRoute] int resvId)
        {
            var status = _reservationService.ChangeFinished(resvId);
            return Ok(status);
        }
    }
}
