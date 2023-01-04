using AutoCar.Entities;
using AutoCar.Services;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.InteropServices;
using AutoCar.Models.DTO;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarkController : ControllerBase
    {
        private readonly IMarkService _markService;
        public MarkController(IMarkService markService)
        {
            _markService = markService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetMarks()
        {
            var marks = _markService.GetMarks();
            return Ok(marks);
        }

        [HttpGet("{carId}")]
        [Authorize]
        public ActionResult GetMarksByCarId([FromRoute] int carId)
        {
            var marks = _markService.GetMarksByCarId(carId);
            return Ok(marks);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddNewMark([FromBody] MarkDto dto)
        {
            return Ok();
        }
    }
}
