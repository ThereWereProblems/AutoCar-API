using AutoCar.Entities;
using AutoCar.Models.DTO;
using AutoCar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairController : Controller
    {
        private readonly IRepairService _repairService;
        public RepairController(IRepairService repairService)
        {
            _repairService = repairService;
        }

        [HttpPost("addrepair")]
        [Authorize]
        public ActionResult OrderRepair([FromBody] RepairDto dto)
        {
            _repairService.OrderRepair(dto);

            return Ok();
        }

        [HttpPost("donerepair/{id}")]
        [Authorize]
        public ActionResult DoRepair([FromRoute] int id, [FromBody] DoRepairDto dto)
        {
            _repairService.DoRepair(id, dto);

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetRepairs()
        {
            var list = _repairService.GetRepairs();

            return Ok(list);
        }

        [HttpGet("todo")]
        [Authorize]
        public ActionResult GetToDoRepairs()
        {
            var list = _repairService.GetToDoRepairs();

            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult GetRepair([FromRoute] int id)
        {
            var rep = _repairService.GetRepair(id);

            return Ok(rep);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult EditRepair([FromRoute] int id, [FromBody] RepairDto dto)
        {
            _repairService.EditRepair(id, dto);

            return Ok();
        }

        [HttpPut("done/{id}")]
        [Authorize]
        public ActionResult EditDoneRepair([FromRoute] int id, [FromBody] DoRepairDto dto)
        {
            _repairService.EditDoneRepair(id, dto);

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public ActionResult Delete([FromRoute] int id)
        {
            _repairService.DeleteRepair(id);

            return NoContent();
        }
    }
}
