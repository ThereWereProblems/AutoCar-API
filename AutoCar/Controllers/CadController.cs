using AutoCar.Entities;
using AutoCar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoCar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CadController : Controller
    {
        private readonly ICadService _cadService;

        public CadController(ICadService cadService)
        {
            _cadService = cadService;
        }

        [HttpPost("search")]
        public ActionResult Search([FromBody] string str)
        {
            var dic = _cadService.Search(str);
            return Ok(dic);
        }
    }
}
